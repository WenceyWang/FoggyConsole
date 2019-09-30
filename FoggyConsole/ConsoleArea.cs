using System ;
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

        public Size ContentSize { get; }

		public ConsoleChar this [ int x , int y ]
		{
			get
			{
				if ( x    < 0
					 || x >= Size . Width )
				{
					return default ;
				}

				if ( y    < 0
					 || y >= Size . Height )
				{
					return default ;
				}

				return Content.Span[ Position . X + x +(( Position . Y + y )*ContentSize.Width)] ;
			}
			set
			{
				if ( x    < 0
					 || x >= Size . Width )
				{
					return ;
				}

				if ( y    < 0
					 || y >= Size . Height )
				{
					return ;
				}

				Content.Span [Position.X + x + ((Position.Y + y) * ContentSize.Width)] = value ;
			}
		}

		public Memory<ConsoleChar> Content { get ; }

		private ConsoleArea ( [NotNull] ConsoleArea area , Rectangle subRectangle )
		{
			if ( area == null )
			{
				throw new ArgumentNullException ( nameof ( area ) ) ;
			}

			if ( ! area . Position . Contain ( subRectangle ) )
			{
				subRectangle = area . Position . Intersect ( subRectangle ) ;
			}


			Content  = area . Content ;
            ContentSize = area.ContentSize;
			Position = subRectangle ;
		}

		public ConsoleArea ( Size size , ConsoleColor color ) : this (
																	  size ,
																	  new ConsoleChar (
																					   ' ' ,
																					   backgroundColor : color ) )
		{
		}

		public ConsoleArea ( Size size , ConsoleChar character )
        {
            ContentSize = size;
			Position = new Rectangle ( size ) ;
			Content  = new Memory<ConsoleChar>(new ConsoleChar[Size.Area]);
            Content.Span.Fill(character);
        }

		public ConsoleArea ( Size size ) : this ( size , ' ' ) { }

		public ConsoleArea CreateSub ( Rectangle rectangle ) => new ConsoleArea ( this , rectangle ) ;

		public void Fill ( ConsoleColor color ) { Fill ( new ConsoleChar ( ' ' , backgroundColor : color ) ) ; }

		public void Fill ( ConsoleChar character )
		{

            Rectangle contentArea = new Rectangle(new Point(), Size);

            bool changeLine = Position.Right != contentArea.Right || Position.Left != contentArea.Left;

            if (changeLine)
            {
                for (int y = contentArea.Top; y <= contentArea.Bottom; y++)
                {
                    Content.Span.Slice(Position.X  + ((Position.Y + y) * ContentSize.Width), Position.Width).Fill(character);
                }
            }
            else
            {
                Content.Span.Slice(Position.X + ((Position.Y) * ContentSize.Width), Position.Area).Fill(character);
            }

		}

	}

}
