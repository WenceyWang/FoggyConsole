﻿using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using WenceyWang . FoggyConsole . Controls . Events ;
using WenceyWang . FoggyConsole . Controls . Renderers ;

namespace WenceyWang . FoggyConsole . Controls
{

	/// <summary>
	///     A very basic
	///     <code>Container</code>
	///     It has no appearance, controls within it are aligned using thier top and left values.
	/// </summary>
	public class Canvas : ItemsContainer
	{

		public override bool CanFocus => false ;

		public Dictionary <Control , Point> Position { get ; } = new Dictionary <Control , Point> ( ) ;

		public Point this [ Control control ]
		{
			get
			{
				if ( Position . ContainsKey ( control ) )
				{
					return Position [ control ] ;
				}
				else
				{
					return default ;
				}
			}
			set => Position [ control ] = value ;
		}

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
		public Canvas ( IControlRenderer renderer = null ) : base ( renderer ?? new CanvasRenderer ( ) )
		{
			ItemsAdded   += Canvas_ItemsAdded ;
			ItemsRemoved += Canvas_ItemsRemoved ;
		}

		private void Canvas_ItemsRemoved ( object sender , ContainerControlEventArgs e )
		{
			Position . Remove ( e . Control ) ;
		}

		private void Canvas_ItemsAdded ( object sender , ContainerControlEventArgs e )
		{
			this [ e . Control ] = new Point ( ) ;
		}


		public override void Arrange ( Rectangle finalRect )
		{
			foreach ( Control control in Items )
			{
				control . Arrange (
									new Rectangle (
													finalRect . LeftTopPoint . Offset (
																						new Vector (
																									Position
																										[
																										control ] ) ) ,
													control . DesiredSize ) ) ;
			}

			base . Arrange ( finalRect ) ;
		}

		public override void Measure ( Size availableSize )
		{
			foreach ( Control control in Items )
			{
				control . Measure ( availableSize ) ;
			}

			Rectangle rectangle = new Rectangle ( ) ;

			foreach ( Control control in Items )
			{
				rectangle = rectangle . Union ( new Rectangle ( this [ control ] , control . DesiredSize ) ) ;
			}

			DesiredSize = rectangle . Size ;
		}

	}

}