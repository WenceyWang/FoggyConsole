using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . FoggyConsole . Controls . Renderers ;

namespace DreamRecorder . FoggyConsole . Controls
{

	public class HorizontalStackPanel : ItemsContainer
	{

		public override bool CanFocus => false ;

		public HorizontalStackPanel ( IControlRenderer renderer = null ) : base (
																				renderer ?? new StackPanelRenderer ( ) )
		{
		}


		public override void Arrange ( Rectangle finalRect )
		{
			int currentHeight = 0 ;
			for ( int i = 0 ; i < Items . Count && currentHeight < finalRect . Height ; i++ )
			{
				Control control = Items [ i ] ;
				switch ( control . HorizontalAlign )
				{
					case ContentHorizontalAlign . Left :
					{
						control . Arrange (
											new Rectangle (
															finalRect . LeftTopPoint . Offset ( 0 , currentHeight ) ,
															new Size (
																	Math . Min (
																				finalRect . Width ,
																				control . DesiredSize . Width ) ,
																	Math . Min (
																				Math . Max (
																							finalRect . Height
																							- currentHeight ,
																							0 ) ,
																				control . DesiredSize . Height ) ) ) ) ;
						break ;
					}

					case ContentHorizontalAlign . Center :
					{
						int controlWidth = Math . Min ( finalRect . Width , control . DesiredSize . Width ) ;
						control . Arrange (
											new Rectangle (
															finalRect . LeftTopPoint . Offset (
																								( finalRect . Width
																								- controlWidth )
																								/ 2 ,
																								currentHeight ) ,
															new Size (
																	controlWidth ,
																	Math . Min (
																				Math . Max (
																							finalRect . Height
																							- currentHeight ,
																							0 ) ,
																				control . DesiredSize . Height ) ) ) ) ;
						break ;
					}

					case ContentHorizontalAlign . Right :
					{
						int controlWidth = Math . Min ( finalRect . Width , control . DesiredSize . Width ) ;
						control . Arrange (
											new Rectangle (
															finalRect . LeftTopPoint . Offset (
																								finalRect . Width
																								- controlWidth ,
																								currentHeight ) ,
															new Size (
																	controlWidth ,
																	Math . Min (
																				Math . Max (
																							finalRect . Height
																							- currentHeight ,
																							0 ) ,
																				control . DesiredSize . Height ) ) ) ) ;
						break ;
					}

					case ContentHorizontalAlign . Stretch :
					{
						control . Arrange (
											new Rectangle (
															finalRect . LeftTopPoint . Offset ( 0 , currentHeight ) ,
															new Size (
																	finalRect . Width ,
																	Math . Min (
																				Math . Max (
																							finalRect . Height
																							- currentHeight ,
																							0 ) ,
																				control . DesiredSize . Height ) ) ) ) ;

						break ;
					}
				}

				currentHeight += control . ActualSize . Height ;
			}

			base . Arrange ( finalRect ) ;
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

			int resultWidth = maxWidth ;

			if ( HorizontalAlign == ContentHorizontalAlign . Stretch )
			{
				resultWidth = Math . Max ( resultWidth , availableSize . Width ) ;
			}

			int resultHeight = heightSum ;

			if ( HorizontalAlign == ContentHorizontalAlign . Stretch )
			{
				resultHeight = Math . Max ( resultHeight , availableSize . Height ) ;
			}

			DesiredSize = new Size ( resultWidth , resultHeight ) ;
		}

	}


	public class StackPanel : ItemsContainer
	{

		public override bool CanFocus => false ;

		public StackPanel ( IControlRenderer renderer = null ) : base ( renderer ?? new StackPanelRenderer ( ) ) { }


		public override void Arrange ( Rectangle finalRect )
		{
			int currentHeight = 0 ;
			for ( int i = 0 ; i < Items . Count && currentHeight < finalRect . Height ; i++ )
			{
				Control control = Items [ i ] ;
				switch ( control . HorizontalAlign )
				{
					case ContentHorizontalAlign . Left :
					{
						control . Arrange (
											new Rectangle (
															finalRect . LeftTopPoint . Offset ( 0 , currentHeight ) ,
															new Size (
																	Math . Min (
																				finalRect . Width ,
																				control . DesiredSize . Width ) ,
																	Math . Min (
																				Math . Max (
																							finalRect . Height
																							- currentHeight ,
																							0 ) ,
																				control . DesiredSize . Height ) ) ) ) ;
						break ;
					}

					case ContentHorizontalAlign . Center :
					{
						int controlWidth = Math . Min ( finalRect . Width , control . DesiredSize . Width ) ;
						control . Arrange (
											new Rectangle (
															finalRect . LeftTopPoint . Offset (
																								( finalRect . Width
																								- controlWidth )
																								/ 2 ,
																								currentHeight ) ,
															new Size (
																	controlWidth ,
																	Math . Min (
																				Math . Max (
																							finalRect . Height
																							- currentHeight ,
																							0 ) ,
																				control . DesiredSize . Height ) ) ) ) ;
						break ;
					}

					case ContentHorizontalAlign . Right :
					{
						int controlWidth = Math . Min ( finalRect . Width , control . DesiredSize . Width ) ;
						control . Arrange (
											new Rectangle (
															finalRect . LeftTopPoint . Offset (
																								finalRect . Width
																								- controlWidth ,
																								currentHeight ) ,
															new Size (
																	controlWidth ,
																	Math . Min (
																				Math . Max (
																							finalRect . Height
																							- currentHeight ,
																							0 ) ,
																				control . DesiredSize . Height ) ) ) ) ;
						break ;
					}

					case ContentHorizontalAlign . Stretch :
					{
						control . Arrange (
											new Rectangle (
															finalRect . LeftTopPoint . Offset ( 0 , currentHeight ) ,
															new Size (
																	finalRect . Width ,
																	Math . Min (
																				Math . Max (
																							finalRect . Height
																							- currentHeight ,
																							0 ) ,
																				control . DesiredSize . Height ) ) ) ) ;

						break ;
					}
				}

				currentHeight += control . ActualSize . Height ;
			}

			base . Arrange ( finalRect ) ;
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

			int resultWidth = maxWidth ;

			if ( HorizontalAlign == ContentHorizontalAlign . Stretch )
			{
				resultWidth = Math . Max ( resultWidth , availableSize . Width ) ;
			}

			int resultHeight = heightSum ;

			if ( HorizontalAlign == ContentHorizontalAlign . Stretch )
			{
				resultHeight = Math . Max ( resultHeight , availableSize . Height ) ;
			}

			DesiredSize = new Size ( resultWidth , resultHeight ) ;
		}

	}

}
