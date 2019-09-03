using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using JetBrains . Annotations ;

namespace DreamRecorder . FoggyConsole
{

	public static class ConsoleAreaExtensions
	{

		public static void InvertColor ( this ConsoleArea area )
		{
			for ( int y = 0 ; y < area . Size . Height ; y++ )
			{
				for ( int x = 0 ; x < area . Size . Width ; x++ )
				{
					area [ x , y ] = area [ x , y ] . InvertColor ( ) ;
				}
			}
		}

		public static void DrawBoarder (
			[NotNull] this ConsoleArea area ,
			LineStyle                  boarderStyle ,
			ConsoleColor               foregroundColor ,
			ConsoleColor               backgroundColor )
		{
			if ( area == null )
			{
				throw new ArgumentNullException ( nameof ( area ) ) ;
			}

			if ( area . Size . Height == 1 )
			{
				area [ 0 , 0 ] = new ConsoleChar (
												  boarderStyle . SingleLineLeftEdge ,
												  foregroundColor ,
												  backgroundColor ) ;

				area [ area . Size . Width - 1 , 0 ] = new ConsoleChar (
																		boarderStyle . SingleLineRightEdge ,
																		foregroundColor ,
																		backgroundColor ) ;
			}
			else
			{
				area [ 0 , 0 ] = new ConsoleChar ( boarderStyle . TopLeftCorner , foregroundColor , backgroundColor ) ;
				area [ area . Size . Width - 1 , 0 ] = new ConsoleChar (
																		boarderStyle . TopRightCorner ,
																		foregroundColor ,
																		backgroundColor ) ;
				area [ 0 , area . Size . Height - 1 ] = new ConsoleChar (
																		 boarderStyle . BottomLeftCorner ,
																		 foregroundColor ,
																		 backgroundColor ) ;
				area [ area . Size . Width - 1 , area . Size . Height - 1 ] =
					new ConsoleChar ( boarderStyle . BottomRightCorner , foregroundColor , backgroundColor ) ;

				for ( int x = 1 ; x < area . Size . Width - 1 ; x++ )
				{
					area [ x , 0 ] = new ConsoleChar (
													  boarderStyle . HorizontalEdge ,
													  foregroundColor ,
													  backgroundColor ) ;
					area [ x , area . Size . Height - 1 ] = new ConsoleChar (
																			 boarderStyle . HorizontalEdge ,
																			 foregroundColor ,
																			 backgroundColor ) ;
				}

				for ( int y = 1 ; y < area . Size . Height - 1 ; y++ )
				{
					area [ 0 , y ] = new ConsoleChar (
													  boarderStyle . VerticalEdge ,
													  foregroundColor ,
													  backgroundColor ) ;
					area [ area . Size . Width - 1 , y ] = new ConsoleChar (
																			boarderStyle . VerticalEdge ,
																			foregroundColor ,
																			backgroundColor ) ;
				}
			}
		}

	}

}
