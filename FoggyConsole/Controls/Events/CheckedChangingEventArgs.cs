using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . FoggyConsole . Controls . Events
{

	/// <summary>
	///     Contains information about an occuring status-change of a
	///     <code>CheckBox</code>
	/// </summary>
	public class CheckedChangingEventArgs : EventArgs
	{

		/// <summary>
		///     The state the CheckBox is going to have
		/// </summary>
		public CheckState State { get ; }

		/// <summary>
		///     True if the change should be canceled, otherwise false
		/// </summary>
		public bool Cancel { get ; set ; }

		/// <summary>
		///     Creates a new
		///     <code>CheckboxCheckedChangingEventArg</code>
		/// </summary>
		/// <param name="state">The state the CheckBox is going to have</param>
		public CheckedChangingEventArgs ( CheckState state )
		{
			State  = state ;
			Cancel = false ;
		}

	}

}
