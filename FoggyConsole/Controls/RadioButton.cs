using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . FoggyConsole . Controls . Events ;
using DreamRecorder . FoggyConsole . Controls . Renderers ;

namespace DreamRecorder . FoggyConsole . Controls
{

	/// <summary>
	///     A checkbox-like control of which only one in a group can be checked at at the same time
	/// </summary>
	public class RadioButton : CheckableBase
	{

		/// <summary>
		///     The group this RadioButton belongs to
		/// </summary>
		public string ComboBoxGroup { get ; set ; }

		public override bool CanFocus => Enabled ;

		/// <summary>
		///     Creates a new RadioButton
		/// </summary>
		/// <param name="text">The text to display</param>
		/// <param name="renderer">
		///     The
		///     <code>ControlRenderer</code>
		///     to use. If null a new instance of
		///     <code>RadioButtonRenderer</code>
		///     will be used.
		/// </param>
		/// <exception cref="ArgumentException">
		///     Thrown if the
		///     <code>ControlRenderer</code>
		///     which should be set already has an other
		///     Control assigned
		/// </exception>
		public RadioButton ( ControlRenderer <RadioButton> renderer = null ) : base (
																					renderer
																					?? new RadioButtonRenderer ( ) )
		{
			State           =  CheckState . Unchecked ;
			CheckedChanging += OnCheckedChanging ;
		}

		private void OnCheckedChanging ( object sender , CheckedChangingEventArgs checkedChangingEventArgs )
		{
			IEnumerable <RadioButton> radioButtons =
				( Page . Container as ItemsContainer ) ? . Items ? . OfType <RadioButton> ( ) ? .
															Where ( cb => cb . ComboBoxGroup == ComboBoxGroup ) ;
			if ( radioButtons != null )
			{
				foreach ( RadioButton radioButton in radioButtons )
				{
					if ( radioButton           != this
						&& radioButton . State != CheckState . Unchecked )
					{
						radioButton . State = CheckState . Unchecked ;
					}
				}
			}
		}

	}

}
