using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Globalization ;
using System . Linq ;

namespace DreamRecorder . FoggyConsole
{

	public struct Size
	{

		public static readonly Size Empty = new Size ( ) ;

		public Size ( Point point )
		{
			Width  = point . X ;
			Height = point . Y ;
		}

		public Size ( int width , int height )
		{
			Width  = width ;
			Height = height ;
		}

		public static Size operator + ( Size size1 , Size size2 ) => Add ( size1 , size2 ) ;

		public static Size operator - ( Size size1 , Size size2 ) => Subtract ( size1 , size2 ) ;

		public static bool operator == ( Size size1 , Size size2 )
			=> size1 . Width == size2 . Width && size1 . Height == size2 . Height ;

		public static bool operator != ( Size size1 , Size size2 ) => ! ( size1 == size2 ) ;

		public static explicit operator Point ( Size size ) => new Point ( size . Width , size . Height ) ;

		public bool IsEmpty => Width == 0 || Height == 0 ;

		public int Width { get ; }

		public int Height { get ; }

		public static Size Add ( Size size1 , Size size2 )
			=> new Size ( size1 . Width + size2 . Width , size1 . Height + size2 . Height ) ;

		public static Size Subtract ( Size size1 , Size size2 )
			=> new Size ( size1 . Width - size2 . Width , size1 . Height - size2 . Height ) ;

		public override bool Equals ( object obj )
		{
			if ( ! ( obj is Size ) )
			{
				return false ;
			}

			Size comp = ( Size ) obj ;

			// Note value types can't have derived classes, so we don't need to
			// check the types of the objects here.  -- Microsoft, 2/21/2001
			return comp . Width == Width && comp . Height == Height ;
		}

		public override int GetHashCode ( ) => Width ^ Height ;

		public override string ToString ( )
			=> "{Width="
				+ Width . ToString ( CultureInfo . CurrentCulture )
				+ ", Height="
				+ Height . ToString ( CultureInfo . CurrentCulture )
				+ "}" ;

	}

}
