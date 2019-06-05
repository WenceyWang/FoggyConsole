using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . FoggyConsole . Controls . Renderers
{

	/// <summary>
	///     Draws a
	///     <code>ProgressBar</code>
	///     -control
	/// </summary>
	public class ProgressBarRenderer : ControlRenderer <ProgressBar>
	{

		/// <summary>
		///     The character which is used to draw the bar
		/// </summary>
		public char ProgressChar { get ; set ; } = '|' ;

		public ProgressBarRenderer ( ProgressBar control ) { }

		/// <summary>
		///     Draws the ProgressBar given in the Control-Property
		/// </summary>
		public override void Draw ( ConsoleArea area )
		{
			area . Fill ( Control . ActualBackgroundColor ) ;

			if ( Control . ActualHeight == 1 )
			{
				area [ 0 , 0 ] = new ConsoleChar (
												'[' ,
												Control . ActualForegroundColor ,
												Control . ActualBackgroundColor ) ;
				area [ Control . ActualWidth - 1 , 0 ] = new ConsoleChar (
																		']' ,
																		Control . ActualForegroundColor ,
																		Control . ActualBackgroundColor ) ;
				int barWidth = Control . ActualWidth
								- 2
								* ( Control . Value
									- Control . MinValue / ( Control . MaxValue - Control . MinValue ) ) ;
				for ( int x = 0 ; x < barWidth ; x++ )
				{
					area [ x + 1 , 0 ] = new ConsoleChar (
														ProgressChar ,
														Control . ActualForegroundColor ,
														Control . ActualBackgroundColor ) ;
				}
			}
			else
			{
				area [ 0 , 0 ] = new ConsoleChar (
												'┌' ,
												Control . ActualForegroundColor ,
												Control . ActualBackgroundColor ) ;
				area [ Control . ActualWidth - 1 , 0 ] = new ConsoleChar (
																		'┐' ,
																		Control . ActualForegroundColor ,
																		Control . ActualBackgroundColor ) ;
				area [ 0 , Control . ActualHeight - 1 ] = new ConsoleChar (
																			'└' ,
																			Control . ActualForegroundColor ,
																			Control . ActualBackgroundColor ) ;
				area [ Control . ActualWidth - 1 , Control . ActualHeight - 1 ] =
					new ConsoleChar ( '┘' , Control . ActualForegroundColor , Control . ActualBackgroundColor ) ;

				int barWidth = Control . ActualWidth
								- 2
								* ( Control . Value
									- Control . MinValue / ( Control . MaxValue - Control . MinValue ) ) ;

				for ( int y = 0 ; y < Control . ActualHeight ; y++ )
				{
					for ( int x = 0 ; x < barWidth ; x++ )
					{
						area [ x + 1 , 0 ] = new ConsoleChar (
															ProgressChar ,
															Control . ActualForegroundColor ,
															Control . ActualBackgroundColor ) ;
					}
				}
			}
		}

	}

}
