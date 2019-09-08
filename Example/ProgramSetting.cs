using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . ToolBox . CommandLine ;

namespace DreamRecorder . FoggyConsole . Example
{

	public class ProgramSetting : SettingBase <ProgramSetting , ProgramSettingCatalog>
	{

		[SettingItem (
			( int ) ProgramSettingCatalog . General ,
			nameof ( PortNumber ) ,
			"Tcp port to listen to." ,
			true ,
			22 )]
		public int PortNumber { get ; set ; }

	}

}
