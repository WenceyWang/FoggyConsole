using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace WenceyWang . FoggyConsole
{

	public class ConsoleArea
	{

		public Size Size { get ; }

		public ConsoleChar this [ int x , int y ]
        {
            get
			{
				if ( x < 0
					|| x >= Content . GetLength ( 0 ) )
				{
					throw new ArgumentOutOfRangeException ( nameof(x) ) ;
				}

				if ( y < 0
					|| y >= Content . GetLength ( 1 ) )
				{
					throw new ArgumentOutOfRangeException ( nameof(y) ) ;
				}

				return Content [ x , y ] ;
			}
            set {
                if (x    < 0
                    || x >= Content.GetLength(0))
                {
                    throw new ArgumentOutOfRangeException(nameof(x));
                }

                if (y    < 0
                    || y >= Content.GetLength(1))
                {
                    throw new ArgumentOutOfRangeException(nameof(y));
                }
                Content[x, y] = value; }
        }

        public ConsoleChar [ , ] Content { get ; }

		public ConsoleArea ( Size size , ConsoleColor color ) : this ( size ,
																		new ConsoleChar ( ' ' , backgroundColor : color ) )
		{
		}

		public ConsoleArea ( Size size , ConsoleChar backGround )
		{
			Size = size ;
			Content = new ConsoleChar[ Size . Width , Size . Height ] ;
			for ( int y = 0 ; y < size . Height ; y++ )
			{
				for ( int x = 0 ; x < size . Width ; x++ )
				{
					Content [ x , y ] = backGround ;
				}
			}
		}

		public ConsoleArea ( Size size ) : this ( size , ' ' ) { }

	}

}
