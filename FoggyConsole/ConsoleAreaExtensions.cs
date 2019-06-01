using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace WenceyWang . FoggyConsole
{

	public static class ConsoleAreaExtensions
	{

		public static ConsoleArea AddBoard ( this ConsoleArea source , LineStyle lineStyle )
		{
			ConsoleArea result =
				new ConsoleArea ( new Size ( source . Size . Width + 2 , source . Size . Height + 2 ) ) ;

			for ( int y = 0 ; y < source . Size . Height ; y++ )
			{
				for ( int x = 0 ; x < source . Size . Width ; x++ )
				{
					result . Content [ x + 1 , y + 1 ] = source . Content [ x , y ] ;
				}
			}

			result . Content [ 0 , 0 ]                              = lineStyle . TopLeftCorner ;
			result . Content [ result . Size . Width      - 1 , 0 ] = lineStyle . TopRightCorner ;
			result . Content [ 0 , result . Size . Height - 1 ]     = lineStyle . BottomLeftCorner ;
			result . Content [ result . Size . Width - 1 , result . Size . Height - 1 ] =
				lineStyle . BottomRightCorner ;

			for ( int x = 1 ; x < result . Size . Width - 1 ; x++ )
			{
				result . Content [ x , 0 ]                          = lineStyle . HorizontalEdge ;
				result . Content [ x , result . Size . Height - 1 ] = lineStyle . HorizontalEdge ;
			}

			for ( int y = 1 ; y < result . Size . Width - 1 ; y++ )
			{
				result . Content [ 0 , y ]                         = lineStyle . VerticalEdge ;
				result . Content [ result . Size . Width - 1 , y ] = lineStyle . VerticalEdge ;
			}

			return result ;
		}

	}

}
