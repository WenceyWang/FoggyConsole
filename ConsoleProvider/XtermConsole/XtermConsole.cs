﻿using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . IO ;
using System . Linq ;
using System . Text ;
using System . Threading ;

using DreamRecorder . FoggyConsole . XtermConsole . EscapeSequenceParsers ;

using JetBrains . Annotations ;

using static DreamRecorder . FoggyConsole . XtermConsole . EscapeSequenceParsers . EscapeSequences ;

namespace DreamRecorder . FoggyConsole . XtermConsole
{

	public class XtermConsole : IConsole
	{

		private ConsoleColor _backgroundColor ;

		private bool _cursorVisible ;

		private ConsoleColor _foregroundColor ;

		private Size _internalSize = new Size ( 80 , 24 ) ;

		private string _title ;

		public Stream Stream { get ; set ; }

		public int RefreshLimit { get ; set ; } = 5 ;

		public bool IsRunning { get ; private set ; }

		public Thread WatcherThread { get ; private set ; }

		public Thread PullSizeThread { get ; private set ; }

		public StreamWriter Writer { get ; set ; }

		public Size InternalSize
		{
			get => _internalSize ;
			set
			{
				if ( value != _internalSize )
				{
					Size oldSize = _internalSize ;

					_internalSize = value ;

					SizeChanged ? . Invoke (
											this ,
											new ConsoleSizeChangedEvnetArgs { NewSize = value , OldSize = oldSize } ) ;
				}
			}
		}


		public bool CursorVisible
		{
			get => _cursorVisible ;
			set
			{
				if ( _cursorVisible != value )
				{
					lock ( Writer )
					{
						Writer . Write ( Csi ) ;
						Writer . Write ( "?25" ) ;
						if ( value )
						{
							Writer . Write ( "h" ) ;
						}
						else
						{
							Writer . Write ( "l" ) ;
						}

						Writer . Flush ( ) ;
					}

					_cursorVisible = value ;
				}
			}
		}

		public string Title
		{
			get => _title ;
			set
			{
				if ( _title != value )
				{
					lock ( Writer )
					{
						Writer . Write ( Osc ) ;
						Writer . Write ( $"2;{value}" ) ;
						Writer . Write ( St ) ;

						Writer . Flush ( ) ;
					}

					_title = value ;
				}
			}
		}

		public ConsoleColor BackgroundColor
		{
			get => _backgroundColor ;
			set
			{
				if ( value != _backgroundColor )
				{
					lock ( Writer )
					{
						//CSI Pm m  Character Attributes (SGR).
						Writer . Write ( Csi ) ;
						Writer . Write ( $"{value . BackgroundColorToCode ( )}m" ) ;
						Writer . Flush ( ) ;
					}

					_backgroundColor = value ;
				}
			}
		}

		public ConsoleColor ForegroundColor
		{
			get => _foregroundColor ;
			set
			{
				if ( value != _foregroundColor )
				{
					lock ( Writer )
					{
						//CSI Pm m  Character Attributes (SGR).
						Writer . Write ( Csi ) ;
						Writer . Write ( $"{value . ForegroundColorToCode ( )}m" ) ;
						Writer . Flush ( ) ;
					}

					_foregroundColor = value ;
				}
			}
		}


		public List <IInputEscapeSequenceParser> Parsers { get ; } =
			new List <IInputEscapeSequenceParser>
			{
				new ConsoleSizeEscapeSequenceParser ( ) , new KeyEscapeSequenceParser ( )
			} ;

		public XtermConsole ( [NotNull] Stream stream )
			=> Stream = stream ?? throw new ArgumentNullException ( nameof ( stream ) ) ;

		public void Bell ( )
		{
			lock ( Writer )
			{
				//CSI Ps = 8; height; width t
				Writer . Write ( Bel ) ;
				Writer . Flush ( ) ;
			}
		}

		public event EventHandler <ConsoleSizeChangedEvnetArgs> SizeChanged ;

		public Size Size
		{
			get => InternalSize ;
			set
			{
				if ( value != InternalSize )
				{
					lock ( Writer )
					{
						//CSI Ps = 8; height; width t
						Writer . Write ( Csi ) ;
						Writer . Write ( $"8;{value . Height};{value . Width}t" ) ;
						Writer . Flush ( ) ;
					}

					_internalSize = value ;
				}
			}
		}

		/// <summary>
		///     Is fired when a user presses an key
		/// </summary>
		public event EventHandler <KeyPressedEventArgs> KeyPressed ;

		public void Start ( )
		{
			if ( ! IsRunning )
			{
				Writer = new StreamWriter ( Stream , Encoding . UTF8 , 1024 , true ) ;

				if ( ! Application . IsDebug )
				{
					CursorVisible = false ;
				}

				if ( Application . Name != null )
				{
					Title = Application . Name ;
				}

				if ( Application . AutoDefaultColor )
				{
					Application . DefaultBackgroundColor = BackgroundColor ;
					Application . DefaultForegroundColor = ForegroundColor ;
				}

				WatcherThread  = new Thread ( Watch ) { Name    = nameof ( WatcherThread ) } ;
				PullSizeThread = new Thread ( PullSize ) { Name = nameof ( PullSizeThread ) } ;

				IsRunning = true ;

				PullSizeThread . Start ( ) ;
				WatcherThread . Start ( ) ;
			}
		}

		/// <summary>
		///     Stops the internal watcher-thread
		/// </summary>
		public void Stop ( )
		{
			IsRunning = false ;

			ResetColor ( ) ;
			SetCursorPosition ( 0 , 0 ) ;
			Clear ( ) ;
			CursorVisible = true ;
		}

		public void Draw ( Point position , ConsoleArea area )
		{
			Draw ( new Rectangle ( position , area . Size ) , area . Content ) ;
		}

		public Application Application { get ; set ; }

		public void InvokeKeyPressed ( ConsoleKeyInfo keyInfo )
			=> KeyPressed ? . Invoke ( this , new KeyPressedEventArgs ( keyInfo ) ) ;

		private void ResetColor ( )
		{
			lock ( Writer )
			{
				Writer . Write ( Csi ) ;
				Writer . Write ( "39m" ) ;
				Writer . Write ( Csi ) ;
				Writer . Write ( "49m" ) ;
				Writer . Flush ( ) ;
			}
		}

		private void Clear ( )
		{
			lock ( Writer )
			{
				//CSI Ps J Erase in Display(ED), VT100.
				//	Ps = 0->Erase Below(default).
				//	Ps = 1->Erase Above.
				//	Ps = 2->Erase All.
				//	Ps = 3->Erase Saved Lines(xterm).

				Writer . Write ( Csi ) ;
				Writer . Write ( "2J" ) ;
				Writer . Flush ( ) ;
			}
		}

		private void SetCursorPosition ( int x , int y )
		{
			lock ( Writer )
			{
				//CSI Ps; Ps H
				//Cursor Position[row; column] (default =  [1, 1])(CUP)  

				Writer . Write ( Csi ) ;
				Writer . Write ( $"{y + 1};{x + 1}H" ) ;
				Writer . Flush ( ) ;
			}
		}

		private void PullSize ( )
		{
			DateTime nextUpdate = DateTime . Now + TimeSpan . FromMilliseconds ( 1000d / RefreshLimit ) ;

			while ( IsRunning )
			{
				TimeSpan waitTime = nextUpdate - DateTime . Now ;
				nextUpdate = DateTime . Now + TimeSpan . FromMilliseconds ( 1000d / RefreshLimit ) ;

				lock ( Writer )
				{
					////CSI 1 8 t
					Writer . Write ( Csi ) ;
					Writer . Write ( "18t" ) ;
					Writer . Flush ( ) ;
				}

				Thread . Yield ( ) ;
				Thread . Sleep ( Math . Max ( Convert . ToInt32 ( waitTime . TotalMilliseconds ) , 0 ) ) ;
			}
		}

		private void Watch ( )
		{
			StreamReader reader = new StreamReader ( Stream , Encoding . UTF8 , false , 1024 , true ) ;

			Stream . ReadTimeout = 2000 / RefreshLimit ;

			List <char> buffer = new List <char> ( ) ;

			while ( IsRunning )
			{
				int currentChar ;

				try
				{
					currentChar = reader . Read ( ) ;
				}
				catch ( IOException )
				{
					continue ;
				}

				if ( currentChar == - 1 )
				{
					return ;
				}

				buffer . Add ( ( char ) currentChar ) ;

				while ( buffer . Any ( ) )
				{
					List <(IInputEscapeSequenceParser parser , ParseResult result)> parseResults =
						Parsers . Select ( parser => ( parser , parser . TryParse ( buffer ) ) ) . ToList ( ) ;

					List <(IInputEscapeSequenceParser parser , ParseResult result)> finishedParsers =
						parseResults . Where ( result => result . result == ParseResult . Finished ) . ToList ( ) ;

					if ( finishedParsers . Any ( ) )
					{
						finishedParsers . First ( ) . parser . Apply ( buffer , this ) ;
					}
					else
					{
						if ( parseResults . Any ( result => result . result == ParseResult . Established ) )
						{
							break ;
						}
						else
						{
							char c = buffer . First ( ) ;
							buffer . RemoveAt ( 0 ) ;
							if ( c . ToConsoleKey ( ) is ConsoleKeyInfo consoleKey )
							{
								InvokeKeyPressed ( consoleKey ) ;
							}
						}
					}
				}
			}
		}

		public void Draw ( Rectangle position , ConsoleChar [ , ] content )
		{
			try
			{
				Rectangle consoleArea = new Rectangle ( new Point ( ) , Size ) ;

				position = Rectangle . Intersect ( position , consoleArea ) ;

				StringBuilder stringBuilder = new StringBuilder ( content . Length ) ;

				for ( int y = 0 ; y < position . Height ; y++ )
				{
					SetCursorPosition ( position . Left , position . Top + y ) ;

					for ( int x = Math . Max ( - position . X , 0 ) ; x < position . Width ; x++ )
					{
						ConsoleChar currentPosition = content [ x , y ] ;

						ConsoleColor targetBackgroundColor = currentPosition . BackgroundColor ;
						ConsoleColor targetForegroundColor = currentPosition . ForegroundColor ;

						if ( BackgroundColor != targetBackgroundColor
							 || ForegroundColor != targetForegroundColor
							 && ! char . IsWhiteSpace ( currentPosition . Character ) )
						{
							Write ( stringBuilder ) ;

							BackgroundColor = targetBackgroundColor ;
							ForegroundColor = targetForegroundColor ;
						}

						stringBuilder . Append ( currentPosition . Character ) ;
					}

					Write ( stringBuilder ) ;
				}

				SetCursorPosition ( position . Left , position . Top ) ;
			}

			// ReSharper disable once EmptyGeneralCatchClause
			catch
			{
				//Todo: Warning
			}
		}

		private void Write ( StringBuilder stringBuilder )
		{
			if ( stringBuilder . Length > 0 )
			{
				lock ( Writer )
				{
					Writer . Write ( stringBuilder . ToString ( ) ) ;
					Writer . Flush ( ) ;
				}

				stringBuilder . Clear ( ) ;
			}
		}

	}

}
