using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace WenceyWang . FoggyConsole
{

	/// <summary>
	///     A set of characters which can be used to draw a box or complex lines
	/// </summary>
	public struct LineStyle : IEquatable <LineStyle>
	{

		/// <summary>
		///     A character representing the top left corner of an rectangle
		/// </summary>
		public char TopLeftCorner { get ; set ; }

		/// <summary>
		///     A character representing the top right corner of an rectangle
		/// </summary>
		public char TopRightCorner { get ; set ; }

		/// <summary>
		///     A character representing the bottom left corner of an rectangle
		/// </summary>
		public char BottomLeftCorner { get ; set ; }

		public bool Equals ( LineStyle other )
		{
			return TopLeftCorner == other . TopLeftCorner
					&& TopRightCorner == other . TopRightCorner
					&& BottomLeftCorner == other . BottomLeftCorner
					&& BottomRightCorner == other . BottomRightCorner
					&& VerticalEdge == other . VerticalEdge
					&& HorizontalEdge == other . HorizontalEdge
					&& ConnectionHorizontalUp == other . ConnectionHorizontalUp
					&& ConnectionHorizontalDown == other . ConnectionHorizontalDown
					&& ConnectionVerticalRight == other . ConnectionVerticalRight
					&& ConnectionVerticalLeft == other . ConnectionVerticalLeft
					&& ConnectionCross == other . ConnectionCross
					&& EmptyChar == other . EmptyChar ;
		}

		public override bool Equals ( object obj )
		{
			if ( ReferenceEquals ( null , obj ) )
			{
				return false ;
			}

			return obj is LineStyle && Equals ( ( LineStyle ) obj ) ;
		}

		public override int GetHashCode ( )
		{
			unchecked
			{
				int hashCode = TopLeftCorner . GetHashCode ( ) ;
				hashCode = ( hashCode * 397 ) ^ TopRightCorner . GetHashCode ( ) ;
				hashCode = ( hashCode * 397 ) ^ BottomLeftCorner . GetHashCode ( ) ;
				hashCode = ( hashCode * 397 ) ^ BottomRightCorner . GetHashCode ( ) ;
				hashCode = ( hashCode * 397 ) ^ VerticalEdge . GetHashCode ( ) ;
				hashCode = ( hashCode * 397 ) ^ HorizontalEdge . GetHashCode ( ) ;
				hashCode = ( hashCode * 397 ) ^ ConnectionHorizontalUp . GetHashCode ( ) ;
				hashCode = ( hashCode * 397 ) ^ ConnectionHorizontalDown . GetHashCode ( ) ;
				hashCode = ( hashCode * 397 ) ^ ConnectionVerticalRight . GetHashCode ( ) ;
				hashCode = ( hashCode * 397 ) ^ ConnectionVerticalLeft . GetHashCode ( ) ;
				hashCode = ( hashCode * 397 ) ^ ConnectionCross . GetHashCode ( ) ;
				hashCode = ( hashCode * 397 ) ^ EmptyChar . GetHashCode ( ) ;
				return hashCode ;
			}
		}

		public static bool operator == ( LineStyle left , LineStyle right ) { return left . Equals ( right ) ; }

		public static bool operator != ( LineStyle left , LineStyle right ) { return ! left . Equals ( right ) ; }

		/// <summary>
		///     A character representing the bottom right corner of an rectangle
		/// </summary>
		public char BottomRightCorner { get ; set ; }

		/// <summary>
		///     A character representing the a left or right edge of an rectangle
		/// </summary>
		public char VerticalEdge { get ; set ; }

		/// <summary>
		///     A character representing the a top or bottom edge of an rectangle
		/// </summary>
		public char HorizontalEdge { get ; set ; }

		/// <summary>
		///     A character representing a connection between a
		///     <code>HorizontalEdge</code>
		///     and a
		///     <code>VerticalEdge</code>
		///     ,
		///     in which the
		///     <code>VerticalEdge</code>
		///     is above the
		///     <code>HorizontalEdge</code>
		/// </summary>
		public char ConnectionHorizontalUp { get ; set ; }

		/// <summary>
		///     A character representing a connection between a
		///     <code>HorizontalEdge</code>
		///     and a
		///     <code>VerticalEdge</code>
		///     ,
		///     in which the
		///     <code>VerticalEdge</code>
		///     is below the
		///     <code>HorizontalEdge</code>
		/// </summary>
		public char ConnectionHorizontalDown { get ; set ; }

		/// <summary>
		///     A character representing a connection between a
		///     <code>VerticalEdge</code>
		///     and a
		///     <code>HorizontalEdge</code>
		///     ,
		///     in which the
		///     <code>HorizontalEdge</code>
		///     is on the left side of the
		///     <code>VerticalEdge</code>
		/// </summary>
		public char ConnectionVerticalRight { get ; set ; }

		/// <summary>
		///     A character representing a connection between a
		///     <code>VerticalEdge</code>
		///     and a
		///     <code>HorizontalEdge</code>
		///     ,
		///     in which the
		///     <code>HorizontalEdge</code>
		///     is on the right side of the
		///     <code>VerticalEdge</code>
		/// </summary>
		public char ConnectionVerticalLeft { get ; set ; }

		/// <summary>
		///     A characer representing a connection between two
		///     <code>VerticalEdge</code>
		///     and two
		///     <code>HorizontalEdge</code>
		/// </summary>
		public char ConnectionCross { get ; set ; }

		/// <summary>
		///     A empty character which can be used to fill boxes
		/// </summary>
		public char EmptyChar { get ; set ; }

		/// <summary>
		///     A
		///     <code>LineStyle</code>
		///     which uses single-lined box-drawing-characters
		/// </summary>
		/// <returns></returns>
		public static LineStyle Empty => new LineStyle
										{
											TopLeftCorner = ' ' ,
											TopRightCorner = ' ' ,
											BottomLeftCorner = ' ' ,
											BottomRightCorner = ' ' ,
											VerticalEdge = ' ' ,
											HorizontalEdge = ' ' ,
											ConnectionHorizontalUp = ' ' ,
											ConnectionHorizontalDown = ' ' ,
											ConnectionVerticalRight = ' ' ,
											ConnectionVerticalLeft = ' ' ,
											ConnectionCross = ' ' ,
											EmptyChar = ' '
										} ;


		/// <summary>
		///     A very simple
		///     <code>LineStyle</code>
		///     .
		/// </summary>
		public static LineStyle GetSimpleSet { get ; } = new LineStyle
														{
															TopLeftCorner = '.' ,
															TopRightCorner = '.' ,
															BottomLeftCorner = '`' ,
															BottomRightCorner = '´' ,
															VerticalEdge = '|' ,
															HorizontalEdge = '-' ,
															ConnectionHorizontalUp = '+' ,
															ConnectionHorizontalDown = '+' ,
															ConnectionVerticalRight = '+' ,
															ConnectionVerticalLeft = '+' ,
															ConnectionCross = '+' ,
															EmptyChar = ' '
														} ;

		/// <summary>
		///     A
		///     <code>LineStyle</code>
		///     which uses single-lined box-drawing-characters
		/// </summary>
		/// <returns></returns>
		public static LineStyle SingleLinesSet => new LineStyle
												{
													TopLeftCorner = '\u250C' , // ┌
													TopRightCorner = '\u2510' , // ┐
													BottomLeftCorner = '\u2514' , // └
													BottomRightCorner = '\u2518' , // ┘
													VerticalEdge = '\u2502' , // │
													HorizontalEdge = '\u2500' , // ─
													ConnectionHorizontalUp = '\u2534' , // ┴
													ConnectionHorizontalDown = '\u252C' , // ┬
													ConnectionVerticalRight = '\u251C' , // ├
													ConnectionVerticalLeft = '\u2524' , // ┤
													ConnectionCross = '\u253C' , // ┼
													EmptyChar = ' '
												} ;

		/// <summary>
		///     A
		///     <code>LineStyle</code>
		///     which uses double-lined box-drawing-characters
		/// </summary>
		public static LineStyle DoubleLinesSet => new LineStyle
												{
													TopLeftCorner = '\u2554' , // ╔
													TopRightCorner = '\u2557' , // ╗
													BottomLeftCorner = '\u255A' , // ╚
													BottomRightCorner = '\u255D' , // ╝
													VerticalEdge = '\u2551' , // ║
													HorizontalEdge = '\u2550' , // ═
													ConnectionHorizontalUp = '\u2569' , // ╩
													ConnectionHorizontalDown = '\u2566' , // ╦
													ConnectionVerticalRight = '\u2560' , // ╠
													ConnectionVerticalLeft = '\u2563' , // ╣
													ConnectionCross = '\u256C' , // ╬
													EmptyChar = ' '
												} ;

	}

}
