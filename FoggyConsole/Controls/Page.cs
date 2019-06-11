using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Reflection ;
using System . Xml . Linq ;

using DreamRecorder . FoggyConsole . Controls . Renderers ;

using JetBrains . Annotations ;

namespace DreamRecorder . FoggyConsole . Controls
{

	/// <summary>
	///     Page should be inherited but currently no and we think we should be able to create raw Page
	/// </summary>
	[PublicAPI]
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


		public Control CrateControl ( XElement control )
		{
			List <TypeInfo> controlTypes = AppDomain . CurrentDomain . GetAssemblies ( ) .
														SelectMany (
																	assembly
																		=> assembly . DefinedTypes . Where (
																											type
																												=> type .
																													IsSubclassOf (
																																typeof
																																(
																																	Control
																																) ) ) ) .
														ToList ( ) ;

			Type controlType = controlTypes . FirstOrDefault ( type => type . Name == control . Name ) ? . AsType ( )
								?? controlTypes . FirstOrDefault (
																type
																	=> type . Name
																		== typeof ( Page ) . Namespace
																		+ "."
																		+ control . Name ) ;

			if ( controlType == null )
			{
				throw new ArgumentException ( $"Cannot find type {control . Name}" , nameof ( control ) ) ;
			}

			Control currentControl = ( Control ) Activator . CreateInstance ( controlType ) ;
			foreach ( XAttribute attribute in control . Attributes ( ) )
			{
				PropertyInfo property = controlType . GetProperty (
																	attribute . Name . LocalName ,
																	BindingFlags . Instance
																	| BindingFlags . IgnoreCase
																	| BindingFlags . NonPublic
																	| BindingFlags . Public
																	| BindingFlags . SetProperty ) ;

				if ( property != null )
				{
					object value = Convert . ChangeType ( attribute . Value , property . PropertyType ) ;

					property . SetValue ( currentControl , value ) ;
				}
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
