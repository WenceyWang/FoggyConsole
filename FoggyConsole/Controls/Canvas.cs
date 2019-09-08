using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . FoggyConsole . Controls . Events ;
using DreamRecorder . FoggyConsole . Controls . Renderers ;

namespace DreamRecorder . FoggyConsole . Controls
{

	/// <summary>
	///     A very basic
	///     <code>ContainerBase</code>
	///     It has no appearance, controls within it are aligned using their top and left values.
	/// </summary>
	public class Canvas : ItemsContainer , IItemDependencyContainer <Point>
	{

		public override bool CanFocusedOn => false ;

		protected Dictionary <Control , Point> Position { get ; } = new Dictionary <Control , Point> ( ) ;

		/// <summary>
		///     Creates a new
		///     <code>Canvas</code>
		/// </summary>
		/// <param name="renderer">
		///     The
		///     <code>ControlRenderer</code>
		///     to use. If null a new instance of
		///     <code>PanelRenderer</code>
		///     will be used.
		/// </param>
		/// <exception cref="ArgumentException">
		///     Thrown if the
		///     <code>ControlRenderer</code>
		///     which should be set already has an other
		///     Control assigned
		/// </exception>
		public Canvas ( IControlRenderer renderer = null ) : base ( renderer )
		{
			ItemsAdded   += Canvas_ItemsAdded ;
			ItemsRemoved += Canvas_ItemsRemoved ;
		}

		public Canvas ( ) : this ( null ) { }

		public Point this [ Control control ]
		{
			get
			{
				if ( Position . ContainsKey ( control ) )
				{
					return Position [ control ] ;
				}

				return default ;
			}
			set => Position [ control ] = value ;
		}


		private void Canvas_ItemsRemoved ( object sender , ContainerControlEventArgs e )
		{
			Position . Remove ( e . Control ) ;
		}

		private void Canvas_ItemsAdded ( object sender , ContainerControlEventArgs e )
		{
			this [ e . Control ] = new Point ( ) ;
		}


		public override void ArrangeOverride ( Rectangle finalRect )
		{
			foreach ( Control control in Items )
			{
				Rectangle result = new Rectangle (
												  finalRect . LeftTopPoint . Offset (
																					 new Vector (
																								 Position
																									 [ control ] ) ) ,
												  control . DesiredSize ) ;


				control . Arrange ( result . Intersect ( finalRect ) ) ;
			}


			base . ArrangeOverride ( finalRect ) ;
		}

		public override void Measure ( Size availableSize )
		{
			foreach ( Control control in Items )
			{
				control . Measure ( availableSize ) ;
			}

			Rectangle rectangle = Rectangle . Empty ;

			foreach ( Control control in Items )
			{
				rectangle = rectangle . Union ( new Rectangle ( this [ control ] , control . DesiredSize ) ) ;
			}

			int resultWidth = rectangle . Size . Width ;

			if ( ! AutoWidth )
			{
				resultWidth = Math . Max ( resultWidth , Width ) ;
			}

			int resultHeight = rectangle . Size . Height ;

			if ( ! AutoHeight )
			{
				resultHeight = Math . Max ( resultHeight , Height ) ;
			}

			DesiredSize = new Size ( resultWidth , resultHeight ) ;
		}

	}

}
