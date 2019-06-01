using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace WenceyWang . FoggyConsole
{

	public struct Point
	{

		/// <summary>
		///     X - double.  Default value is 0.
		/// </summary>
		public int X { get ; }

		/// <summary>
		///     Y - double.  Default value is 0.
		/// </summary>
		public int Y { get ; }


		/// <summary>
		///     Offset - update the location by adding offsetX to X and offsetY to Y
		/// </summary>
		/// <param name="offsetX"> The offset in the x dimension </param>
		/// <param name="offsetY"> The offset in the y dimension </param>
		public Point Offset ( int offsetX , int offsetY ) { return new Point ( X + offsetX , Y + offsetY ) ; }

		public Point Offset ( Vector vector ) { return new Point ( X + vector . X , Y + vector . Y ) ; }

		/// <summary>
		///     Operator Point + Vector
		/// </summary>
		/// <returns>
		///     Point - The result of the addition
		/// </returns>
		/// <param name="point"> The Point to be added to the Vector </param>
		/// <param name="vector"> The Vector to be added to the Point </param>
		public static Point operator + ( Point point , Vector vector )
		{
			return new Point ( point . X + vector . X , point . Y + vector . Y ) ;
		}

		/// <summary>
		///     Operator Point - Vector
		/// </summary>
		/// <returns>
		///     Point - The result of the subtraction
		/// </returns>
		/// <param name="point"> The Point from which the Vector is subtracted </param>
		/// <param name="vector"> The Vector which is subtracted from the Point </param>
		public static Point operator - ( Point point , Vector vector )
		{
			return new Point ( point . X - vector . X , point . Y - vector . Y ) ;
		}

		/// <summary>
		///     Operator Point - Point
		/// </summary>
		/// <returns>
		///     Vector - The result of the subtraction
		/// </returns>
		/// <param name="point1"> The Point from which point2 is subtracted </param>
		/// <param name="point2"> The Point subtracted from point1 </param>
		public static Vector operator - ( Point point1 , Point point2 )
		{
			return new Vector ( point1 . X - point2 . X , point1 . Y - point2 . Y ) ;
		}


		/// <summary>
		///     Explicit conversion to Size.  Note that since Size cannot contain negative values,
		///     the resulting size will contains the absolute values of X and Y
		/// </summary>
		/// <returns>
		///     Size - A Size equal to this Point
		/// </returns>
		/// <param name="point"> Point - the Point to convert to a Size </param>
		public static explicit operator Size ( Point point )
		{
			return new Size ( Math . Abs ( point . X ) , Math . Abs ( point . Y ) ) ;
		}

		/// <summary>
		///     Explicit conversion to Vector
		/// </summary>
		/// <returns>
		///     Vector - A Vector equal to this Point
		/// </returns>
		/// <param name="point"> Point - the Point to convert to a Vector </param>
		public static explicit operator Vector ( Point point ) { return new Vector ( point . X , point . Y ) ; }

		/// <summary>
		///     Constructor which accepts the X and Y values
		/// </summary>
		/// <param name="x">The value for the X coordinate of the new Point</param>
		/// <param name="y">The value for the Y coordinate of the new Point</param>
		public Point ( int x , int y )
		{
			X = x ;
			Y = y ;
		}

		/// <summary>
		///     Add: Point + Vector
		/// </summary>
		/// <returns>
		///     Point - The result of the addition
		/// </returns>
		/// <param name="point"> The Point to be added to the Vector </param>
		/// <param name="vector"> The Vector to be added to the Point </param>
		public static Point Add ( Point point , Vector vector )
		{
			return new Point ( point . X + vector . X , point . Y + vector . Y ) ;
		}

		/// <summary>
		///     Subtract: Point - Vector
		/// </summary>
		/// <returns>
		///     Point - The result of the subtraction
		/// </returns>
		/// <param name="point"> The Point from which the Vector is subtracted </param>
		/// <param name="vector"> The Vector which is subtracted from the Point </param>
		public static Point Subtract ( Point point , Vector vector )
		{
			return new Point ( point . X - vector . X , point . Y - vector . Y ) ;
		}

		/// <summary>
		///     Subtract: Point - Point
		/// </summary>
		/// <returns>
		///     Vector - The result of the subtraction
		/// </returns>
		/// <param name="point1"> The Point from which point2 is subtracted </param>
		/// <param name="point2"> The Point subtracted from point1 </param>
		public static Vector Subtract ( Point point1 , Point point2 )
		{
			return new Vector ( point1 . X - point2 . X , point1 . Y - point2 . Y ) ;
		}

	}

}
