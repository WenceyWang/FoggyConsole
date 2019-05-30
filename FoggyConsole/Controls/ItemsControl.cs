using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using JetBrains.Annotations;
using WenceyWang . FoggyConsole . Controls . Renderers ;

namespace WenceyWang . FoggyConsole . Controls
{

	public abstract class ItemsControl : Container
	{

		public abstract IList <Control> Items { get ; }

		public override IList <Control> Children => Items ;

		protected ItemsControl ( IControlRenderer renderer ) : base ( renderer ) { }

        public virtual void AddChild([NotNull] Control control)
        {
            if (control == null) throw new ArgumentNullException(nameof(control));
            Children.Add(control);
            control.Container = this;
        }

    }

}