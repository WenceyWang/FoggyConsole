using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . FoggyConsole . Controls . Events ;
using DreamRecorder . FoggyConsole . Controls . Renderers ;

namespace DreamRecorder . FoggyConsole . Controls
{

	public class StackPanel : ItemsContainer
	{

		public override bool CanFocus => false ;

		public Dictionary <Control , ContentAlign> ControlAlign { get ; } =
			new Dictionary <Control , ContentAlign> ( ) ;


		public ContentAlign this [ Control control ]
		{
			get
			{
				if ( ControlAlign . ContainsKey ( control ) )
				{
					return ControlAlign [ control ] ;
				}
				else
				{
					return default ;
				}
			}
			set => ControlAlign [ control ] = value ;
		}

		public StackPanel ( IControlRenderer renderer = null ) : base ( renderer ?? new StackPanelRenderer ( ) )
		{
			ItemsAdded   += StackPanel_ItemsAdded ;
			ItemsRemoved += StackPanel_ItemsRemoved ;
		}

		private void StackPanel_ItemsRemoved ( object sender , ContainerControlEventArgs e )
		{
			ControlAlign . Remove ( e . Control ) ;
		}

		private void StackPanel_ItemsAdded ( object sender , ContainerControlEventArgs e )
		{
			ControlAlign [ e . Control ] = default ;
		}

		public override void Arrange ( Rectangle finalRect )
		{
			int currentHeight = 0 ;
			for ( int i = 0 ; i < Items . Count && currentHeight < finalRect . Height ; i++ )
			{
				Control control = Items [ i ] ;
				switch ( this [ control ] )
				{
					case ContentAlign . Left :
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

					case ContentAlign . Center :
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

					case ContentAlign . Right :
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

				switch ( ControlAlign [ control ] )
				{
					case ContentAlign . Left :
					case ContentAlign . Center :
						maxWidth = Math . Max ( control . DesiredSize . Width , maxWidth ) ;
						break ;
					default :
						maxWidth = Math . Max (
												availableSize . Width ,
												Math . Max ( control . DesiredSize . Width , maxWidth ) ) ;
						break ;
				}
			}

			DesiredSize = new Size ( maxWidth , heightSum ) ;
		}

	}

}
