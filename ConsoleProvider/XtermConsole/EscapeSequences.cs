using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . FoggyConsole . XtermConsole
{

	public static class EscapeSequences
	{

		public const char Esc = '\u001B' ;

		public static readonly char [ ] Csi = {Esc, '[' } ;

		public static readonly char [ ] Bel = { '\u0007' } ;

		public static readonly char [ ] Ss2 = { Esc, 'N' } ;

		public static readonly char [ ] Ss3 = { Esc, 'O' } ;

		public static readonly char [ ] St = { Esc, '\\' } ;

		public static readonly char [ ] Osc = { Esc, ']' } ;


		public static byte [ ] CmdNewline = { 10 } ;

		public static byte [ ] CmdEsc = {(byte) Esc } ;

		public static byte [ ] CmdDel = { 0x7f } ;

		public static byte [ ] CmdDelKey = { (byte)Esc , ( byte ) '[' , ( byte ) '3' , ( byte ) '~' } ;

		public static byte [ ] MoveUpApp = { (byte)Esc , ( byte ) 'O' , ( byte ) 'A' } ;

		public static byte [ ] MoveUpNormal = { (byte)Esc , ( byte ) '[' , ( byte ) 'A' } ;

		public static byte [ ] MoveDownApp = { (byte)Esc , ( byte ) 'O' , ( byte ) 'B' } ;

		public static byte [ ] MoveDownNormal = { (byte)Esc , ( byte ) '[' , ( byte ) 'B' } ;

		public static byte [ ] MoveLeftApp = { (byte)Esc , ( byte ) 'O' , ( byte ) 'D' } ;

		public static byte [ ] MoveLeftNormal = { (byte)Esc , ( byte ) '[' , ( byte ) 'D' } ;

		public static byte [ ] MoveRightApp = { (byte)Esc , ( byte ) 'O' , ( byte ) 'C' } ;

		public static byte [ ] MoveRightNormal = { (byte)Esc , ( byte ) '[' , ( byte ) 'C' } ;

		public static byte [ ] MoveHomeApp = { (byte)Esc , ( byte ) 'O' , ( byte ) 'H' } ;

		public static byte [ ] MoveHomeNormal = { (byte)Esc , ( byte ) '[' , ( byte ) 'H' } ;

		public static byte [ ] MoveEndApp = { (byte)Esc , ( byte ) 'O' , ( byte ) 'F' } ;

		public static byte [ ] MoveEndNormal = { (byte)Esc , ( byte ) '[' , ( byte ) 'F' } ;

		public static byte [ ] CmdTab = { 9 } ;

		public static byte [ ] CmdBackTab = { (byte)Esc , ( byte ) '[' , ( byte ) 'Z' } ;

		public static byte [ ] CmdPageUp = { (byte)Esc , ( byte ) '[' , ( byte ) '5' , ( byte ) '~' } ;

		public static byte [ ] CmdPageDown = { (byte)Esc , ( byte ) '[' , ( byte ) '6' , ( byte ) '~' } ;

		public static byte [ ] [ ] CmdF =
		{
			new [ ] { ( byte ) Esc , ( byte ) 'O' , ( byte ) 'P' } ,                               /* F1 */
			new [ ] { ( byte ) Esc , ( byte ) 'O' , ( byte ) 'Q' } ,                               /* F2 */
			new [ ] { ( byte ) Esc , ( byte ) 'O' , ( byte ) 'R' } ,                               /* F3 */
			new [ ] { ( byte ) Esc , ( byte ) 'O' , ( byte ) 'S' } ,                               /* F4 */
			new [ ] { ( byte ) Esc , ( byte ) '[' , ( byte ) '1' , ( byte ) '5' , ( byte ) '~' } , /* F5 */
			new [ ] { ( byte ) Esc , ( byte ) '[' , ( byte ) '1' , ( byte ) '7' , ( byte ) '~' } , /* F6 */
			new [ ] { ( byte ) Esc , ( byte ) '[' , ( byte ) '1' , ( byte ) '8' , ( byte ) '~' } , /* F7 */
			new [ ] { ( byte ) Esc , ( byte ) '[' , ( byte ) '1' , ( byte ) '9' , ( byte ) '~' } , /* F8 */
			new [ ] { ( byte ) Esc , ( byte ) '[' , ( byte ) '2' , ( byte ) '0' , ( byte ) '~' } , /* F9 */
			new [ ] { ( byte ) Esc , ( byte ) '[' , ( byte ) '2' , ( byte ) '1' , ( byte ) '~' } , /* F10 */
			new [ ] { ( byte ) Esc , ( byte ) '[' , ( byte ) '2' , ( byte ) '3' , ( byte ) '~' } , /* F11 */
			new [ ] { ( byte ) Esc , ( byte ) '[' , ( byte ) '2' , ( byte ) '4' , ( byte ) '~' }   /* F12 */
		} ;

	}

}
