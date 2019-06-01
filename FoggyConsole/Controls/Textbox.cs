using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using WenceyWang . FoggyConsole . Controls . Renderers ;

namespace WenceyWang . FoggyConsole . Controls
{

	/// <summary>
	///     A control which provides single line editing and text input
	/// </summary>
	public class TextBox : TextualBase
	{

		private int _cursorPosition ;


		/// <summary>
		///     The position of the cursor within the textbox
		/// </summary>
		public int CursorPosition
		{
			get => _cursorPosition ;
			private set
			{
				_cursorPosition = value ;
				Draw ( ) ;
			}
		}

		public bool MultiLine { get ; set ; }

		public override bool CanFocus => Enabled ;

		/// <summary>
		///     Creates a new textbox
		/// </summary>
		/// <param name="renderer">
		///     The
		///     <code>ControlRenderer</code>
		///     to use. If null a new instance of
		///     <code>TextboxRenderer</code>
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
			IsFocusedChanged += OnFocusChanged ;
		}


		/// <summary>
		/// </summary>
		public event EventHandler EnterPressed ;

		/// <summary>
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="eventArgs"></param>
		protected void OnFocusChanged ( object sender , EventArgs eventArgs ) { Draw ( ) ; }

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
					if ( Text . Length != 0 )
					{
						if ( CursorPosition > 0 )
						{
							Text = Text . Remove ( CursorPosition - 1 ) ;
							CursorPosition-- ;
						}
					}

					break ;
				}

				default :
				{
					args . Handled = true ;
					char newChar = args . KeyInfo . KeyChar ;
					Text = Text . Insert ( CursorPosition , new string ( newChar , 1 ) ) ;
					CursorPosition++ ;
					break ;
				}
			}
		}

	}

}
