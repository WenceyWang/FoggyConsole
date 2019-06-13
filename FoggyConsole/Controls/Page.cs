using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Reflection ;
using System . Xml . Linq ;

using DreamRecorder . FoggyConsole . Controls . Renderers ;
using DreamRecorder . ToolBox . General ;

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

			Content = CrateControl ( page , this ) ;
		}

		public virtual void OnNavigateTo ( ) { }

		public virtual void OnNavigateOut ( ) { }

		public Control CrateControl ( XElement control , [CanBeNull] ContainerBase container = null )
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
				string propertyName = attribute . Name . LocalName ;

				if ( propertyName == ( "Container.Item" ) )
				{
					if ( container != null )
					{
						PropertyInfo property =
							container . GetType ( ) .
										GetProperty (
													propertyName ,
													BindingFlags . Instance
													| BindingFlags . IgnoreCase
													| BindingFlags . NonPublic
													| BindingFlags . Public
													| BindingFlags . SetProperty ) ;

						if ( property != null )
						{
							property . SetValue (
												currentControl ,
												attribute . Value . ParseTo ( property . PropertyType ) ,
												new object [ ] { currentControl } ) ;
						}
					}
				}
				else
				{
					PropertyInfo property = controlType . GetProperty (
																		propertyName ,
																		BindingFlags . Instance
																		| BindingFlags . IgnoreCase
																		| BindingFlags . NonPublic
																		| BindingFlags . Public
																		| BindingFlags . SetProperty ) ;

					if ( property != null )
					{
						property . SetValue (
											currentControl ,
											attribute . Value . ParseTo ( property . PropertyType ) ) ;
					}
				}
			}

			if ( container is ItemsContainer itemsContainer )
			{
				itemsContainer . Items . Add ( currentControl ) ;
			}
			else if ( container is ContentControl contentControl )
			{
				contentControl . Content = currentControl ;
			}


			if ( currentControl is ItemsContainer currentItemsContainer )
			{
				foreach ( XElement child in control . Elements ( ) )
				{
					CrateControl ( child , currentItemsContainer ) ;
				}
			}
			else if ( currentControl is ContentControl currentContentControl )
			{
				XElement child = control . Elements ( ) . FirstOrDefault ( ) ;
				if ( ! ( child is null ) )
				{
					CrateControl ( child , currentContentControl ) ;
				}
			}

			return currentControl ;
		}

	}

}
