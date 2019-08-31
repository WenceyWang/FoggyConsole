using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Text ;
using System . Threading ;

namespace DreamRecorder . FoggyConsole . LocalConsole
{

	public class LocalConsole : IConsole
	{

		public static LocalConsole Current { get ; } = new LocalConsole ( ) ;

		public Size LastRenderedSize { get ; set ; }

		public Size PreviousSize { get ; set ; }

		public int RefreshLimit { get ; set ; } = 60 ;

		public bool IsRunning { get ; private set ; }

		public Thread WatcherThread { get ; private set ; }

		private ConsoleColor CurrentForegroundColor { get ; set ; }

		private ConsoleColor CurrentBackgroundColor { get ; set ; }

		public void Bell ( ) { Console . Beep ( ) ; }

		public event EventHandler <ConsoleSizeChangedEvnetArgs> SizeChanged ;

		public Size Size
		{
			get => new Size ( Console . WindowWidth , Console . WindowHeight ) ;
			set => Console . SetWindowSize ( value . Width , value . Height ) ;
		}

		/// <summary>
		///     Is fired when a user presses an key
		/// </summary>
		public event EventHandler <KeyPressedEventArgs> KeyPressed ;

		public void Start ( )
		{
			if ( ! IsRunning )
			{
				if ( ! Application . IsDebug )
				{
					Console . CursorVisible = false ;
				}

				Console . OutputEncoding = Encoding . UTF8 ;
				Console . InputEncoding  = Encoding . UTF8 ;

				if ( Application . Name != null )
				{
					Console . Title = Application . Name ;
				}

				if ( Application . AutoDefaultColor )
				{
					Application . DefaultBackgroundColor = Console . BackgroundColor ;
					Application . DefaultForegroundColor = Console . ForegroundColor ;
				}

				WatcherThread = new Thread ( Watch ) { Name = nameof ( WatcherThread ) } ;
				IsRunning     = true ;
				WatcherThread . Start ( ) ;
			}
		}

		/// <summary>
		///     Stops the internal watcher-thread
		/// </summary>
		public void Stop ( )
		{
			IsRunning                =  false ;
			Console . CancelKeyPress -= Console_CancelKeyPress ;

			Console . ResetColor ( ) ;
			Console . SetCursorPosition ( 0 , 0 ) ;
			Console . Clear ( ) ;
			Console . CursorVisible = true ;
		}

		public void Draw ( Point position , ConsoleArea area )
		{
			Draw ( new Rectangle ( position , area . Size ) , area . Content ) ;
		}

		public Application Application { get ; set ; }

		private void Console_CancelKeyPress ( object sender , ConsoleCancelEventArgs e ) { e . Cancel = true ; }

		private void Watch ( )
		{
			bool isPaused = false ;

			PreviousSize = Size ;

			DateTime lastUpdate = DateTime . Now + TimeSpan . FromMilliseconds ( 1000d / RefreshLimit ) ;

			while ( IsRunning )
			{
				Size newSize = Size ;

				if ( newSize == PreviousSize )
				{
					if ( isPaused )
					{
						SizeChanged ? . Invoke (
												this ,
												new ConsoleSizeChangedEvnetArgs
												{
													NewSize = newSize , OldSize = LastRenderedSize
												} ) ;

						Application . ViewRoot . ResumeRedraw ( ) ;

						isPaused = false ;
					}
				}
				else
				{
					if ( ! isPaused )
					{
						LastRenderedSize = PreviousSize ;

						Application . ViewRoot . PauseRedraw ( ) ;

						isPaused = true ;
					}

					PreviousSize = newSize ;
				}

				if ( Console . KeyAvailable )
				{
					ConsoleKeyInfo keyInfo = Console . ReadKey ( true ) ;
					KeyPressed ? . Invoke ( null , new KeyPressedEventArgs ( keyInfo ) ) ;
				}

				TimeSpan waitTime = lastUpdate - DateTime . Now ;
				lastUpdate = DateTime . Now + TimeSpan . FromMilliseconds ( 1000d / RefreshLimit) ;

				Thread . Yield ( ) ;
				Thread . Sleep ( Math . Max ( Convert . ToInt32 ( waitTime . TotalMilliseconds ) , 0 ) ) ;
			}
		}

		public void Draw ( Rectangle position , ConsoleChar [ , ] content )
		{
			try
			{
				CurrentBackgroundColor = Console . BackgroundColor ;
				CurrentForegroundColor = Console . ForegroundColor ;

				Rectangle consoleArea = new Rectangle ( new Point ( ) , Size ) ;

				position = Rectangle . Intersect ( position , consoleArea ) ;

				bool changeLine = position . Right != consoleArea . Right || position . Left != consoleArea . Left ;

				StringBuilder stringBuilder = new StringBuilder ( content . Length ) ;

				if ( ! changeLine )
				{
					Console . SetCursorPosition ( position . Left , position . Top ) ;
				}

				for ( int y = 0 ; y < position . Height ; y++ )
				{
					if ( changeLine )
					{
						Console . SetCursorPosition ( position . Left , position . Top + y ) ;
					}

					for ( int x = Math . Max ( - position . X , 0 ) ; x < position . Width ; x++ )
					{
						ConsoleChar currentPosition = content [ x , y ] ;

						ConsoleColor targetBackgroundColor = currentPosition . BackgroundColor ;
						ConsoleColor targetForegroundColor = currentPosition . ForegroundColor ;

						if ( CurrentBackgroundColor != targetBackgroundColor
							|| CurrentForegroundColor != targetForegroundColor
							&& ! char . IsWhiteSpace ( currentPosition . Character ) )
						{
							Write ( stringBuilder ) ;

							Console . BackgroundColor = CurrentBackgroundColor = targetBackgroundColor ;
							Console . ForegroundColor = CurrentForegroundColor = targetForegroundColor ;
						}

						stringBuilder . Append ( currentPosition . Character ) ;
					}

					if ( changeLine )
					{
						Write ( stringBuilder ) ;
					}
				}

				if ( ! changeLine )
				{
					Write ( stringBuilder ) ;
				}

				Console . SetCursorPosition ( position . Left , position . Top ) ;
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
				Console . Write ( stringBuilder . ToString ( ) ) ;
				stringBuilder . Clear ( ) ;
			}
		}

	}

}
