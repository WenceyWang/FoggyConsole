﻿using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using JetBrains . Annotations ;

namespace DreamRecorder . FoggyConsole
{

	public class ConsoleArea
	{

		public Size Size => Position . Size ;

		public Rectangle Position { get ; }

		public ConsoleChar this [ int x , int y ]
		{
			get
			{
				if ( x   < 0
					|| x >= Size . Width )
				{
					throw new ArgumentOutOfRangeException ( nameof ( x ) ) ;
				}

				if ( y   < 0
					|| y >= Size . Height )
				{
					throw new ArgumentOutOfRangeException ( nameof ( y ) ) ;
				}

				return Content [ Position . X + x , Position . Y + y ] ;
			}
			set
			{
				if ( x   < 0
					|| x >= Size . Width )
				{
#if DEBUG
					throw new ArgumentOutOfRangeException ( nameof ( x ) ) ;
#endif

					return ;
				}

				if ( y   < 0
					|| y >= Size . Height )
				{
#if DEBUG
					throw new ArgumentOutOfRangeException ( nameof ( x ) ) ;
#endif
					return ;
				}

				Content [ Position . X + x , Position . Y + y ] = value ;
			}
		}

		internal ConsoleChar [ , ] Content { get ; }

		private ConsoleArea ( [NotNull] ConsoleArea area , Rectangle subRectangle )
		{
			if ( area == null )
			{
				throw new ArgumentNullException ( nameof ( area ) ) ;
			}

			if ( ! area . Position . Contain ( subRectangle ) )
			{
#if DEBUG
				throw new ArgumentException (
											$"{nameof ( subRectangle )} should be contain in {nameof ( area )}.{nameof ( area . Position )}" ) ;
#endif
				subRectangle = area . Position . Intersect ( subRectangle ) ;
			}


			Content  = area . Content ;
			Position = subRectangle ;
		}

		public ConsoleArea ( Size size , ConsoleColor color ) : this (
																	size ,
																	new ConsoleChar (
																					' ' ,
																					backgroundColor
																					: color ) )
		{
		}

		public ConsoleArea ( Size size , ConsoleChar character )
		{
			Position = new Rectangle ( size ) ;
			Content  = new ConsoleChar[ Size . Width , Size . Height ] ;
			Fill ( character ) ;
		}

		public ConsoleArea ( Size size ) : this ( size , ' ' ) { }

		public ConsoleArea CreateSub ( Rectangle rectangle )
			=> new ConsoleArea ( this , rectangle ) ;

		public void Fill ( ConsoleColor color )
		{
			Fill ( new ConsoleChar ( ' ' , backgroundColor : color ) ) ;
		}

		public void Fill ( ConsoleChar character )
		{
			for ( int y = 0 ; y < Size . Height ; y++ )
			{
				for ( int x = 0 ; x < Size . Width ; x++ )
				{
					this [ x , y ] = character ;
				}
			}
		}

	}

}
