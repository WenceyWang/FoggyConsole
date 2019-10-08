using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Security ;

using DreamRecorder . FoggyConsole . Controls . Renderers ;

namespace DreamRecorder . FoggyConsole . Controls
{

	public class PasswordBox : TextBox
	{

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

	}

}
