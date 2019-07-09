using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using JetBrains . Annotations ;

namespace DreamRecorder . FoggyConsole
{

	public class KeyBindingManager
	{

		public static KeyBindingManager Current { get; private set; }

		public KeyBindingManager ( ) {
            Current = this;
		}

		private Dictionary <ConsoleKeyInfo , IHandleKeyInput> BoundHotkeys { get ; set ; } =
			new Dictionary <ConsoleKeyInfo , IHandleKeyInput> ( ) ;

		[CanBeNull]
		public IHandleKeyInput this[ConsoleKeyInfo keyInfo]
		{
			get {
				if (BoundHotkeys.ContainsKey(keyInfo))
				{
					return BoundHotkeys[keyInfo];
				}
				else
				{
					return default;
				}
			}
			set => BoundHotkeys [ keyInfo ] = value ;
		}

		public void HandleKey (KeyPressedEventArgs args )
		{
			if ( args == null )
			{
                return;

			}

			this [ args . KeyInfo ] ? . HandleKeyInput ( args ) ;
		}


	}

}