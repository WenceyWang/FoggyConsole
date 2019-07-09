using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . FoggyConsole . Controls . Renderers ;

namespace DreamRecorder . FoggyConsole . Controls
{

	public class HorizontalStackPanel : ItemsContainer
	{

		public override bool CanFocusedOn => false ;

		public HorizontalStackPanel ( IControlRenderer renderer = null ) : base (
																				renderer
																				?? new ItemsContainerRenderer <
																					HorizontalStackPanel> ( ) )
		{
		}

		public HorizontalStackPanel ( ) : this ( null ) { }


		public override void Arrange ( Rectangle finalRect )
		{
			int currentWidth = 0 ;
			for ( int i = 0 ; i < Items . Count && currentWidth < finalRect . Width ; i++ )
			{
				Control control = Items [ i ] ;

				Size arrangeSize ;

				Point arrangeLocation ;

				switch ( control . VerticalAlign )
				{
					case ContentVerticalAlign . Top :
					{
						arrangeLocation = finalRect . LeftTopPoint . Offset ( currentWidth , 0 ) ;
						arrangeSize = new Size (
												Math . Min (
															Math . Max ( finalRect . Width - currentWidth , 0 ) ,
															control . DesiredSize . Width ) ,
												Math . Min ( finalRect . Height , control . DesiredSize . Height ) ) ;
						break ;
					}

					case ContentVerticalAlign . Center :
					{
						int controlHeight = Math . Min ( finalRect . Height , control . DesiredSize . Height ) ;
						arrangeLocation =
							finalRect . LeftTopPoint . Offset (
																currentWidth ,
																( finalRect . Height - controlHeight ) / 2 ) ;
						arrangeSize = new Size (
												Math . Min (
															Math . Max ( finalRect . Width - currentWidth , 0 ) ,
															control . DesiredSize . Width ) ,
												Math . Min ( finalRect . Height , control . DesiredSize . Height ) ) ;
						break ;
					}

					case ContentVerticalAlign . Bottom :
					{
						int controlHeight = Math . Min ( finalRect . Height , control . DesiredSize . Height ) ;
						arrangeLocation =
							finalRect . LeftTopPoint . Offset (
																currentWidth ,
																( finalRect . Height - controlHeight ) ) ;
						arrangeSize = new Size (
												Math . Min (
															Math . Max ( finalRect . Width - currentWidth , 0 ) ,
															control . DesiredSize . Width ) ,
												Math . Min ( finalRect . Height , control . DesiredSize . Height ) ) ;
						break ;
					}

					default :
					case ContentVerticalAlign . Stretch :
					{
						arrangeLocation = finalRect . LeftTopPoint . Offset ( currentWidth , 0 ) ;
						arrangeSize = new Size (
												Math . Min (
															Math . Max ( finalRect . Width - currentWidth , 0 ) ,
															control . DesiredSize . Width ) ,
												finalRect . Height ) ;

						break ;
					}
				}

				control . Arrange ( new Rectangle ( arrangeLocation , arrangeSize ) ) ;

				currentWidth += arrangeSize . Width ;
			}

			base . Arrange ( finalRect ) ;
		}

		public override void Measure ( Size availableSize )
		{
			int widthSum  = 0 ;
			int maxHeight = 0 ;

			foreach ( Control control in Items )
			{
				control . Measure ( new Size ( int . MaxValue , availableSize . Height ) ) ;
				widthSum  += control . DesiredSize . Width ;
				maxHeight =  Math . Max ( control . DesiredSize . Height , maxHeight ) ;
			}

			if ( ! AutoHeight )
			{
				maxHeight = Math . Max ( Height , maxHeight ) ;
			}

			if ( ! AutoWidth )
			{
				widthSum = Math . Max ( Width , widthSum ) ;
			}

			DesiredSize = new Size ( widthSum , maxHeight ) ;
		}

	}

}
