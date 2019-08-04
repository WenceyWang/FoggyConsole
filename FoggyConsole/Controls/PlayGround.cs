using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . FoggyConsole . Controls . Renderers ;

namespace DreamRecorder . FoggyConsole . Controls
{

	// TODO: don't redraw the whole thing all the time
	// TODO: add methods for easy from-drawing
	// TODO: entities?
	// TODO: make Height and Width changeable

	/// <summary>
	///     A class which draws the contents of an two-dimensional char-array.
	///     This can be used to draw a small game or display graphs.
	/// </summary>
	public class PlayGround : Control
	{

		private ConsoleArea _buffer ;

		public ConsoleArea Buffer
		{
			get => _buffer ;
			set
			{
				if ( _buffer != value )
				{
					_buffer = value ;
					RequestMeasure ( ) ;
				}
			}
		}

		/// <summary>
		///     Gets or sets the char at (
		///     <paramref name="top" />
		///     |
		///     <paramref name="left" />
		///     ).
		///     Setting triggers an redraw if
		///     <code>Playground.AutoRedraw</code>
		///     is true.
		/// </summary>
		/// <param name="top"></param>
		/// <param name="left"></param>
		/// <returns></returns>
		/// <seealso cref="AutoRedraw" />
		public ConsoleChar this [ int top , int left ]
		{
			get => Buffer [ top , left ] ;
			set
			{
				if ( Buffer [ top , left ] != value )
				{
					Buffer [ top , left ] = value ;

					if ( AutoRedraw )
					{
						RequestRedraw ( ) ;
					}
				}
			}
		}

		/// <summary>
		///     True if a redraw should be triggered on every change
		/// </summary>
		public bool AutoRedraw { get ; set ; }

		public override bool CanFocusedOn => false ;

		/// <summary>
		///     Creates a new
		///     <code>Playground</code>
		/// </summary>
		/// <param name="renderer">
		///     The
		///     <code>ControlRenderer</code>
		///     to use. If null a new instance of
		///     <code>PlayGroundRenderer</code>
		///     will be used.
		/// </param>
		/// <exception cref="ArgumentException">
		///     Thrown if the
		///     <code>ControlRenderer</code>
		///     which should be set already has an other
		///     Control assigned
		/// </exception>
		public PlayGround ( IControlRenderer renderer = null ) : base (
																		renderer
																		?? new
																			PlayGroundRenderer ( ) )
			=> AutoRedraw = true ;

		public PlayGround ( ) : this ( null ) { }

	}

}
