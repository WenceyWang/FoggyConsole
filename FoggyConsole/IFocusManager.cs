using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . FoggyConsole . Controls ;

namespace DreamRecorder . FoggyConsole
{

	/// <summary>
	///     Represents a class which can handle focus changed. The standard implementation is
	///     <code>FocusManager</code>
	/// </summary>
	public interface IFocusManager : IHandleKeyInput , IApplicationItem
	{

		/// <summary>
		///     The currently focused control
		/// </summary>
		Control FocusedControl { get ; }

	}

}
