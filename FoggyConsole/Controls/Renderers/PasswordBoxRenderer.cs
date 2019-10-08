using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . FoggyConsole . Controls . Renderers
{

	public class PasswordBoxRenderer : ControlRenderer <PasswordBox>
	{

		public override void DrawOverride ( Application application , ConsoleArea area )
		{

            if (string.IsNullOrEmpty( Control.Text))
            {
                
            }

			for ( int y = 0 ; y < Control . ContentHeight && y * Control . ContentHeight < Control . Text . Length ; y++ )
			{
				for ( int x = 0 ; x < Control . ContentWidth ; x++ )
				{
					if ( x + y * Control . ContentHeight < Control . Text . Length )
					{
						area [ x , y ] = new ConsoleChar (
														  Control . PasswordChar ,
														  Control . ActualForegroundColor ,
														  Control . ActualBackgroundColor ) ;
					}
					else
					{
						break ;
					}
				}
			}
		}

	}

}
