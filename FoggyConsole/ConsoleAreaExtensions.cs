using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using JetBrains . Annotations ;

namespace DreamRecorder . FoggyConsole
{

	public static class ConsoleCharExtensions
	{

		public static ConsoleChar InvertColor ( this ConsoleChar character )
			=> new ConsoleChar (
								character . Character ,
								character . ForegroundColor . Invert ( ) ,
								character . BackgroundColor . Invert ( ) ) ;

	}

	public static class ConsoleColorExtensions
	{

		public static ConsoleColor Invert ( this ConsoleColor color )
		{
			switch ( color )
			{
				case ConsoleColor . Black :
					return ConsoleColor . White ;
				case ConsoleColor . Blue :
					return ConsoleColor . DarkYellow ;
				case ConsoleColor . Cyan :
					return ConsoleColor . DarkRed ;
				case ConsoleColor . DarkBlue :
					return ConsoleColor . Yellow ;
				case ConsoleColor . DarkCyan :
					return ConsoleColor . Red ;
				case ConsoleColor . DarkGray :
					return ConsoleColor . Gray ;
				case ConsoleColor . DarkGreen :
					return ConsoleColor . Magenta ;
				case ConsoleColor . DarkMagenta :
					return ConsoleColor . Green ;
				case ConsoleColor . DarkRed :
					return ConsoleColor . Cyan ;
				case ConsoleColor . DarkYellow :
					return ConsoleColor . Blue ;
				case ConsoleColor . Gray :
					return ConsoleColor . DarkGray ;
				case ConsoleColor . Green :
					return ConsoleColor . DarkMagenta ;
				case ConsoleColor . Magenta :
					return ConsoleColor . DarkGreen ;
				case ConsoleColor . Red :
					return ConsoleColor . DarkCyan ;
				case ConsoleColor . White :
					return ConsoleColor . Black ;
				case ConsoleColor . Yellow :
					return ConsoleColor . DarkBlue ;
				default :
					throw new ArgumentOutOfRangeException ( nameof ( color ) , color , null ) ;
			}
		}

	}

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
																		boarderStyle .
																			SingleLineRightEdge ,
																		foregroundColor ,
																		backgroundColor ) ;
			}
			else
			{
				area [ 0 , 0 ] = new ConsoleChar (
												boarderStyle . TopLeftCorner ,
												foregroundColor ,
												backgroundColor ) ;
				area [ area . Size . Width - 1 , 0 ] = new ConsoleChar (
																		boarderStyle .
																			TopRightCorner ,
																		foregroundColor ,
																		backgroundColor ) ;
				area [ 0 , area . Size . Height - 1 ] = new ConsoleChar (
																		boarderStyle .
																			BottomLeftCorner ,
																		foregroundColor ,
																		backgroundColor ) ;
				area [ area . Size . Width - 1 , area . Size . Height - 1 ] =
					new ConsoleChar (
									boarderStyle . BottomRightCorner ,
									foregroundColor ,
									backgroundColor ) ;

				for ( int x = 1 ; x < area . Size . Width - 1 ; x++ )
				{
					area [ x , 0 ] = new ConsoleChar (
													boarderStyle . HorizontalEdge ,
													foregroundColor ,
													backgroundColor ) ;
					area [ x , area . Size . Height - 1 ] = new ConsoleChar (
																			boarderStyle .
																				HorizontalEdge ,
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
																			boarderStyle .
																				VerticalEdge ,
																			foregroundColor ,
																			backgroundColor ) ;
				}
			}
		}

	}

}
