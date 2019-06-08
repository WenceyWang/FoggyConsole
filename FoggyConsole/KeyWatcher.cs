using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Threading ;

using DreamRecorder . FoggyConsole . Controls ;

namespace DreamRecorder . FoggyConsole
{

	/// <summary>
	///     Watches out of user input using
	///     <code>Console.KeyAvailable</code>
	///     and
	///     <code>Code.ReadKey</code>
	/// </summary>
	internal static class KeyWatcher
	{

		public static bool IsRunning { get ; private set ; }

		public static Thread WatcherThread { get ; private set ; }

		/// <summary>
		///     Is fired when a user presses an key
		/// </summary>
		public static event EventHandler <KeyPressedEventArgs> KeyPressed ;

		public static void Start ( )
		{
			if ( ! IsRunning )
			{
				WatcherThread            =  new Thread ( Watch ) { Name = "KeyWatcher" } ;
				IsRunning                =  true ;
				Console . CancelKeyPress += Console_CancelKeyPress ;
				WatcherThread . Start ( ) ;
			}
		}

		private static void Console_CancelKeyPress ( object sender , ConsoleCancelEventArgs e ) { e . Cancel = true ; }


		private static void Watch ( )
		{
			bool isPaused = false ;

			Window . OldSize = Window . Size ;

			while ( IsRunning )
			{
				Size newSize = Window . Size ;

				if ( newSize == Window . OldSize )
				{
					if ( isPaused )
					{
						Frame . Current . ResumeRedraw ( ) ;
						isPaused = false ;
					}
				}
				else
				{
					if ( ! isPaused )
					{
						Frame . Current . PauseRedraw ( ) ;
						isPaused = true ;
					}

					Window . OldSize = newSize ;
				}

				if ( ! Console . KeyAvailable )
				{
					Thread . Yield ( ) ;
					Thread . Sleep ( 1000 / 120 ) ;
				}
				else
				{
					ConsoleKeyInfo keyInfo = Console . ReadKey ( true ) ;
					KeyPressed ? . Invoke ( null , new KeyPressedEventArgs ( keyInfo ) ) ;
				}
			}
		}

		/// <summary>
		///     Stops the internal watcher-thread
		/// </summary>
		public static void Stop ( ) { IsRunning = false ; }

	}

}
