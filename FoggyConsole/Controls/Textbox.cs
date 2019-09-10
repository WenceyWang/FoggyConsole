﻿using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Collections . ObjectModel ;
using System . Linq ;

using DreamRecorder . FoggyConsole . Controls . Renderers ;

namespace DreamRecorder . FoggyConsole . Controls
{

	/// <summary>
	///     A control which provides single line editing and text input
	/// </summary>
	public class TextBox : TextualBase
	{

		private Point _cursorPosition ;

		/// <summary>
		///     The position of the cursor within the TextBox
		/// </summary>
		public Point CursorPosition
		{
			get => _cursorPosition ;
			set
			{
				if ( _cursorPosition != value )
				{
					_cursorPosition = ValidateCursor ( value ) ;
					RequestRedraw ( ) ;
				}
			}
		}

		public int CursorIndex
			=> Lines . Take ( CursorPosition . Y ) . Sum ( line => line . Length + Environment . NewLine . Length )
			   + CursorPosition . X ;

		public ReadOnlyCollection <string> Lines { get ; private set ; }

		public bool MultiLine { get ; set ; }

		public override bool CanFocusedOn => Enabled ;

		/// <summary>
		///     Creates a new TextBox
		/// </summary>
		/// <param name="renderer">
		///     The
		///     <code>ControlRenderer</code>
		///     to use. If null a new instance of
		///     <code>TextBoxRenderer</code>
		///     will be used.
		/// </param>
		/// <exception cref="ArgumentException">
		///     Thrown if the
		///     <code>ControlRenderer</code>
		///     which should be set already has an other
		///     Control assigned
		/// </exception>
		public TextBox ( TextBoxRenderer renderer = null ) : base ( renderer ?? new TextBoxRenderer ( ) )
		{
			UpdateLines ( ) ;
			TextChanged += TextBox_TextChanged ;
		}

		public TextBox ( ) : this ( null ) { }

		private Point ValidateCursor ( Point position )
		{
			ReadOnlyCollection <string> lines = Lines ;
			int                         y     = Math . Min ( Math . Max ( position . Y , 0 ) , lines . Count - 1 ) ;
			int                         x     = Math . Min ( Math . Max ( position . X , 0 ) , lines [ y ] . Length ) ;

			return new Point ( x , y ) ;
		}

		private void TextBox_TextChanged ( object sender , EventArgs e )
		{
			UpdateLines ( ) ;
			CursorPosition = ValidateCursor ( CursorPosition ) ;
		}

		private void UpdateLines ( )
		{
			Lines = new ReadOnlyCollection <string> (
													 Text . Split (
																   new [ ] { Environment . NewLine } ,
																   StringSplitOptions . None ) ) ;
		}

		/// <summary>
		/// </summary>
		public event EventHandler <KeyPressedEventArgs> EnterPressed ;

		public override void KeyPressed ( KeyPressedEventArgs args )
		{
			if ( ! Enabled )
			{
				return ;
			}

			switch ( args . KeyInfo . Key )
			{
				case ConsoleKey . Tab :
				case ConsoleKey . Escape :
				{
					break ;
				}

				case ConsoleKey . Enter :
				{
					EnterPressed ? . Invoke ( this , args ) ;
					if ( ! args . Handled && MultiLine )
					{
						args . Handled = true ;
						Text           = Text . Insert ( CursorIndex , Environment . NewLine ) ;
						CursorPosition = new Point ( 0 , CursorPosition . Y + 1 ) ;
					}

					break ;
				}

				case ConsoleKey . RightArrow :
				{
					args . Handled = true ;

					CursorPosition = new Point ( CursorPosition . X + 1 , CursorPosition . Y ) ;

					break ;
				}

				case ConsoleKey . LeftArrow :
				{
					args . Handled = true ;

					CursorPosition = new Point ( CursorPosition . X - 1 , CursorPosition . Y ) ;

					break ;
				}

				case ConsoleKey . Backspace :
				{
					args . Handled = true ;
					if ( CursorPosition . X > 0 )
					{
						CursorPosition = new Point ( CursorPosition . X - 1 , CursorPosition . Y ) ;
						Text           = Text . Remove ( CursorIndex , 1 ) ;
					}
					else
					{
						if ( MultiLine )
						{
							if ( CursorPosition . Y > 0 )
							{
								int prevCursorIndex = CursorIndex ;
								CursorPosition = new Point (
															Lines [ CursorPosition . Y - 1 ] . Length ,
															CursorPosition . Y - 1 ) ;
								Text = Text . Remove (
													  prevCursorIndex - Environment . NewLine . Length ,
													  Environment . NewLine . Length ) ;
							}
						}
					}

					break ;
				}

				case ConsoleKey . UpArrow :
				{
					if ( MultiLine )
					{
						args . Handled = true ;
						if ( CursorPosition . Y > 0 )
						{
							CursorPosition = new Point (
														Math . Min (
																	Lines [ CursorPosition . Y - 1 ] . Length ,
																	CursorPosition . X ) ,
														CursorPosition . Y - 1 ) ;
						}
					}

					break ;
				}

				case ConsoleKey . DownArrow :
				{
					if ( MultiLine )
					{
						args . Handled = true ;
						if ( CursorPosition . Y < Lines . Count - 1 )
						{
							CursorPosition = new Point (
														Math . Min (
																	Lines [ CursorPosition . Y + 1 ] . Length ,
																	CursorPosition . X ) ,
														CursorPosition . Y + 1 ) ;
						}
					}

					break ;
				}

				default :
				{
					args . Handled = true ;
					char newChar = args . KeyInfo . KeyChar ;
					if ( ! char . IsControl ( newChar ) )
					{
						Text           = Text . Insert ( CursorIndex , new string ( newChar , 1 ) ) ;
						CursorPosition = new Point ( CursorPosition . X + 1 , CursorPosition . Y ) ;
					}

					break ;
				}
			}
		}

	}

}
