using System;
using System . Text ;
using DreamRecorder.ToolBox.CommandLine;
using Microsoft.Extensions.Logging;
using WenceyWang.FoggyConsole;
using WenceyWang.FoggyConsole.Controls;

namespace Example
{
	public class Program: ApplicationBase<Program, ProgramExitCode, ProgramSetting, ProgramSettingCatalog>
    {
        public static void Main(string[] args)
        {
            new Program().RunMain(args);
        }

        public override void ConfigureLogger(ILoggingBuilder builder)
        {
            builder.AddFilter((level)=>true).AddDebug();
        }

        public override void ShowLogo()
        {
            Console.WriteLine(new WenceyWang.FIGlet.AsciiArt("Foggy Console Example").ToString());
        }

        public override void ShowCopyright()
        {
            Console.WriteLine("Copyright");
        }


        public override void Start(string[] args)
        {
           
            base.Start(args);
        }

        public override Frame PrepareViewRoot()
        {
            ViewRoot = new Frame();
            var panel=new StackPanel();
            var page = new Page();
            page.Content = panel;
            ViewRoot.NavigateTo(page);

            Button buttonA = new Button() { Name = "buttonA", Text = "A",Height=1};
            panel.AddChild(buttonA);
            Button buttonB = new Button() { Name = "buttonB", Text = "B", Height = 1 };
            panel.AddChild(buttonB);
            return ViewRoot;
        }


        public override string License => "AGPL";
        public override bool LoadSetting => true;
        public override bool AutoSaveSetting => true;
    }
    public class ProgramSetting : SettingBase<ProgramSetting, ProgramSettingCatalog>
    {

        [SettingItem((int)ProgramSettingCatalog.General, nameof(BotToken), "", true, "")]
        public string BotToken { get; set; }

        [SettingItem((int)ProgramSettingCatalog.General, nameof(DatabaseConnection), "", true, "")]
        public string DatabaseConnection { get; set; }

        [SettingItem((int)ProgramSettingCatalog.General, nameof(HttpProxy), "", true, null)]
        public string HttpProxy { get; set; }

    }
    public enum ProgramSettingCatalog
    {

        General = 0

    }
    public class ProgramExitCode : ProgramExitCode<ProgramExitCode>
    {

    }
}
