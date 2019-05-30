using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using WenceyWang . FoggyConsole . Controls . Renderers ;

namespace WenceyWang . FoggyConsole . Controls
{

	/// <summary>
	///     A control which displays a Text and has a State-State
	/// </summary>
	public class Checkbox : CheckableBase
	{

		/// <summary>
		///     Handles the key-userinput which is given in
		///     <paramref name="keyInfo" />
		///     .
		///     The State-State will flip if the user presses the spacebar.
		/// </summary>
		/// <returns>true if the keypress was handled, otherwise false</returns>
		/// <param name="keyInfo">The keypress to handle</param>
		//public override bool HandleKeyInput ( ConsoleKeyInfo keyInfo )
		//{
		//	if ( keyInfo . Key == ConsoleKey . Spacebar )
		//	{
		//		CheckState newState ;
		//		switch ( State )
		//		{
		//			case CheckState . Checked :
		//			{
		//				newState = CheckState . Unchecked ;
		//				break ;
		//			}
		//			case CheckState . Unchecked :
		//			case CheckState . Indeterminate :
		//			{
		//				newState = CheckState . Checked ;
		//				break ;
		//			}
		//			default :

		//				// stupid C#-compiler is stupid, will never happen.
		//				throw new ArgumentOutOfRangeException ( ) ;
		//		}

		//		State = newState ;
		//		return true ;
		//	}

		//	return false ;
		//}
		public override bool CanFocus => Enabled ;

		/// <summary>
		///     Creates a new Checkbox
		/// </summary>
		/// <param name="text">The text to display</param>
		/// <param name="renderer">
		///     The
		///     <code>ControlRenderer</code>
		///     to use. If null a new instance of
		///     <code>CheckboxRenderer</code>
		///     will be used.
		/// </param>
		/// <exception cref="ArgumentException">
		///     Thrown if the
		///     <code>ControlRenderer</code>
		///     which should be set already has an other
		///     Control assigned
		/// </exception>
		public Checkbox ( CheckboxRenderer renderer = null ) : base ( renderer ?? new CheckboxRenderer ( ) )
		{
			State = CheckState . Indeterminate ;
		}

	}

}
