using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . FoggyConsole . Controls ;

namespace DreamRecorder . FoggyConsole
{

	public struct CheckableChar : IEquatable <CheckableChar>
	{

		public char Checked { get ; }

		public char Unchecked { get ; }

		public char Indeterminate { get ; }

		public static CheckableChar Default => new CheckableChar ( 'X' , ' ' , '?' ) ;

		public CheckableChar ( char @checked , char @unchecked , char indeterminate )
		{
			Checked       = @checked ;
			Unchecked     = @unchecked ;
			Indeterminate = indeterminate ;
		}

		public char GetStateChar ( CheckState state )
		{
			switch ( state )
			{
				case CheckState . Checked :
				{
					return Checked ;
				}

				case CheckState . Unchecked :
				{
					return Unchecked ;
				}

				case CheckState . Indeterminate :
				{
					return Indeterminate ;
				}

				default :
				{
					throw new ArgumentOutOfRangeException ( nameof ( state ) , state , null ) ;
				}
			}
		}

		public bool Equals ( CheckableChar other )
			=> Checked           == other . Checked
				&& Unchecked     == other . Unchecked
				&& Indeterminate == other . Indeterminate ;

		public override bool Equals ( object obj )
		{
			if ( obj is null )
			{
				return false ;
			}

			return obj is CheckableChar c && Equals ( c ) ;
		}

		public override int GetHashCode ( )
		{
			unchecked
			{
				int hashCode = Checked . GetHashCode ( ) ;
				hashCode = ( hashCode * 397 ) ^ Unchecked . GetHashCode ( ) ;
				hashCode = ( hashCode * 397 ) ^ Indeterminate . GetHashCode ( ) ;
				return hashCode ;
			}
		}

		public static bool operator == ( CheckableChar left , CheckableChar right )
			=> left . Equals ( right ) ;

		public static bool operator != ( CheckableChar left , CheckableChar right )
			=> ! left . Equals ( right ) ;

	}

}
