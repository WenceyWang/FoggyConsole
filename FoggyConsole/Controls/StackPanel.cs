using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . FoggyConsole . Controls . Renderers ;

namespace DreamRecorder . FoggyConsole . Controls
{

	public class StackPanel : ItemsContainer
	{

		public override bool CanFocusedOn => false ;

		public StackPanel ( IControlRenderer renderer = null ) : base ( renderer ?? new ItemsContainerRenderer ( ) ) { }

		public StackPanel ( ) : this ( null ) { }

		public override void ArrangeOverride ( Rectangle finalRect )
		{
			int currentHeight = 0 ;
			for ( int i = 0 ; i < Items . Count && currentHeight < finalRect . Height ; i++ )
			{
				Control control = Items [ i ] ;

				Size arrangeSize ;

				Point arrangeLocation ;

				switch ( control . HorizontalAlign )
				{
					case ContentHorizontalAlign . Left :
					{
						arrangeLocation = finalRect . LeftTopPoint . Offset ( 0 , currentHeight ) ;
						arrangeSize = new Size (
												Math . Min ( finalRect . Width , control . DesiredSize . Width ) ,
												Math . Min (
															Math . Max ( finalRect . Height - currentHeight , 0 ) ,
															control . DesiredSize . Height ) ) ;
						break ;
					}

					case ContentHorizontalAlign . Center :
					{
						int controlWidth = Math . Min ( finalRect . Width , control . DesiredSize . Width ) ;
						arrangeLocation = finalRect . LeftTopPoint . Offset (
																			 ( finalRect . Width - controlWidth ) / 2 ,
																			 currentHeight ) ;
						arrangeSize = new Size (
												controlWidth ,
												Math . Min (
															Math . Max ( finalRect . Height - currentHeight , 0 ) ,
															control . DesiredSize . Height ) ) ;
						break ;
					}

					case ContentHorizontalAlign . Right :
					{
						int controlWidth = Math . Min ( finalRect . Width , control . DesiredSize . Width ) ;
						arrangeLocation = finalRect . LeftTopPoint . Offset (
																			 finalRect . Width - controlWidth ,
																			 currentHeight ) ;
						arrangeSize = new Size (
												controlWidth ,
												Math . Min (
															Math . Max ( finalRect . Height - currentHeight , 0 ) ,
															control . DesiredSize . Height ) ) ;
						break ;
					}

					default :
					case ContentHorizontalAlign . Stretch :
					{
						arrangeLocation = finalRect . LeftTopPoint . Offset ( 0 , currentHeight ) ;
						arrangeSize = new Size (
												finalRect . Width ,
												Math . Min (
															Math . Max ( finalRect . Height - currentHeight , 0 ) ,
															control . DesiredSize . Height ) ) ;

						break ;
					}
				}

				control . Arrange ( new Rectangle ( arrangeLocation , arrangeSize ) ) ;

				currentHeight += arrangeSize . Height ;
			}

			RenderArea = finalRect ;
		}

		public override void Measure ( Size availableSize )
		{
			int heightSum = 0 ;
			int maxWidth  = 0 ;

			foreach ( Control control in Items )
			{
				control . Measure ( new Size ( availableSize . Width , int . MaxValue ) ) ;
				heightSum += control . DesiredSize . Height ;
				maxWidth  =  Math . Max ( control . DesiredSize . Width , maxWidth ) ;
			}

			if ( ! AutoWidth )
			{
				maxWidth = Math . Max ( Width , maxWidth ) ;
			}

			if ( ! AutoHeight )
			{
				heightSum = Math . Max ( Height , heightSum ) ;
			}

			DesiredSize = new Size ( maxWidth , heightSum ) ;
		}

	}

}
