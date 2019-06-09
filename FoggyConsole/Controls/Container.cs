﻿using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . FoggyConsole . Controls . Renderers ;

using JetBrains . Annotations ;

namespace DreamRecorder . FoggyConsole . Controls
{

	/// <summary>
	///     A
	///     <code>Control</code>
	///     which can contain other Controls.
	/// </summary>
	public abstract class Container : Control
	{

		public abstract IReadOnlyCollection <Control> Children { get ; }

		public Container ( IControlRenderer renderer ) : base ( renderer ) { }

		public Control Find ( [NotNull] string name )
		{
			if ( name == null )
			{
				throw new ArgumentNullException ( nameof ( name ) ) ;
			}

			foreach ( Control control in Children )
			{
				if ( control . Name == name )
				{
					return control ;
				}
				else
				{
					if ( control is Container container )
					{
						Control result = container . Find ( name ) ;

						if ( result != null )
						{
							return result ;
						}
					}
				}
			}

			return null ;
		}

		public List <Control> GetAllItem ( )
		{
			List <Control> controlList = new List <Control> { this } ;
			foreach ( Control control in Children )
			{
				if ( control is Container container )
				{
					controlList . AddRange ( container . GetAllItem ( ) ) ;
				}
				else
				{
					controlList . Add ( control ) ;
				}
			}

			return controlList ;
		}

	}

}
