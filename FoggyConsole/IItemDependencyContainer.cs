using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . FoggyConsole . Controls ;

namespace DreamRecorder . FoggyConsole
{

	public interface IItemDependencyContainer <T>
	{

		T this [ Control control ] { get ; set ; }

	}

}
