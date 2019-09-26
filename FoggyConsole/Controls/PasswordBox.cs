using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Security ;

using DreamRecorder . FoggyConsole . Controls . Renderers ;

namespace DreamRecorder . FoggyConsole . Controls
{

	public class PasswordBox : Control
	{

		private int _cursorPosition ;

		private string _hintText ;

		private ConsoleColor _hintTextColor = ConsoleColor . Gray ;

		private char _passwordChar = '*' ;

		public ConsoleColor HintTextColor
		{
			get => _hintTextColor ;
			set
			{
				if ( _hintTextColor != value )
				{
					_hintTextColor = value ;
					RequestRedraw ( ) ;
				}
			}
		}

		public string HintText
		{
			get => _hintText ;
			set
			{
				if ( _hintText != value )
				{
					_hintText = value ;
					RequestRedraw ( ) ;
				}
			}
		}

		public SecureString Text { get ; set ; }


		/// <summary>
		///     The position of the cursor within the TextBox
		/// </summary>
		public int CursorPosition
		{
			get => _cursorPosition ;
			private set
			{
				_cursorPosition = value ;
				RequestRedraw ( ) ;
			}
		}

		/// <summary>
		///     The char as which all characters should be rendered if
		///     <code>PasswordMode</code>
		///     is true
		/// </summary>
		public char PasswordChar
		{
			get => _passwordChar ;
			set
			{
				if ( _passwordChar != value )
				{
					_passwordChar = value ;
					RequestRedraw ( ) ;
				}
			}
		}

		public override bool CanFocusedOn => Enabled ;

		public PasswordBox ( ControlRenderer <PasswordBox> renderer = null ) : base (
																					 renderer
																					 ?? new PasswordBoxRenderer ( ) )
		{
		}

		public PasswordBox ( ) : this ( null ) { }

		/// <summary>
		/// </summary>
		public event EventHandler EnterPressed ;

		public override void OnKeyPressed ( KeyPressedEventArgs args )
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
					args . Handled = true ;
					EnterPressed ? . Invoke ( this , EventArgs . Empty ) ;
					break ;
				}

				case ConsoleKey . RightArrow :
				{
					args . Handled = true ;
					if ( CursorPosition < Text . Length )
					{
						CursorPosition++ ;
					}

					break ;
				}

				case ConsoleKey . LeftArrow :
				{
					args . Handled = true ;
					if ( CursorPosition > 0 )
					{
						CursorPosition-- ;
					}

					break ;
				}

				case ConsoleKey . Backspace :
				{
					args . Handled = true ;
					if ( Text . Length     != 0
						 && CursorPosition > 0 )
					{
						Text . RemoveAt ( CursorPosition - 1 ) ;
						CursorPosition-- ;
					}

					break ;
				}

				case ConsoleKey . Delete :
				{
					args . Handled = true ;
					if ( Text . Length     != 0
						 && CursorPosition < Text . Length )
					{
						Text . RemoveAt ( CursorPosition + 1 ) ;
					}

					break ;
				}

				default :
				{
					args . Handled = true ;
					char newChar = args . KeyInfo . KeyChar ;
					Text . InsertAt ( CursorPosition , newChar ) ;
					CursorPosition++ ;
					break ;
				}
			}
		}

	}

}
