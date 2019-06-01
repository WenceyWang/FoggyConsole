using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using WenceyWang . FoggyConsole . Controls . Renderers ;

namespace WenceyWang . FoggyConsole . Controls
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

		private readonly ConsoleArea _field ;

		/// <summary>
		///     Gets or sets the char at (
		///     <paramref name="top" />
		///     |
		///     <paramref name="left" />
		///     ).
		///     Setting triggers an redraw if
		///     <code>Playground.AutoRedaw</code>
		///     is true.
		/// </summary>
		/// <param name="top"></param>
		/// <param name="left"></param>
		/// <returns></returns>
		/// <seealso cref="AutoRedraw" />
		public ConsoleChar this [ int top , int left ]
		{
			get => _field [ top , left ] ;
			set
			{
				if ( _field [ top , left ] != value )
				{
					_field [ top , left ] = value ;

					if ( AutoRedraw )
					{
						Draw ( ) ;
					}
				}
			}
		}

		/// <summary>
		///     True if a redraw should be triggered on every change
		/// </summary>
		/// <seealso cref="Redraw" />
		public bool AutoRedraw { get ; set ; }

		public override bool CanFocus => false ;

		/// <summary>
		///     Creates a new
		///     <code>Playground</code>
		/// </summary>
		/// <param name="height">The height of this Playground</param>
		/// <param name="width">The width of this Playground</param>
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
		public PlayGround ( int height , int width , ControlRenderer <PlayGround> renderer = null ) : base ( renderer )
		{
			if ( renderer == null )
			{
				Renderer = new PlayGroundRenderer ( this ) ;
			}

			Height = height ;
			Width  = width ;

			_field = new ConsoleArea ( new Size ( width , height ) ) ;

			AutoRedraw = true ;
		}

	}

}
