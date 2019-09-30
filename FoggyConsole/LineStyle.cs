using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . ComponentModel ;
using System . Globalization ;
using System . Linq ;
using System . Reflection ;

using DreamRecorder . ToolBox . General ;

using JetBrains . Annotations ;

namespace DreamRecorder . FoggyConsole
{

	[AttributeUsage ( AttributeTargets . Property )]
	public sealed class LineStyleAttribute : Attribute
	{

	}

	/// <summary>
	///     A set of characters which can be used to draw a box or complex lines
	/// </summary>
	[TypeConverter ( typeof ( LineStyleTypeConverter ) )]
	public struct LineStyle : IEquatable <LineStyle>
	{

		public override string ToString ( )
			=> $"{TopLeftCorner}{TopRightCorner}{BottomLeftCorner}{BottomRightCorner}{VerticalEdge}{HorizontalEdge}{ConnectionHorizontalUp}{ConnectionHorizontalDown}{ConnectionVerticalRight}{ConnectionVerticalLeft}{ConnectionCross}{EmptyChar}{SingleLineLeftEdge}{SingleLineRightEdge}" ;

		public static implicit operator LineStyle ( [NotNull] string data )
		{
			if ( data == null )
			{
				throw new ArgumentNullException ( nameof ( data ) ) ;
			}

			PropertyInfo result =
				typeof ( LineStyle ) . GetProperty ( $"{data}Set" , BindingFlags . Static | BindingFlags . Public ) ;

			if ( result != null )
			{
				return ( LineStyle ) result . GetValue ( null ) ;
			}

			if ( data . Length < 14 )
			{
				return default ;
			}

			return new LineStyle (
								  data [ 0 ] ,
								  data [ 1 ] ,
								  data [ 2 ] ,
								  data [ 3 ] ,
								  data [ 4 ] ,
								  data [ 5 ] ,
								  data [ 6 ] ,
								  data [ 7 ] ,
								  data [ 8 ] ,
								  data [ 9 ] ,
								  data [ 10 ] ,
								  data [ 11 ] ,
								  data [ 12 ] ,
								  data [ 13 ] ) ;
		}

		public class LineStyleTypeConverter : TypeConverter
		{

			public override bool CanConvertFrom ( ITypeDescriptorContext context , Type sourceType )
				=> sourceType == typeof ( string ) ;

			public override bool CanConvertTo ( ITypeDescriptorContext context , Type destinationType )
				=> destinationType == typeof ( string ) ;

			public override object ConvertFrom ( ITypeDescriptorContext context , CultureInfo culture , object value )
			{
				if ( value is null )
				{
					return null ;
				}

				if ( value is string lineStyleData )
				{
					return ( LineStyle ) lineStyleData ;
				}

				return EmptySet ;
			}

			public override object ConvertTo (
				ITypeDescriptorContext context ,
				CultureInfo            culture ,
				object                 value ,
				Type                   destinationType )
			{
				if ( destinationType == typeof ( string ) )
				{
					return value . ToString ( ) ;
				}

				return destinationType . GetDefault ( ) ;
			}

		}

		/// <summary>
		///     A character representing the top left corner of an rectangle
		/// </summary>
		public char TopLeftCorner { get ; }

		/// <summary>
		///     A character representing the top right corner of an rectangle
		/// </summary>
		public char TopRightCorner { get ; }

		/// <summary>
		///     A character representing the bottom left corner of an rectangle
		/// </summary>
		public char BottomLeftCorner { get ; }

		/// <summary>
		///     A character representing the bottom right corner of an rectangle
		/// </summary>
		public char BottomRightCorner { get ; }

		/// <summary>
		///     A character representing the a left or right edge of an rectangle
		/// </summary>
		public char VerticalEdge { get ; }

		/// <summary>
		///     A character representing the a top or bottom edge of an rectangle
		/// </summary>
		public char HorizontalEdge { get ; }

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
		public char ConnectionHorizontalUp { get ; }

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
		public char ConnectionHorizontalDown { get ; }

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
		public char ConnectionVerticalRight { get ; }

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
		public char ConnectionVerticalLeft { get ; }

		/// <summary>
		///     A character representing a connection between two
		///     <code>VerticalEdge</code>
		///     and two
		///     <code>HorizontalEdge</code>
		/// </summary>
		public char ConnectionCross { get ; }

		public char SingleLineLeftEdge { get ; }

		public char SingleLineRightEdge { get ; }

		/// <summary>
		///     A empty character which can be used to fill boxes
		/// </summary>
		public char EmptyChar { get ; }

		public LineStyle (
			char topLeftCorner ,
			char topRightCorner ,
			char bottomLeftCorner ,
			char bottomRightCorner ,
			char verticalEdge ,
			char horizontalEdge ,
			char singleLineLeftEdge ,
			char singleLineRightEdge ,
			char connectionHorizontalUp ,
			char connectionHorizontalDown ,
			char connectionVerticalRight ,
			char connectionVerticalLeft ,
			char connectionCross ,
			char emptyChar )
		{
			TopLeftCorner            = topLeftCorner ;
			TopRightCorner           = topRightCorner ;
			BottomLeftCorner         = bottomLeftCorner ;
			BottomRightCorner        = bottomRightCorner ;
			VerticalEdge             = verticalEdge ;
			HorizontalEdge           = horizontalEdge ;
			ConnectionHorizontalUp   = connectionHorizontalUp ;
			ConnectionHorizontalDown = connectionHorizontalDown ;
			ConnectionVerticalRight  = connectionVerticalRight ;
			ConnectionVerticalLeft   = connectionVerticalLeft ;
			ConnectionCross          = connectionCross ;
			EmptyChar                = emptyChar ;
			SingleLineLeftEdge       = singleLineLeftEdge ;
			SingleLineRightEdge      = singleLineRightEdge ;
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
				hashCode = ( hashCode * 397 ) ^ SingleLineLeftEdge . GetHashCode ( ) ;
				hashCode = ( hashCode * 397 ) ^ SingleLineRightEdge . GetHashCode ( ) ;
				hashCode = ( hashCode * 397 ) ^ EmptyChar . GetHashCode ( ) ;
				return hashCode ;
			}
		}

		public bool Equals ( LineStyle other )
			=> TopLeftCorner               == other . TopLeftCorner
			   && TopRightCorner           == other . TopRightCorner
			   && BottomLeftCorner         == other . BottomLeftCorner
			   && BottomRightCorner        == other . BottomRightCorner
			   && VerticalEdge             == other . VerticalEdge
			   && HorizontalEdge           == other . HorizontalEdge
			   && ConnectionHorizontalUp   == other . ConnectionHorizontalUp
			   && ConnectionHorizontalDown == other . ConnectionHorizontalDown
			   && ConnectionVerticalRight  == other . ConnectionVerticalRight
			   && ConnectionVerticalLeft   == other . ConnectionVerticalLeft
			   && ConnectionCross          == other . ConnectionCross
			   && SingleLineLeftEdge       == other . SingleLineLeftEdge
			   && SingleLineRightEdge      == other . SingleLineRightEdge
			   && EmptyChar                == other . EmptyChar ;

		public override bool Equals ( object obj ) => obj is LineStyle other && Equals ( other ) ;

		public static bool operator == ( LineStyle left , LineStyle right ) => left . Equals ( right ) ;

		public static bool operator != ( LineStyle left , LineStyle right ) => ! left . Equals ( right ) ;

		/// <summary>
		///     A
		///     <code>LineStyle</code>
		///     which uses single-lined box-drawing-characters
		/// </summary>
		/// <returns></returns>
		[LineStyle]
		public static LineStyle EmptySet
			=> new LineStyle ( ' ' , ' ' , ' ' , ' ' , ' ' , ' ' , ' ' , ' ' , ' ' , ' ' , ' ' , ' ' , ' ' , ' ' ) ;


		/// <summary>
		///     A very simple
		///     <code>LineStyle</code>
		///     .
		/// </summary>
		[LineStyle]
		public static LineStyle SimpleSet
			=> new LineStyle ( '.' , '.' , '`' , '´' , '|' , '-' , '[' , ']' , '+' , '+' , '+' , '+' , '+' , ' ' ) ;


		/// <summary>
		///     A very simple
		///     <code>LineStyle</code>
		///     .
		/// </summary>
		[LineStyle]
		public static LineStyle CornerOnlySingleLineSet
			=> new LineStyle (
							  '\u250C' , // ┌
							  '\u2510' , // ┐
							  '\u2514' , // └
							  '\u2518' , // ┘
							  ' ' ,
							  ' ' ,
							  '[' ,
							  ']' ,
							  '\u2534' , // ┴
							  '\u252C' , // ┬
							  '\u251C' , // ├
							  '\u2524' , // ┤
							  '\u253C' , // ┼
							  ' ' ) ;

		/// <summary>
		///     A
		///     <code>LineStyle</code>
		///     which uses single-lined box-drawing-characters
		/// </summary>
		/// <returns></returns>
		[LineStyle]
		public static LineStyle SingleLineSet
			=> new LineStyle (
							  '\u250C' , // ┌
							  '\u2510' , // ┐
							  '\u2514' , // └
							  '\u2518' , // ┘
							  '\u2502' , // │
							  '\u2500' , // ─
							  '[' ,
							  ']' ,
							  '\u2534' , // ┴
							  '\u252C' , // ┬
							  '\u251C' , // ├
							  '\u2524' , // ┤
							  '\u253C' , // ┼
							  ' ' ) ;

		/// <summary>
		///     A
		///     <code>LineStyle</code>
		///     which uses double-lined box-drawing-characters
		/// </summary>
		[LineStyle]
		public static LineStyle DoubleLinesSet
			=> new LineStyle (
							  '\u2554' , // ╔
							  '\u2557' , // ╗
							  '\u255A' , // ╚
							  '\u255D' , // ╝
							  '\u2551' , // ║
							  '\u2550' , // ═
							  '⟦' ,
							  '⟧' ,
							  '\u2569' , // ╩
							  '\u2566' , // ╦
							  '\u2560' , // ╠
							  '\u2563' , // ╣
							  '\u256C' , // ╬
							  ' ' ) ;

	}

}
