using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace WenceyWang . FoggyConsole . Controls . Renderers
{

	/// <summary>
	///     Draws a
	///     <code>Label</code>
	///     -Control
	/// </summary>
	public class LabelRenderer : ControlRenderer <Label>
	{

		/// <summary>
		///     Draws the
		///     <code>Label</code>
		///     given in the Control-Property.
		/// </summary>
		/// <exception cref="InvalidOperationException">Is thrown if the Control-Property isn't set.</exception>
		/// <exception cref="InvalidOperationException">Is thrown if the CalculateBoundary-Method hasn't been called.</exception>
		public override void Draw ( )
		{
			ConsoleArea result = new ConsoleArea ( Control . ActualSize , Control . ActualBackgroundColor ) ;

			switch ( Control . Align )
			{
				case ContentAlign . Left :
				{
					for ( int x = 0 ; x < Control . ActualWidth && x < Control . Text . Length ; x++ )
					{
						result [ x , 0 ] = new ConsoleChar (
															Control . Text [ x ] ,
															Control . ActualForegroundColor ,
															Control . ActualBackgroundColor ) ;
					}

					break ;
				}

				case ContentAlign . Center :
				{
					int startPosition = ( Control . RenderArea . Width - Control . Text . Length ) / 2 ;
					for ( int x = 0 ; x < Control . ActualWidth && x < Control . Text . Length ; x++ )
					{
						result [ x + startPosition , 0 ] = new ConsoleChar (
																			Control . Text [ x ] ,
																			Control . ActualForegroundColor ,
																			Control . ActualBackgroundColor ) ;
					}

					break ;
				}

				case ContentAlign . Right :
				{
					for ( int x = 0 ; x < Control . ActualWidth && x < Control . Text . Length ; x++ )
					{
						result [ Control . ActualWidth - Control . Text . Length + x , 0 ] =
							new ConsoleChar (
											Control . Text [ x ] ,
											Control . ActualForegroundColor ,
											Control . ActualBackgroundColor ) ;
					}

					break ;
				}
			}

			FogConsole . Draw ( Control . RenderPoint , result ) ;
		}

	}

}
