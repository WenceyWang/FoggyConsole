using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace WenceyWang . FoggyConsole . Controls . Events
{

	/// <summary>
	///     Used by
	///     <code>Container</code>
	///     in ControlAdded and ControlRemoved events
	/// </summary>
	public class ContainerControlEventArgs : EventArgs
	{

		/// <summary>
		///     The control which has been added or removed
		/// </summary>
		public Control Control { get ; }

		/// <summary>
		///     Creates a new
		///     <code>ContainerControlEventArgs</code>
		/// </summary>
		/// <param name="control">The control which has been added or removed</param>
		public ContainerControlEventArgs ( Control control ) { Control = control ; }

	}

}
