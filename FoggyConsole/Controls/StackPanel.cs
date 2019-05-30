using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using WenceyWang . FoggyConsole . Controls . Renderers ;

namespace WenceyWang . FoggyConsole . Controls
{

	public class StackPanel : ItemsControl
	{

		public override bool CanFocus => false ;

		public override IList <Control> Items { get ; } = new List <Control> ( ) ;

		public Dictionary <Control , ContentAlign> ControlAlign { get ; } = new Dictionary <Control , ContentAlign> ( ) ;

		public ContentAlign this [ Control control ]
        {
            get
            {
                if (ControlAlign.ContainsKey(control))
                {
                    return ControlAlign[control];

                }
                else
                {
                    return default;
                }
            }
            set => ControlAlign [ control ] = value ;
        }

        public StackPanel ( IControlRenderer renderer = null ) : base ( renderer ?? new StackPanelRenderer ( ) ) { }

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
						control . Arrange ( new Rectangle ( finalRect . LeftTopPoint . Offset ( 0 , currentHeight ) ,
															new Size ( Math . Min ( finalRect . Width , control . DesiredSize . Width ) ,
																		Math . Min ( Math . Max ( finalRect . Height - currentHeight , 0 ) ,
																					control . DesiredSize . Height ) ) ) ) ;
						break ;
					}
					case ContentAlign . Center :
					{
						int controlWidth = Math . Min ( finalRect . Width , control . DesiredSize . Width ) ;
						control . Arrange ( new Rectangle ( finalRect . LeftTopPoint . Offset ( ( control . DesiredSize . Width
																								- controlWidth )
																								/ 2 ,
																								currentHeight ) ,
															new Size ( controlWidth ,
																		Math . Min ( Math . Max ( finalRect . Height - currentHeight , 0 ) ,
																					control . DesiredSize . Height ) ) ) ) ;
						break ;
					}
					case ContentAlign . Right :
					{
						int controlWidth = Math . Min ( finalRect . Width , control . DesiredSize . Width ) ;
						control . Arrange ( new Rectangle ( finalRect . LeftTopPoint . Offset ( control . DesiredSize . Width
																								- controlWidth ,
																								currentHeight ) ,
															new Size ( controlWidth ,
																		Math . Min ( Math . Max ( finalRect . Height - currentHeight , 0 ) ,
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
			foreach ( Control control in Items )
			{
				control . Measure ( new Size ( Math . Min ( availableSize . Width , control . Width ) , control . Height ) ) ;
			}

			base . Measure ( availableSize ) ;
		}

	}

}
