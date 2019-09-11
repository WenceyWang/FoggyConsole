using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System.Net;
using System.Net.Sockets;
using DreamRecorder . FoggyConsole . Controls ;
using DreamRecorder . FoggyConsole . Example . Pages ;
using DreamRecorder . ToolBox . CommandLine ;
using DreamRecorder . ToolBox . General ;

using Microsoft . Extensions . Logging ;

using WenceyWang . FIGlet ;

namespace DreamRecorder . FoggyConsole . Example
{

	public class Program : ProgramBase <Program , ProgramExitCode , ProgramSetting , ProgramSettingCatalog>
	{

		public override string License => typeof ( Program ) . GetResourceFile ( @"License.AGPL.txt" ) ;

		public override bool CanExit => true ;

		public override bool HandleInput => true ;

		public override bool LoadSetting => true ;

		public override bool AutoSaveSetting => true ;

		public override bool CheckLicense => false ;

		public override bool WaitForExit => true ;

		public Application Application { get ; set ; }

		private const string Name = "Foggy Console Example" ;

		public static void Main ( string [ ] args ) { new Program ( ) . RunMain ( args ) ; }

		public override void Start ( string [ ] args )
		{
            ////SerialPort port = new SerialPort("COM7");

            ////port.Open();

            TcpListener listener = new TcpListener(IPAddress.Any, Setting.PortNumber);

            listener.Start();

            TcpClient connection = listener.AcceptTcpClient();

            XtermConsole.XtermConsole console = new XtermConsole.XtermConsole(connection.GetStream());

            Application =
				new Application ( console , PrepareViewRoot )
				{
					Name = Name , IsDebug = IsDebug
				} ;

			Application . Start ( ) ;
		}

		public override void ShowCopyright ( )
		{
			Console . WriteLine ( $"{Name} Copyright (C) 2019 - {DateTime . Now . Year} Wencey Wang." ) ;
			Console . WriteLine ( @"This program comes with ABSOLUTELY NO WARRANTY." ) ;
			Console . WriteLine (
								 @"This is free software, and you are welcome to redistribute it under certain conditions; read License.txt for details." ) ;
		}

		public override void ConfigureLogger ( ILoggingBuilder builder )
		{
			builder . AddFilter ( level => true ) . AddDebug ( ) ;
			builder . AddFilter ( level => level >= LogLevel . Information ) . AddConsole ( ) ;
		}

		public override void ShowLogo ( )
		{
			Console . WriteLine ( new AsciiArt ( Name , width : CharacterWidth . Smush ) . ToString ( ) ) ;
		}

		public override void OnExit ( ProgramExitCode code ) { Application . Stop ( ) ; }

		public Frame PrepareViewRoot ( )
		{
			Frame viewRoot = new Frame ( ) ;
			Page  page     = new ControlDisplayPage ( ) ;
			viewRoot . NavigateTo ( page ) ;

			return viewRoot ;
		}

		private void ExitButton_Pressed ( object sender , EventArgs e ) { Application . Stop ( ) ; }

	}

}
