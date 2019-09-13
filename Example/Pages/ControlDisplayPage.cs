using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Xml . Linq ;

using DreamRecorder . FoggyConsole . Controls ;
using DreamRecorder . ToolBox . General ;

namespace DreamRecorder . FoggyConsole . Example . Pages
{

	[AttributeUsage ( AttributeTargets . Class , Inherited = false )]
	public sealed class ControlDisplayAttribute : Attribute
	{

	}

	public sealed class NewsPage : Page
	{

		public NewsPage ( ) : base (
									XDocument . Parse (
													   typeof ( ControlDisplayPage ) . GetResourceFile (
																										$"{nameof ( NewsPage )}.xml" ) ) .
												Root )
		{
            Text = Find<TextBox>(nameof(Text));
            Bar = Find<ProgressBar>(nameof(Bar));

            Text.TextChanged += Text_TextChanged;

        }

        private void Text_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(Text.Text,out int value))
            {
                Bar.Value = value;

            }
        }

        public TextBox Text { get; private set; }
        public ProgressBar Bar { get; private set; }

        public override void OnNavigateTo ( ) { }

	}

	public sealed class ControlDisplayPage : Page
	{

		public Frame PageFrame { get ; }

		public StackPanel PageList { get ; }

		public List <Page> Pages { get ; set ; }

		public ControlDisplayPage ( ) : base (
											  XDocument . Parse (
																 typeof ( ControlDisplayPage ) . GetResourceFile (
																												  $"{nameof ( ControlDisplayPage )}.xml" ) ) .
														  Root )
		{
			PageFrame = Find <Frame> ( nameof ( PageFrame ) ) ;
			PageList  = Find <StackPanel> ( nameof ( PageList ) ) ;

			Pages = new List <Page> { new NewsPage ( ) } ;
		}

		public override void OnNavigateTo ( )
		{
			PageFrame . NavigateTo ( Pages . First ( page => page is NewsPage ) ) ;
		}

	}

}
