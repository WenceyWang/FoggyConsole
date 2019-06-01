using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace WenceyWang . FoggyConsole
{

	public static class Window
	{

		public static Size Size
		{
			get => new Size ( Console . WindowWidth , Console . WindowHeight ) ;
			set => Console . SetWindowSize ( value . Width , value . Height ) ;
		}

	}

}
