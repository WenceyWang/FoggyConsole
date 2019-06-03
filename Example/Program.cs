using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Net ;

using DreamRecorder . FoggyConsole ;
using DreamRecorder . FoggyConsole . Controls ;
using DreamRecorder . ToolBox . CommandLine ;

using Microsoft . Extensions . Logging ;

using WenceyWang . FIGlet ;

namespace Example
{

	public class Program : ApplicationBase <Program , ProgramExitCode , ProgramSetting , ProgramSettingCatalog>
	{

		public override string License => "AGPL" ;

		public override bool LoadSetting => true ;

		public override bool AutoSaveSetting => true ;

		public Program ( ) => Name = "GuguguCalendar" ;


		public static void Main ( string [ ] args ) { new Program ( ) . RunMain ( args ) ; }

		public override void ConfigureLogger ( ILoggingBuilder builder )
		{
			builder . AddFilter ( level => true ) . AddDebug ( ) ;
		}

		public override void ShowLogo ( )
		{
			Console . WriteLine (
								new AsciiArt (
											"Foggy Console Example" ,
											width : CharacterWidth . Smush ) . ToString ( ) ) ;
		}

		public override void ShowCopyright ( ) { Console . WriteLine ( "Copyright" ) ; }


		public override void Start ( string [ ] args ) { base . Start ( args ) ; }


		public override Frame PrepareViewRoot ( )
		{
			ViewRoot = new Frame ( ) ;
			StackPanel panel = new StackPanel ( ) ;
			Page       page  = new Page ( ) ;
			page . Content = panel ;
			ViewRoot . NavigateTo ( page ) ;

			Button buttonA = new Button { Name = "buttonA" , Text = "A" , KeyBind = 'A' } ;
			panel . Items . Add ( buttonA ) ;
			Button buttonB = new Button { Name = "buttonB" , Text = "B" , KeyBind = 'B' } ;
			panel . Items . Add ( buttonB ) ;

			panel [ buttonB ] = ContentAlign . Right ;

			Button buttonExit = new Button
								{
									Name            = "buttonExit" ,
									Text            = "Exit" ,
									KeyBind         = 'E' ,
									BoarderStyle    = LineStyle . SingleLinesSet ,
									AllowSingleLine = false
								} ;
			panel . Items . Add ( buttonExit ) ;

			WebClient client = new WebClient ( ) ;


			FIGletLabel label = new FIGletLabel { Text = 24 . ToString ( ) , CharacterWidth = CharacterWidth . Smush } ;

			panel . Items . Add ( label ) ;

			panel[label] = ContentAlign.Center;

            buttonExit. Pressed += ButtonExit_Pressed ;

			return ViewRoot ;
		}

		private void ButtonExit_Pressed ( object sender , EventArgs e ) { Exit ( ProgramExitCode . Success ) ; }

	}

	public class ProgramSetting : SettingBase <ProgramSetting , ProgramSettingCatalog>
	{

		[SettingItem ( ( int ) ProgramSettingCatalog . General , nameof ( BotToken ) , "" , true , "" )]
		public string BotToken { get ; set ; }

		[SettingItem ( ( int ) ProgramSettingCatalog . General , nameof ( DatabaseConnection ) , "" , true , "" )]
		public string DatabaseConnection { get ; set ; }

		[SettingItem ( ( int ) ProgramSettingCatalog . General , nameof ( HttpProxy ) , "" , true , null )]
		public string HttpProxy { get ; set ; }

	}

	public enum ProgramSettingCatalog
	{

		General = 0

	}

	public class ProgramExitCode : ProgramExitCode <ProgramExitCode>
	{

	}

}
