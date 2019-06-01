using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace WenceyWang . FoggyConsole . Controls . Renderers
{

	/// <summary>
	///     Draws a
	///     <code>CheckBox</code>
	///     -Control
	/// </summary>
	public class CheckboxRenderer : ControlRenderer <CheckBox>
	{

		private CheckableChar _checkableChar ;

		public CheckableChar CheckableChar
		{
			get => _checkableChar ;
			set
			{
				if ( _checkableChar != value )
				{
					_checkableChar = value ;
					Draw ( ) ;
				}
			}
		}

		public override void Draw ( )
		{
			ConsoleArea result = new ConsoleArea ( Control . ActualSize , Control . ActualBackgroundColor ) ;

			result [ 0 , 0 ] = new ConsoleChar (
												'[' ,
												Control . ActualForegroundColor ,
												Control . ActualBackgroundColor ) ;
			result [ 1 , 0 ] = new ConsoleChar (
												CheckableChar . GetStateChar ( Control . State ) ,
												Control . ActualForegroundColor ,
												Control . ActualBackgroundColor ) ;
			result [ 2 , 0 ] = new ConsoleChar (
												']' ,
												Control . ActualForegroundColor ,
												Control . ActualBackgroundColor ) ;

			if ( Control . ActualHeight == 1 )
			{
				for ( int x = 0 ; x < Control . ActualWidth - 4 ; x++ )
				{
					result [ x + 3 , 0 ] = new ConsoleChar (
															Control . Text [ x ] ,
															Control . ActualForegroundColor ,
															Control . ActualBackgroundColor ) ;
				}
			}
			else
			{
				for ( int x = 0 ; x < Control . ActualWidth - 4 ; x++ )
				{
					result [ x + 3 , 0 ] = new ConsoleChar (
															Control . Text [ x ] ,
															Control . ActualForegroundColor ,
															Control . ActualBackgroundColor ) ;
				}

				for ( int y = 1 ; y < Control . ActualWidth ; y++ )
				{
					for ( int x = 0 ; x < Control . ActualWidth ; x++ )
					{
						result [ x , 0 ] = new ConsoleChar (
															Control . Text [ x ] ,
															Control . ActualForegroundColor ,
															Control . ActualBackgroundColor ) ;
					}
				}
			}

			FogConsole . Draw ( Control . RenderPoint , result ) ;
		}

	}

}
