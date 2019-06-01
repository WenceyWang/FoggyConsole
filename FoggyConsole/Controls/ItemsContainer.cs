﻿using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Collections . ObjectModel ;
using System . Collections . Specialized ;
using System . Linq ;

using WenceyWang . FoggyConsole . Controls . Events ;
using WenceyWang . FoggyConsole . Controls . Renderers ;

namespace WenceyWang . FoggyConsole . Controls
{

	public abstract class ItemsContainer : Container
	{

		public override IReadOnlyCollection <Control> Children { get ; }


		public ObservableCollection <Control> Items { get ; }

		protected ItemsContainer ( IControlRenderer renderer ) : base ( renderer )
		{
			Items                     =  new ObservableCollection <Control> ( ) ;
			Items . CollectionChanged += Items_CollectionChanged ;

			Children = new ReadOnlyCollection <Control> ( Items ) ;
		}

		private void Items_CollectionChanged ( object sender , NotifyCollectionChangedEventArgs e )
		{
			if ( ! ( e . OldItems is null ) )
			{
				List <Control> removedItems = e . OldItems . Cast <Control> ( ) .
												Where (
														control => ( ! ( e ? . NewItems ? . Contains ( control ) ) )
																	?? true ) .
												ToList ( ) ;

				foreach ( Control control in removedItems )
				{
					control . Container = null ;
					ItemsRemoved ? . Invoke ( this , new ContainerControlEventArgs ( control ) ) ;
				}
			}

			if ( ! ( e . NewItems is null ) )
			{
				List <Control> newItems = e . NewItems . Cast <Control> ( ) .
											Where (
													control
														=> ( ! ( e ? . OldItems ? . Contains ( control ) ) ) ?? true ) .
											ToList ( ) ;

				foreach ( Control control in newItems )
				{
					control . Container = this ;
					ItemsAdded ? . Invoke ( this , new ContainerControlEventArgs ( control ) ) ;
				}
			}
		}

		//public virtual void AddChild([NotNull] Control control)
		//      {
		//          if (control == null)
		//	{
		//		throw new ArgumentNullException(nameof(control));
		//	}

		//	Children.Add(control);
		//          control.Container = this;
		//      }

		/// <summary>
		///     Fired if a control gets added to this container
		/// </summary>
		/// <seealso cref="ItemsRemoved" />
		public event EventHandler <ContainerControlEventArgs> ItemsAdded ;


		/// <summary>
		///     Fired if a control gets removed from this container
		/// </summary>
		/// <seealso cref="ItemsAdded" />
		public event EventHandler <ContainerControlEventArgs> ItemsRemoved ;

	}

}