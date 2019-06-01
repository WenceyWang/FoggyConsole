using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace WenceyWang . FoggyConsole
{

	/// <summary>
	///     Stores information about a pressed key
	/// </summary>
	public class KeyPressedEventArgs : EventArgs
	{

		public bool Handled { get ; set ; }

		/// <summary>
		///     The key which was pressed
		/// </summary>
		public ConsoleKeyInfo KeyInfo { get ; }

		/// <inheritdoc />
		/// <summary>
		///     Creates a new
		///     <code>KeyPressedEventArgs</code>
		///     instance
		/// </summary>
		/// <param name="keyInfo">The key which was pressed</param>
		public KeyPressedEventArgs ( ConsoleKeyInfo keyInfo ) { KeyInfo = keyInfo ; }

	}

}
