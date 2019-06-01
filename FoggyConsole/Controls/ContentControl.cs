using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Collections . ObjectModel ;
using System . Linq ;

using JetBrains . Annotations ;

using WenceyWang . FoggyConsole . Controls . Renderers ;

namespace WenceyWang . FoggyConsole . Controls
{

	public abstract class ContentControl : Container
	{

		[CanBeNull] private Control _content ;


		[CanBeNull]
		public virtual Control Content
		{
			get => _content ;
			set
			{
				_content = value ;
				if ( _content != null )
				{
					_content . Container = this ;
				}
			}
		}

		public override IReadOnlyCollection <Control> Children
		{
			get
			{
				if ( Content is null )
				{
					return new ReadOnlyCollection <Control> ( new Control [ ] { } ) ;
				}
				else
				{
					return new ReadOnlyCollection <Control> ( new [ ] { Content } ) ;
				}
			}
		}

		protected ContentControl ( IControlRenderer renderer ) : base ( renderer ) { }

	}

}
