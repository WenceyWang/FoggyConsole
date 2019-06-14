using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Text ;

using DreamRecorder . FoggyConsole . Controls ;
using DreamRecorder . ToolBox . CommandLine ;

using Microsoft . Extensions . Logging ;

namespace DreamRecorder . FoggyConsole
{

	/// <summary>
	///     The actual Application.
	///     It contains a
	///     <code>RootControl</code>
	///     in which all other
	///     <code>Control</code>
	///     instances are stored in a
	///     tree-format.
	///     It also manages user input and drawing.
	/// </summary>
	public abstract class ApplicationBase
		<T , TExitCode , TSetting , TSettingCategory> : ProgramBase <T , TExitCode , TSetting , TSettingCategory>
		where T : ApplicationBase <T , TExitCode , TSetting , TSettingCategory>
		where TExitCode : ProgramExitCode <TExitCode> , new ( )
		where TSetting : SettingBase <TSetting , TSettingCategory> , new ( )
		where TSettingCategory : Enum , IConvertible
	{

		/// <summary>
		///     The root of the Control-Tree
		/// </summary>
		public Frame ViewRoot { get ; set ; }

		/// <summary>
		///     Used as the boundary for the ViewRoot if the terminal-size can't determined
		/// </summary>
		public static Size StandardRootBoundary { get ; } = new Size ( 80 , 24 ) ;


		/// <summary>
		///     Responsible for focus-changes, for example when the user presses the TAB-key
		/// </summary>
		public FocusManager FocusManager { get ; set ; }

		/// <summary>
		///     The name of this application
		/// </summary>
		public virtual string Name { get ; set ; }

		public override bool WaitForExit => true ;

		public override bool CanExit => false ;

		public override bool HandleInput => true ;

		public virtual bool WaitForStart { get ; } = false ;

		public override void Start ( string [ ] args ) { Start ( ) ; }

		public override void OnExit ( TExitCode code ) { Stop ( ) ; }

		public abstract Frame PrepareViewRoot ( ) ;

		public override void BeforePrepare ( )
		{
			ViewRoot     = PrepareViewRoot ( ) ;
			FocusManager = new FocusManager ( ViewRoot ) ;

			base . BeforePrepare ( ) ;
		}

		/// <summary>
		///     Starts this
		///     <code>ApplicationBase</code>
		///     .
		/// </summary>
		public void Start ( )
		{
			if ( WaitForExit )
			{
				Console . WriteLine ( "Press Enter to start {0}" , Name ) ;
				Console . ReadLine ( ) ;
			}

			if ( ! IsDebug )
			{
				Console . CursorVisible = false ;
			}

			Console . OutputEncoding = Encoding . UTF8 ;
			Console . InputEncoding  = Encoding . UTF8 ;

			if ( Name != null )
			{
				Console . Title = Name ;
			}

			KeyWatcher . KeyPressed += KeyWatcherOnKeyPressed ;
			KeyWatcher . Start ( ) ;

			ViewRoot . Enabled = true ;
			ViewRoot . ResumeRedraw ( ) ;
		}

		/// <summary>
		///     Stops this
		///     <code>ApplicationBase</code>
		///     .
		/// </summary>
		public void Stop ( )
		{
			if ( KeyWatcher . IsRunning )
			{
				KeyWatcher . KeyPressed -= KeyWatcherOnKeyPressed ;
				KeyWatcher . Stop ( ) ;

				Console . ResetColor ( ) ;
				Console . SetCursorPosition ( 0 , 0 ) ;
				Console . Clear ( ) ;
				Console . CursorVisible = true ;
			}
		}


		private void KeyWatcherOnKeyPressed ( object sender , KeyPressedEventArgs eventArgs )
		{
			if ( IsDebug )
			{
				Logger . LogDebug ( $"Key pressed: {eventArgs . KeyInfo . Key}" ) ;
			}

			ViewRoot . PauseRedraw ( ) ;

			Control currentControl = FocusManager ? . FocusedControl ;
			while ( ! ( currentControl is null ) )
			{
				currentControl ? . KeyPressed ( eventArgs ) ;
				if ( ! eventArgs . Handled )
				{
					currentControl = currentControl ? . Container ;
				}
				else
				{
					ViewRoot . ResumeRedraw ( ) ;
					return ;
				}
			}

			if ( FocusManager != null )
			{
				FocusManager . HandleKeyInput ( eventArgs ) ;
				if ( IsDebug )
				{
					Logger . LogDebug ( $"Focused: {FocusManager . FocusedControl ? . Name}" ) ;
				}
			}

			ViewRoot . ResumeRedraw ( ) ;
		}

	}

}
