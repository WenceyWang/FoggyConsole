using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . FoggyConsole . Controls . Renderers ;

namespace DreamRecorder . FoggyConsole . Controls
{

	/// <summary>
	///     A Control which is able to display the progress of an ongoing task
	/// </summary>
	public class ProgressBar : Control
	{

		private int _maxValue = 100 ;

		private int _minValue ;

		private int _value ;

		public int SmallChange { get ; set ; }

		public int LargeChange { get ; set ; }


		public int MinValue
		{
			get => _minValue ;
			set
			{
				if ( value > MaxValue )
				{
					throw new ArgumentOutOfRangeException ( nameof ( value ) ) ;
				}

				if ( value != _minValue )
				{
					Draw ( ) ;
					_minValue = value ;
				}
			}
		}

		public int MaxValue
		{
			get => _maxValue ;
			set
			{
				if ( value > MaxValue )
				{
					throw new ArgumentOutOfRangeException ( nameof ( value ) ) ;
				}

				if ( value != _minValue )
				{
					Draw ( ) ;
					_maxValue = value ;
				}
			}
		}

		/// <summary>
		///     The progress which is shown, 0 is no progress, 100 is finished
		/// </summary>
		public int Value
		{
			get => _value ;
			set
			{
				if ( value   < MinValue
					|| value > MaxValue )
				{
					throw new ArgumentOutOfRangeException ( nameof ( value ) ) ;
				}

				if ( _value != value )
				{
					ValueChanged ? . Invoke ( this , EventArgs . Empty ) ;
					Draw ( ) ;
					_value = value ;
				}
			}
		}

		public override bool CanFocus => false ;

		/// <summary>
		///     Creates a new ProgressBar
		/// </summary>
		/// <param name="renderer">
		///     The
		///     <code>ControlRenderer</code>
		///     to use. If null a new instance of
		///     <code>ProgressbarDrawer</code>
		///     will be used.
		/// </param>
		/// <exception cref="ArgumentException">
		///     Thrown if the
		///     <code>ControlRenderer</code>
		///     which should be set already has an other
		///     Control assigned
		/// </exception>
		public ProgressBar ( ControlRenderer <ProgressBar> renderer = null ) : base ( renderer )
		{
			if ( renderer == null )
			{
				Renderer = new ProgressBarRenderer ( this ) ;
			}

			Height = 1 ;
		}


		/// <summary>
		///     Fired if the Value-Property has changed
		/// </summary>
		public event EventHandler ValueChanged ;

		/// <summary>
		///     Fires the ValueChanged-event and requests an redraw
		/// </summary>
		private void OnValueChanged ( ) { }

	}

}
