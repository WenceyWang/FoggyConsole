using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . FoggyConsole . Controls . Renderers
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
		public override void Draw ( ConsoleArea area )
		{
			area . Fill ( Control . ActualBackgroundColor ) ;

			switch ( Control . HorizontalAlign )
			{
				default :
				case ContentHorizontalAlign . Stretch :
				case ContentHorizontalAlign . Left :
				{
					for ( int x = 0 ;
						x < Control . ActualWidth && x < Control . Text . Length ;
						x++ )
					{
						area [ x , 0 ] = new ConsoleChar (
														Control . Text [ x ] ,
														Control . ActualForegroundColor ,
														Control . ActualBackgroundColor ) ;
					}

					break ;
				}

				case ContentHorizontalAlign . Center :
				{
					int startPosition = ( Control . ActualWidth - Control . Text . Length ) / 2 ;
					for ( int x = 0 ;
						x < Control . ActualWidth && x < Control . Text . Length ;
						x++ )
					{
						area [ x + startPosition , 0 ] = new ConsoleChar (
																		Control . Text [ x ] ,
																		Control .
																			ActualForegroundColor ,
																		Control .
																			ActualBackgroundColor ) ;
					}

					break ;
				}

				case ContentHorizontalAlign . Right :
				{
					for ( int x = 0 ;
						x < Control . ActualWidth && x < Control . Text . Length ;
						x++ )
					{
						area [ Control . ActualWidth - Control . Text . Length + x , 0 ] =
							new ConsoleChar (
											Control . Text [ x ] ,
											Control . ActualForegroundColor ,
											Control . ActualBackgroundColor ) ;
					}

					break ;
				}
			}
		}

	}

}
