using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . FoggyConsole
{

	public struct ConsoleChar
	{

		public readonly char Character ;

		public readonly ConsoleColor ForegroundColor ;

		public readonly ConsoleColor BackgroundColor ;

		public bool Equals ( ConsoleChar other )
			=> Character           == other . Character
				&& ForegroundColor == other . ForegroundColor
				&& BackgroundColor == other . BackgroundColor ;

		public static implicit operator ConsoleChar ( char character )
			=> new ConsoleChar ( character , ConsoleColor . Gray ) ;

		public override bool Equals ( object obj )
		{
			if ( obj is null )
			{
				return false ;
			}

			return obj is ConsoleChar consoleChar && Equals ( consoleChar ) ;
		}

		public override int GetHashCode ( )
		{
			unchecked
			{
				int hashCode = Character . GetHashCode ( ) ;
				hashCode = ( hashCode * 397 ) ^ ( int ) ForegroundColor ;
				hashCode = ( hashCode * 397 ) ^ ( int ) BackgroundColor ;
				return hashCode ;
			}
		}

		public static bool operator == ( ConsoleChar left , ConsoleChar right ) => left . Equals ( right ) ;

		public static bool operator != ( ConsoleChar left , ConsoleChar right ) => ! left . Equals ( right ) ;

		public override string ToString ( ) => new string ( Character , 1 ) ;

		public ConsoleChar (
			char         character ,
			ConsoleColor foregroundColor = ConsoleColor . Gray ,
			ConsoleColor backgroundColor = ConsoleColor . Black )
		{
			Character       = character ;
			ForegroundColor = foregroundColor ;
			BackgroundColor = backgroundColor ;
		}

	}

}
