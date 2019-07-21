using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Collections . ObjectModel ;
using System . Collections . Specialized ;
using System . Linq ;

using DreamRecorder . FoggyConsole . Controls . Events ;
using DreamRecorder . FoggyConsole . Controls . Renderers ;

namespace DreamRecorder . FoggyConsole . Controls
{

	public abstract class ItemsContainer : ContainerBase
	{

		public override IReadOnlyCollection <Control> Children { get ; }


		public ObservableCollection <Control> Items { get ; }

		protected ItemsContainer ( IControlRenderer renderer = null ) : base (
																			renderer ?? new ItemsContainerRenderer ( ) )
		{
			Items                     =  new ObservableCollection <Control> ( ) ;
			Items . CollectionChanged += Items_CollectionChanged ;

			Children = new ReadOnlyCollection <Control> ( Items ) ;
		}

		protected ItemsContainer ( ) : this ( null ) { }

		public override void Arrange ( Rectangle ? finalRect )
		{
			if ( finalRect . IsNotEmpty ( ) )
			{
				ArrangeOverride ( finalRect . Value ) ;
			}
			else
			{
				foreach ( Control control in Items )
				{
					control . Arrange ( null ) ;
				}
			}
		}

		private void Items_CollectionChanged ( object sender , NotifyCollectionChangedEventArgs e )
		{
			if ( ! ( e . OldItems is null ) )
			{
				List <Control> removedItems = e . OldItems . Cast <Control> ( ) .
												Where ( control => ! e ? . NewItems ? . Contains ( control ) ?? true ) .
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
											Where ( control => ! e ? . OldItems ? . Contains ( control ) ?? true ) .
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
		//          control.ContainerBase = this;
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
