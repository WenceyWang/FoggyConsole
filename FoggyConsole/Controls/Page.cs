using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Reflection ;
using System . Xml . Linq ;

using JetBrains . Annotations ;

namespace WenceyWang . FoggyConsole . Controls
{

	public class Page : ContentControl
	{

		[CanBeNull] private Control _content ;

		public override bool CanFocus => false ;

		[CanBeNull]
		public override Control Content
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

		public Frame Frame => Container as Frame ;

		public Page ( ) : base ( new PageRenderer ( ) ) { }

		protected Page ( XElement page ) : this ( )
		{
			if ( page == null )
			{
				throw new ArgumentNullException ( nameof(page) ) ;
			}

			Content = CrateControl ( page . Elements ( ) . Single ( ) ) ;
		}

		public virtual void OnNavigateTo ( ) { }

		public virtual void OnNavigateOut ( ) { }

		public override void Measure ( Size availableSize )
		{
			Content ? . Measure ( availableSize ) ;
			DesiredSize = Content ? . DesiredSize ?? availableSize ;
		}

		public override void Arrange ( Rectangle finalRect )
		{
			Content ? . Arrange ( finalRect ) ;
			base . Arrange ( finalRect ) ;
		}

		public Control CrateControl ( XElement control )
		{
			Type controlType = Type . GetType ( typeof ( Page ) . Namespace + "." + control . Name ) ;

			if ( controlType == null )
			{
				throw new ArgumentException ( ) ;
			}

			Control currentControl = ( Control ) Activator . CreateInstance ( controlType ) ;
			foreach ( XAttribute attribute in control . Attributes ( ) )
			{
				PropertyInfo property = controlType . GetTypeInfo ( ) . GetProperty ( attribute . Name . LocalName ) ;
				property . SetValue ( currentControl , Convert . ChangeType ( attribute . Value , property . PropertyType ) ) ;
			}

			if ( currentControl is Container container )
			{
				foreach ( XElement child in control . Elements ( ) )
				{
					Control childControl = CrateControl ( child ) ;

					container . Children . Add ( childControl ) ;
				}
			}

			return currentControl ;
		}

	}

}
