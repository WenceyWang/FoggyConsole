using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . FoggyConsole
{

	public static class RectangleExtensions
	{

		public static bool IsNotEmpty ( this Rectangle ? rect ) => rect ? . IsEmpty == false ;

		public static int MinColumnDiff ( this Rectangle baseRect , Rectangle targetRect )
		{
			if ( targetRect . Left < baseRect . Left )
			{
				if ( targetRect . Right > baseRect . Right )
				{
					return 0 ;
				}

				if ( targetRect . Right < baseRect . Left )
				{
					return targetRect . Right - baseRect . Left ;
				}

				return 0 ;
			}

			if ( targetRect . Left < baseRect . Right )
			{
				return 0 ;
			}

			return targetRect . Left - baseRect . Right ;
		}

		public static int MaxColumnDiff ( this Rectangle baseRect , Rectangle targetRect )
		{
			if ( targetRect . Left < baseRect . Left )
			{
				if ( targetRect . Right > baseRect . Right )
				{
					return Math . Max (
										baseRect . Left    - targetRect . Left ,
										targetRect . Right - targetRect . Right ) ;
				}

				return targetRect . Left - baseRect . Left ;
			}

			if ( targetRect . Right < baseRect . Right )
			{
				return 0 ;
			}

			return targetRect . Right - baseRect . Right ;
		}

		public static int MinRowDiff ( this Rectangle baseRect , Rectangle targetRect )
		{
			if ( targetRect . Top < baseRect . Top )
			{
				if ( targetRect . Bottom > baseRect . Bottom )
				{
					return 0 ;
				}

				if ( targetRect . Bottom < baseRect . Top )
				{
					return targetRect . Bottom - baseRect . Top ;
				}

				return 0 ;
			}

			if ( targetRect . Top < baseRect . Bottom )
			{
				return 0 ;
			}

			return targetRect . Top - baseRect . Bottom ;
		}

		public static int MaxRowDiff ( this Rectangle baseRect , Rectangle targetRect )
		{
			if ( targetRect . Top < baseRect . Top )
			{
				if ( targetRect . Bottom > baseRect . Bottom )
				{
					return Math . Max (
										baseRect . Top      - targetRect . Top ,
										targetRect . Bottom - targetRect . Bottom ) ;
				}

				return targetRect . Top - baseRect . Top ;
			}

			if ( targetRect . Bottom < baseRect . Bottom )
			{
				return 0 ;
			}

			return targetRect . Bottom - baseRect . Bottom ;
		}

	}

}
