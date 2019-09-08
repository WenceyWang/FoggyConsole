using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Collections . ObjectModel ;
using System . Linq ;

using DreamRecorder . FoggyConsole . Controls . Renderers ;

using JetBrains . Annotations ;

namespace DreamRecorder . FoggyConsole . Controls
{

	public abstract class ContentControl : ContainerBase
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

				return new ReadOnlyCollection <Control> ( new [ ] { Content } ) ;
			}
		}


		protected ContentControl ( IControlRenderer renderer = null ) : base (
																			  renderer
																			  ?? new ContentControlRenderer ( ) )
		{
		}

		protected ContentControl ( ) : this ( null ) { }

		public override void Arrange ( Rectangle ? finalRect )
		{
			if ( finalRect . IsNotEmpty ( ) )
			{
				ArrangeOverride ( finalRect . Value ) ;
			}
			else
			{
				Content ? . Arrange ( null ) ;
			}
		}

		public override void Measure ( Size availableSize )
		{
			Content ? . Measure ( availableSize ) ;
			DesiredSize = Content ? . DesiredSize ?? Size.Empty ;
		}

		public override void ArrangeOverride ( Rectangle finalRect )
		{
			Content ? . Arrange ( finalRect ) ;
			base . ArrangeOverride ( finalRect ) ;
		}

	}

}
