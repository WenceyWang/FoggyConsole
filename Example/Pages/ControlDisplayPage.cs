using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Xml . Linq ;

using DreamRecorder . FoggyConsole . Controls ;
using DreamRecorder . ToolBox . General ;

namespace DreamRecorder . FoggyConsole . Example . Pages
{

	public sealed class ControlDisplayPage : Page
	{

		public ControlDisplayPage ( ) : base (
											  XDocument . Parse (
																 typeof ( ControlDisplayPage ) . GetResourceFile (
																												  $"{nameof ( ControlDisplayPage )}.xml" ) ) .
														  Root )
		{
		}


		public override void OnNavigateTo ( ) { base . OnNavigateTo ( ) ; }

	}

}
