﻿using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using WenceyWang . FoggyConsole . Controls . Renderers ;

namespace WenceyWang . FoggyConsole . Controls
{

	/// <summary>
	///     A
	///     <code>Container</code>
	///     which has a border.
	/// </summary>
	public class GroupBox : ItemsContainer
	{

		private string _header = string . Empty ;


		/// <summary>
		///     A description of the contents inside this GroupBox for the user
		/// </summary>
		public string Header
		{
			get => _header ;
			set
			{
				if ( value . Contains ( Environment . NewLine ) )
				{
					throw new ArgumentException (
												$"{nameof ( Header )} can't contain line feeds or carriage returns." ) ;
				}

				if ( _header != value )
				{
					_header = value ;
					Draw ( ) ;
				}
			}
		}

		public override bool CanFocus => false ;


		/// <summary>
		///     Creates a new
		///     <code>GroupBox</code>
		/// </summary>
		/// <param name="renderer">
		///     The
		///     <code>ControlRenderer</code>
		///     to use. If null a new instance of
		///     <code>GroupBoxRenderer</code>
		///     will be used.
		/// </param>
		/// <exception cref="ArgumentException">
		///     Thrown if the
		///     <code>ControlRenderer</code>
		///     which should be set already has an other
		///     Control assigned
		/// </exception>
		public GroupBox ( GroupBoxRenderer renderer = null ) : base ( renderer )
		{
			if ( renderer == null )
			{
				Renderer = new GroupBoxRenderer ( this ) ;
			}

			Header = string . Empty ;
		}

	}

}