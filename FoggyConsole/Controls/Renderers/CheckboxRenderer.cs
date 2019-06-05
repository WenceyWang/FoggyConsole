using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . FoggyConsole . Controls . Renderers
{

	/// <summary>
	///     Draws a
	///     <code>CheckBox</code>
	///     -Control
	/// </summary>
	public class CheckboxRenderer : ControlRenderer <CheckBox>
	{

		public override void Draw ( ConsoleArea area )
		{
			area . Fill ( Control . ActualBackgroundColor ) ;

			area [ 0 , 0 ] = new ConsoleChar (
											'[' ,
											Control . ActualForegroundColor ,
											Control . ActualBackgroundColor ) ;
			area [ 1 , 0 ] = new ConsoleChar (
											Control . CheckableChar . GetStateChar ( Control . State ) ,
											Control . ActualForegroundColor ,
											Control . ActualBackgroundColor ) ;
			area [ 2 , 0 ] = new ConsoleChar (
											']' ,
											Control . ActualForegroundColor ,
											Control . ActualBackgroundColor ) ;

			if ( Control . ActualHeight == 1 )
			{
				for ( int x = 0 ; x < Control . ActualWidth - 4 ; x++ )
				{
					area [ x + 3 , 0 ] = new ConsoleChar (
														Control . Text [ x ] ,
														Control . ActualForegroundColor ,
														Control . ActualBackgroundColor ) ;
				}
			}
			else
			{
				for ( int x = 0 ; x < Control . ActualWidth - 4 ; x++ )
				{
					area [ x + 3 , 0 ] = new ConsoleChar (
														Control . Text [ x ] ,
														Control . ActualForegroundColor ,
														Control . ActualBackgroundColor ) ;
				}

				for ( int y = 1 ; y < Control . ActualWidth ; y++ )
				{
					for ( int x = 0 ; x < Control . ActualWidth ; x++ )
					{
						area [ x , 0 ] = new ConsoleChar (
														Control . Text [ x ] ,
														Control . ActualForegroundColor ,
														Control . ActualBackgroundColor ) ;
					}
				}
			}
		}

	}

}
