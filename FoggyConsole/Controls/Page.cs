using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Reflection ;
using System . Xml . Linq ;

namespace WenceyWang . FoggyConsole . Controls
{

	public class Page : ContentControl
	{

		public override bool CanFocus => false ;

		public Frame Frame => Container as Frame ;

		public Page ( ) : base ( new PageRenderer ( ) ) { }

		protected Page ( XElement page ) : this ( )
		{
			if ( page == null )
			{
				throw new ArgumentNullException ( nameof ( page ) ) ;
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
				property . SetValue (
									currentControl ,
									Convert . ChangeType ( attribute . Value , property . PropertyType ) ) ;
			}

			if ( currentControl is ItemsContainer itemsContainer )
			{
				foreach ( XElement child in control . Elements ( ) )
				{
					Control childControl = CrateControl ( child ) ;

					itemsContainer . Items . Add ( childControl ) ;
				}
			}
			else if ( currentControl is ContentControl contentControl )
			{
				XElement child = control . Elements ( ) . FirstOrDefault ( ) ;
				if ( ! ( child is null ) )
				{
					Control childControl = CrateControl ( child ) ;

					contentControl . Content = childControl ;
				}
			}

			return currentControl ;
		}

	}

}
