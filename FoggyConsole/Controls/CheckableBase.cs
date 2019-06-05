using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . FoggyConsole . Controls . Events ;
using DreamRecorder . FoggyConsole . Controls . Renderers ;

namespace DreamRecorder . FoggyConsole . Controls
{

	/// <summary>
	///     Base class for
	///     <code>CheckBox</code>
	///     and
	///     <code>RadioButton</code>
	/// </summary>
	public abstract class CheckableBase : TextualBase
	{

		private CheckableChar _checkableChar ;

		private CheckState _state ;

		/// <summary>
		///     Gets or sets the state of this checkbox
		/// </summary>
		public CheckState State
		{
			get => _state ;
			set
			{
				if ( _state != value )
				{
					bool cancel = OnCheckedChanging ( value ) ;
					if ( ! cancel )
					{
						_state = value ;
						CheckedChanged ? . Invoke ( this , EventArgs . Empty ) ;
						RequestRedraw ( ) ;
					}
				}
			}
		}

		public CheckableChar CheckableChar
		{
			get => _checkableChar ;
			set
			{
				if ( _checkableChar != value )
				{
					_checkableChar = value ;
					RequestRedraw ( ) ;
				}
			}
		}

		protected CheckableBase ( IControlRenderer renderer ) : base ( renderer ) { }

		/// <summary>
		///     Fired if the State-Property is going to change
		/// </summary>
		public event EventHandler <CheckedChangingEventArgs> CheckedChanging ;

		/// <summary>
		///     Fired if the State-Property has been changed
		/// </summary>
		public event EventHandler CheckedChanged ;

		/// <summary>
		///     Fires the CheckedChanging-event and returns true if the process should be canceled
		/// </summary>
		/// <param name="state">The state the checkbox is going to have</param>
		/// <returns>True if the process should be canceled, otherwise false</returns>
		private bool OnCheckedChanging ( CheckState state )
		{
			CheckedChangingEventArgs args = new CheckedChangingEventArgs ( state ) ;
			CheckedChanging ? . Invoke ( this , args ) ;
			return args . Cancel ;
		}


		public override void KeyPressed ( KeyPressedEventArgs args )
		{
			if ( Enabled && args . KeyInfo . Key == ConsoleKey . Spacebar )
			{
				CheckState newState ;
				if ( State == CheckState . Checked )
				{
					newState = CheckState . Unchecked ;
				}
				else
				{
					newState = CheckState . Checked ;
				}

				State          = newState ;
				args . Handled = true ;
			}
		}

	}

}
