using System ;
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
	public abstract class ContainerBase : Control
	{

		public abstract IReadOnlyCollection <Control> Children { get ; }

		public ContainerBase ( IControlRenderer renderer ) : base ( renderer ) { }

		public T Find <T> ( [NotNull] string name ) where T : Control => Find ( name ) as T ;

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
					if ( control is ContainerBase container )
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
				if ( control is ContainerBase container )
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
