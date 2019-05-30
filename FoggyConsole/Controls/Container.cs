using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using JetBrains.Annotations;
using WenceyWang . FoggyConsole . Controls . Renderers ;

namespace WenceyWang . FoggyConsole . Controls
{

	/// <summary>
	///     A
	///     <code>Control</code>
	///     which can contain other Controls.
	/// </summary>
	public abstract class Container : Control
	{

		public abstract IList <Control> Children { get ; }

		public Container ( IControlRenderer renderer = null ) : base ( renderer ) { }

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
