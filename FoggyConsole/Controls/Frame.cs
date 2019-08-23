using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . FoggyConsole . Controls
{

	public class Frame : ContentControl
	{

		public override bool CanFocusedOn => false ;

		public Page CurrentPage { get ; private set ; }

		public override Control Content { get => CurrentPage ; set => throw new InvalidOperationException ( ) ; }

		public void NavigateTo ( Page page )
		{
			if ( page == null )
			{
				throw new ArgumentNullException ( nameof ( page ) ) ;
			}

			if ( CurrentPage != page )
			{
				CurrentPage             = page ;
				CurrentPage . Container = this ;
				CurrentPage . OnNavigateTo ( ) ;
			}
		}

	}

}
