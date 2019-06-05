using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Numerics ;

namespace DreamRecorder . FoggyConsole
{

	/// <summary>
	///     A very basic representation of a rectangle
	/// </summary>
	public struct Rectangle : IEquatable <Rectangle>
	{

		public int X { get ; }

		public int Y { get ; }

		public Point LeftTopPoint => new Point ( X , Y ) ;

		public Point RightDownPoint => new Point ( X + Width , Y + Height ) ;

		public Point Center => new Point ( X + ( Width / 2 ) , Y + ( Height / 2 ) ) ;

		public Vector2 FloatCenter => new Vector2 ( X + ( Width / 2f ) , Y + ( Height / 2f ) ) ;

		/// <summary>
		///     This rectangles distance from the left edge in characters
		/// </summary>
		public int Left => X ;

		/// <summary>
		/// </summary>
		public int Right => X + Width ;

		/// <summary>
		///     This rectangles distance from the top edge in characters
		/// </summary>
		public int Top => Y ;

		public int Bottom => Y + Height ;

		/// <summary>
		///     This rectangles height in characters
		/// </summary>
		public int Height { get ; }

		public bool IsEmpty => Width <= 0 || Height <= 0 ;

		public bool IntersectsWith ( Rectangle rect )
		{
			if ( IsEmpty || rect . IsEmpty )
			{
				return false ;
			}

			return rect . Left <= Right && rect . Right >= Left && rect . Top <= Bottom && rect . Bottom >= Top ;
		}

		public Rectangle Intersect ( Rectangle rect )
		{
			if ( ! IntersectsWith ( rect ) )
			{
				return Empty ;
			}

			int left   = Math . Max ( Left , rect . Left ) ;
			int top    = Math . Max ( Top ,  rect . Top ) ;
			int width  = Math . Min ( Right ,  rect . Right )  - left ;
			int height = Math . Min ( Bottom , rect . Bottom ) - top ;

			return new Rectangle ( left , top , width , height ) ;
		}


		public Rectangle Union ( Rectangle rect )
		{
			if ( IsEmpty )
			{
				return rect ;
			}

			int left   = Math . Min ( Left , rect . Left ) ;
			int top    = Math . Min ( Top ,  rect . Top ) ;
			int width  = Math . Max ( Math . Max ( Right ,  rect . Right )  - left , 0 ) ;
			int height = Math . Max ( Math . Max ( Bottom , rect . Bottom ) - top ,  0 ) ;

			return new Rectangle ( left , top , width , height ) ;
		}

		/// <summary>
		///     Offset - translate the Location by the offset provided.
		/// </summary>
		public Rectangle Offset ( Vector offsetVector )
		{
			if ( IsEmpty )
			{
				return Empty ;
			}

			return new Rectangle ( X + offsetVector . X , Y + offsetVector . Y , Width , Height ) ;
		}

		/// <summary>
		///     Offset - translate the Location by the offset provided
		/// </summary>
		public Rectangle Offset ( int offsetX , int offsetY )
		{
			if ( IsEmpty )
			{
				return Empty ;
			}

			return new Rectangle ( X + offsetX , Y + offsetY , Width , Height ) ;
		}


		public bool Equals ( Rectangle other )
			=> X == other . X && Y == other . Y && Height == other . Height && Width == other . Width ;

		public override bool Equals ( object obj )
		{
			if ( obj is null )
			{
				return false ;
			}

			return obj is Rectangle rectangle && Equals ( rectangle ) ;
		}

		public override int GetHashCode ( )
		{
			unchecked
			{
				int hashCode = X ;
				hashCode = ( hashCode * 397 ) ^ Y ;
				hashCode = ( hashCode * 397 ) ^ Height ;
				hashCode = ( hashCode * 397 ) ^ Width ;
				return hashCode ;
			}
		}

		public static bool operator == ( Rectangle left , Rectangle right ) => left . Equals ( right ) ;

		public static bool operator != ( Rectangle left , Rectangle right ) => ! left . Equals ( right ) ;

		/// <summary>
		///     Creates a new rectangle
		/// </summary>
		/// <param name="left">The left value to set</param>
		/// <param name="top">The top value to set</param>
		/// <param name="width">The width value to set</param>
		/// <param name="height">The height value to set</param>
		public Rectangle ( int left , int top , int width , int height )
		{
			#region Check Argument

			if ( left < 0 )
			{
				throw new ArgumentOutOfRangeException ( nameof ( left ) ) ;
			}

			if ( top < 0 )
			{
				throw new ArgumentOutOfRangeException ( nameof ( top ) ) ;
			}

			if ( width < 0 )
			{
				throw new ArgumentOutOfRangeException ( nameof ( width ) ) ;
			}

			if ( height < 0 )
			{
				throw new ArgumentOutOfRangeException ( nameof ( height ) ) ;
			}

			#endregion

			X      = left ;
			Y      = top ;
			Height = height ;
			Width  = width ;
		}

		/// <summary>
		///     Constructor which sets the initial values to the values of the parameters
		/// </summary>
		public Rectangle ( Point location , Size size )
		{
			X      = location . X ;
			Y      = location . Y ;
			Width  = size . Width ;
			Height = size . Height ;
		}


		/// <summary>
		///     Constructor which sets the initial values to bound the two points provided.
		/// </summary>
		public Rectangle ( Point point1 , Point point2 )
		{
			X = Math . Min ( point1 . X , point2 . X ) ;
			Y = Math . Min ( point1 . Y , point2 . Y ) ;

			//  Max with 0 to prevent double weirdness from causing us to be (-epsilon..0)
			Width  = Math . Max ( Math . Max ( point1 . X , point2 . X ) - X , 0 ) ;
			Height = Math . Max ( Math . Max ( point1 . Y , point2 . Y ) - Y , 0 ) ;
		}

		/// <summary>
		///     Constructor which sets the initial values to bound the point provided and the point
		///     which results from point + vector.
		/// </summary>
		public Rectangle ( Point point , Vector vector ) : this ( point , point + vector ) { }

		/// <summary>
		///     Constructor which sets the initial values to bound the (0,0) point and the point
		///     that results from (0,0) + size.
		/// </summary>
		public Rectangle ( Size size )
		{
			X      = Y = 0 ;
			Width  = size . Width ;
			Height = size . Height ;
		}

		/// <summary>
		///     This rectangles width in characters
		/// </summary>
		public int Width { get ; }

		/// <summary>
		///     Empty - a static property which provides an EmptyChar rectangle.
		/// </summary>
		public static Rectangle Empty => new Rectangle ( 0 , 0 , 0 , 0 ) ;


		public static Rectangle Intersect ( Rectangle rect1 , Rectangle rect2 ) => rect1 . Intersect ( rect2 ) ;

		public static Rectangle Union ( Rectangle rect1 , Rectangle rect2 ) => rect1 . Union ( rect2 ) ;

		public bool Contain ( Rectangle subRectangle )
			=> subRectangle . Top        >= Top
				&& subRectangle . Bottom <= Bottom
				&& subRectangle . Left   >= Left
				&& subRectangle . Right  <= Right ;

		public static bool Contain ( Rectangle rectangle , Rectangle subRectangle )
			=> rectangle . Contain ( subRectangle ) ;

		/// <summary>
		///     Offset - return the result of offsetting rect by the offset provided
		/// </summary>
		public static Rectangle Offset ( Rectangle rect , Vector offsetVector )
			=> rect . Offset ( offsetVector . X , offsetVector . Y ) ;

		public Size Size => new Size ( Width , Height ) ;

		/// <summary>
		///     Offset - return the result of offsetting rect by the offset provided
		/// </summary>
		public static Rectangle Offset ( Rectangle rect , int offsetX , int offsetY )
			=> rect . Offset ( offsetX , offsetY ) ;

		public override string ToString ( )
			=> $"{nameof ( X )}: {X}, {nameof ( Y )}: {Y}, {nameof ( Height )}: {Height}, {nameof ( Width )}: {Width}" ;

	}

}
