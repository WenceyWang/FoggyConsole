using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . FoggyConsole
{

	public struct Vector
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
		///     Length Property - the length of this Vector
		/// </summary>
		public double Length => Math . Sqrt ( X * X + Y * Y ) ;

		/// <summary>
		///     LengthSquared Property - the squared length of this Vector
		/// </summary>
		public double LengthSquared => X * X + Y * Y ;

		/// <summary>
		///     Constructor which sets the vector's initial values
		/// </summary>
		/// <param name="x"> double - The initial X </param>
		/// <param name="y"> double - THe initial Y </param>
		public Vector ( int x , int y )
		{
			X = x ;
			Y = y ;
		}

		public Vector ( Point point )
		{
			X = point . X ;
			Y = point . Y ;
		}

		public Vector ( Size size )
		{
			X = size . Width ;
			Y = size . Height ;
		}

		/// <summary>
		///     CrossProduct - Returns the cross product: vector1.X*vector2.Y - vector1.Y*vector2.X
		/// </summary>
		/// <returns>
		///     Returns the cross product: vector1.X*vector2.Y - vector1.Y*vector2.X
		/// </returns>
		/// <param name="vector1"> The first Vector </param>
		/// <param name="vector2"> The second Vector </param>
		public static int CrossProduct ( Vector vector1 , Vector vector2 )
			=> vector1 . X * vector2 . Y - vector1 . Y * vector2 . X ;

		/// <summary>
		///     AngleBetween - the angle between 2 vectors
		/// </summary>
		/// <returns>
		///     Returns the the angle in degrees between vector1 and vector2
		/// </returns>
		/// <param name="vector1"> The first Vector </param>
		/// <param name="vector2"> The second Vector </param>
		public static double AngleBetween ( Vector vector1 , Vector vector2 )
		{
			double sin = vector1 . X * vector2 . Y - vector2 . X * vector1 . Y ;
			double cos = vector1 . X * vector2 . X + vector1 . Y * vector2 . Y ;
			return Math . Atan2 ( sin , cos ) * ( 180 / Math . PI ) ;
		}


		/// <summary>
		///     Operator -Vector (unary negation)
		/// </summary>
		public static Vector operator - ( Vector vector )
			=> new Vector ( - vector . X , - vector . Y ) ;

		/// <summary>
		///     Negates the values of X and Y on this Vector
		/// </summary>
		public Vector Negate ( ) => new Vector ( - X , - Y ) ;

		/// <summary>
		///     Operator Vector + Vector
		/// </summary>
		public static Vector operator + ( Vector vector1 , Vector vector2 )
			=> new Vector ( vector1 . X + vector2 . X , vector1 . Y + vector2 . Y ) ;

		/// <summary>
		///     Add: Vector + Vector
		/// </summary>
		public static Vector Add ( Vector vector1 , Vector vector2 )
			=> new Vector ( vector1 . X + vector2 . X , vector1 . Y + vector2 . Y ) ;

		/// <summary>
		///     Operator Vector - Vector
		/// </summary>
		public static Vector operator - ( Vector vector1 , Vector vector2 )
			=> new Vector ( vector1 . X - vector2 . X , vector1 . Y - vector2 . Y ) ;

		/// <summary>
		///     Subtract: Vector - Vector
		/// </summary>
		public static Vector Subtract ( Vector vector1 , Vector vector2 )
			=> new Vector ( vector1 . X - vector2 . X , vector1 . Y - vector2 . Y ) ;

		/// <summary>
		///     Operator Vector + Point
		/// </summary>
		public static Point operator + ( Vector vector , Point point )
			=> new Point ( point . X + vector . X , point . Y + vector . Y ) ;

		/// <summary>
		///     Add: Vector + Point
		/// </summary>
		public static Point Add ( Vector vector , Point point )
			=> new Point ( point . X + vector . X , point . Y + vector . Y ) ;

		/// <summary>
		///     Operator Vector * double
		/// </summary>
		public static Vector operator * ( Vector vector , double scalar )
			=> new Vector ( ( int ) ( vector . X * scalar ) , ( int ) ( vector . Y * scalar ) ) ;

		/// <summary>
		///     Multiply: Vector * double
		/// </summary>
		public static Vector Multiply ( Vector vector , double scalar )
			=> new Vector ( ( int ) ( vector . X * scalar ) , ( int ) ( vector . Y * scalar ) ) ;

		/// <summary>
		///     Operator double * Vector
		/// </summary>
		public static Vector operator * ( double scalar , Vector vector )
			=> new Vector ( ( int ) ( vector . X * scalar ) , ( int ) ( vector . Y * scalar ) ) ;

		/// <summary>
		///     Multiply: double * Vector
		/// </summary>
		public static Vector Multiply ( double scalar , Vector vector )
			=> new Vector ( ( int ) ( vector . X * scalar ) , ( int ) ( vector . Y * scalar ) ) ;

		/// <summary>
		///     Operator Vector / double
		/// </summary>
		public static Vector operator / ( Vector vector , double scalar )
			=> vector * ( 1.0 / scalar ) ;

		/// <summary>
		///     Multiply: Vector / double
		/// </summary>
		public static Vector Divide ( Vector vector , double scalar ) => vector * ( 1.0 / scalar ) ;

		/// <summary>
		///     Operator Vector * Vector, interpreted as their dot product
		/// </summary>
		public static int operator * ( Vector vector1 , Vector vector2 )
			=> vector1 . X * vector2 . X + vector1 . Y * vector2 . Y ;

		/// <summary>
		///     Multiply - Returns the dot product: vector1.X*vector2.X + vector1.Y*vector2.Y
		/// </summary>
		/// <returns>
		///     Returns the dot product: vector1.X*vector2.X + vector1.Y*vector2.Y
		/// </returns>
		/// <param name="vector1"> The first Vector </param>
		/// <param name="vector2"> The second Vector </param>
		public static int Multiply ( Vector vector1 , Vector vector2 )
			=> vector1 . X * vector2 . X + vector1 . Y * vector2 . Y ;

		/// <summary>
		///     Determinant - Returns the determinant det(vector1, vector2)
		/// </summary>
		/// <returns>
		///     Returns the determinant: vector1.X*vector2.Y - vector1.Y*vector2.X
		/// </returns>
		/// <param name="vector1"> The first Vector </param>
		/// <param name="vector2"> The second Vector </param>
		public static int Determinant ( Vector vector1 , Vector vector2 )
			=> vector1 . X * vector2 . Y - vector1 . Y * vector2 . X ;

		/// <summary>
		///     Explicit conversion to Size.  Note that since Size cannot contain negative values,
		///     the resulting size will contains the absolute values of X and Y
		/// </summary>
		/// <returns>
		///     Size - A Size equal to this Vector
		/// </returns>
		/// <param name="vector"> Vector - the Vector to convert to a Size </param>
		public static explicit operator Size ( Vector vector )
			=> new Size ( Math . Abs ( vector . X ) , Math . Abs ( vector . Y ) ) ;

		/// <summary>
		///     Explicit conversion to Point
		/// </summary>
		/// <returns>
		///     Point - A Point equal to this Vector
		/// </returns>
		/// <param name="vector"> Vector - the Vector to convert to a Point </param>
		public static explicit operator Point ( Vector vector )
			=> new Point ( vector . X , vector . Y ) ;

	}

}
