using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . FoggyConsole ;
using DreamRecorder . FoggyConsole . Controls ;
using DreamRecorder . ToolBox . CommandLine ;

using Microsoft . Extensions . Logging ;

using WenceyWang . FIGlet ;

namespace Example
{

	public class Program : ProgramBase <Program , ProgramExitCode , ProgramSetting , ProgramSettingCatalog>
	{

		public override string License => "AGPL" ;

		public override bool CanExit { get ; }

		public override bool HandleInput => true ;

		public override bool LoadSetting => true ;

		public override bool AutoSaveSetting => true ;

		public override bool WaitForExit => true ;

		public Application Application { get ; set ; }

		public static void Main ( string [ ] args ) { new Program ( ) . RunMain ( args ) ; }

		public override void Start ( string [ ] args )
		{
			Application = new Application ( LocalConsole . Current , PrepareViewRoot ) ;

			Application . Start ( ) ;
		}

		public override void ConfigureLogger ( ILoggingBuilder builder )
		{
			builder . AddFilter ( level => true ) . AddDebug ( ) ;
			builder . AddFilter ( level => level >= LogLevel . Information ) . AddConsole ( ) ;
		}

		public override void ShowLogo ( )
		{
			Console . WriteLine (
								new AsciiArt (
											"Foggy Console Example" ,
											width : CharacterWidth . Smush ) . ToString ( ) ) ;
		}

		public override void ShowCopyright ( ) { Console . WriteLine ( "Copyright" ) ; }

		public override void OnExit ( ProgramExitCode code ) { }

		public Frame PrepareViewRoot ( )
		{
			Frame      viewRoot = new Frame ( ) ;
			StackPanel panel    = new StackPanel ( ) ;
			Page       page     = new Page ( ) ;
			page . Content = panel ;
			viewRoot . NavigateTo ( page ) ;

			Button buttonA = new Button { Name = "buttonA" , Text = "A" , KeyBind = 'A' } ;
			panel . Items . Add ( buttonA ) ;
			Button buttonB = new Button
							{
								Name            = "buttonB" ,
								Text            = "B" ,
								KeyBind         = 'B' ,
								HorizontalAlign = ContentHorizontalAlign . Right
							} ;
			panel . Items . Add ( buttonB ) ;


			Button buttonExit = new Button
								{
									Name            = "buttonExit" ,
									Text            = "Exit" ,
									KeyBind         = 'E' ,
									BoarderStyle    = LineStyle . SingleLinesSet ,
									AllowSingleLine = false
								} ;
			panel . Items . Add ( buttonExit ) ;

			FIGletLabel label = new FIGletLabel
								{
									Text            = 24 . ToString ( ) ,
									CharacterWidth  = CharacterWidth . Smush ,
									HorizontalAlign = ContentHorizontalAlign . Center
								} ;

			panel . Items . Add ( label ) ;

			HorizontalStackPanel horizontalStack = new HorizontalStackPanel ( ) ;

			panel . Items . Add ( horizontalStack ) ;

			Button buttonC = new Button { Name = "buttonC" , Text = "C" , KeyBind = 'C' } ;
			horizontalStack . Items . Add ( buttonC ) ;

			Button buttonD = new Button { Name = "buttonD" , Text = "D" , KeyBind = 'D' } ;
			horizontalStack . Items . Add ( buttonD ) ;

			Grid grid = new Grid ( ) ;
			panel . Items . Add ( grid ) ;

			Button buttonE = new Button
							{
								Name            = "buttonE" ,
								Text            = "E" ,
								KeyBind         = 'E' ,
								HorizontalAlign = ContentHorizontalAlign . Center
							} ;
			grid . Items . Add ( buttonE ) ;

			Canvas canvas = new Canvas ( ) ;

			for ( int y = 0 ; y < 30 ; y++ )
			{
				for ( int x = 0 ; x < 30 ; x++ )
				{
					Button button = new Button { Name = $"button{x}{y}" , Text = $"{x}{y}" } ;
					canvas . Items . Add ( button ) ;
					canvas [ button ] = new Point ( 6 * x , y ) ;
				}
			}

			panel . Items . Add ( canvas ) ;

			buttonExit . Pressed += ExitButton_Pressed ;

			return viewRoot ;
		}

		private void ExitButton_Pressed ( object sender , EventArgs e ) { Application . Stop ( ) ; }

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
