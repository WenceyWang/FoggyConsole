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

		private char _progressChar = '|' ;

       // private char 

		private int _value ;

		public int SmallChange { get ; set ; }

		public int LargeChange { get ; set ; }


		/// <summary>
		///     The character which is used to draw the bar
		/// </summary>
		public char ProgressChar
		{
			get => _progressChar ;
			set
			{
				if ( value != _minValue )
				{
					_progressChar = value ;
					RequestRedraw ( ) ;
				}
			}
		}

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
					_minValue = value ;
					RequestRedraw ( ) ;
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
					_maxValue = value ;
					RequestRedraw ( ) ;
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
				if ( value    < MinValue
					 || value > MaxValue )
				{
					throw new ArgumentOutOfRangeException ( nameof ( value ) ) ;
				}

				if ( _value != value )
				{
					_value = value ;
					ValueChanged ? . Invoke ( this , EventArgs . Empty ) ;
					RequestRedraw ( ) ;
				}
			}
		}

		public override bool CanFocusedOn => false ;

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
		public ProgressBar ( IControlRenderer renderer = null ) : base ( renderer ?? new ProgressBarRenderer ( ) ) { }

		public ProgressBar ( ) : this ( null ) { }

		/// <summary>
		///     Fired if the Value-Property has changed
		/// </summary>
		public event EventHandler ValueChanged ;

	}

}
