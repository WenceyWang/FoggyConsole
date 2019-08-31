using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . ComponentModel ;
using System . Linq ;
using System . Reflection ;
using System . Runtime . CompilerServices ;

using DreamRecorder . FoggyConsole . Controls ;

using JetBrains . Annotations ;

namespace DreamRecorder . FoggyConsole
{

	/// <summary>
	///     The actual Application.
	///     It contains a
	///     <code>ViewRoot</code>
	///     in which all other
	///     <code>Control</code>
	///     instances are stored in a
	///     tree-format.
	///     It also manages user input and drawing.
	/// </summary>
	public class Application : INotifyPropertyChanged
	{

		/// <summary>
		///     The root of the Control-Tree
		/// </summary>
		public RootFrame ViewRoot { get ; set ; }

		///// <summary>
		/////     Used as the boundary for the ViewRoot if the terminal-size can't determined
		///// </summary>
		//public static Size StandardRootBoundary { get ; } = new Size ( 80 , 24 ) ;

		/// <summary>
		///     Responsible for focus-changes, for example when the user presses the TAB-key
		/// </summary>
		public FocusManager FocusManager { get ; set ; }

		public KeyBindingManager KeyBindingManager { get ; set ; }

		public IConsole Console { get ; set ; }

		/// <summary>
		///     The name of this application
		/// </summary>
		public virtual string Name { get ; set ; }

		public virtual bool AutoDefaultColor => false ;

		public bool IsDebug { get ; set ; }

		public Func <Control> PrepareViewRoot { get ; set ; }

		public ConsoleColor? DefaultBackgroundColor { get ; set ; }

		public ConsoleColor? DefaultForegroundColor { get ; set ; }

		public Application ( [NotNull] IConsole console , [NotNull] Func <Control> prepareViewRoot )
		{
			PropertyChanged += ApplicationBase_PropertyChanged ;

			Console         = console         ?? throw new ArgumentNullException ( nameof ( console ) ) ;
			PrepareViewRoot = prepareViewRoot ?? throw new ArgumentNullException ( nameof ( prepareViewRoot ) ) ;

			ViewRoot          = new RootFrame ( ) ;
			FocusManager      = new FocusManager ( ) ;
			KeyBindingManager = new KeyBindingManager ( ) ;
		}

		public event PropertyChangedEventHandler PropertyChanged ;

		protected virtual void ApplicationBase_PropertyChanged ( object sender , PropertyChangedEventArgs e )
		{
			PropertyInfo property = GetType ( ) . GetProperty ( e . PropertyName ) ;

			if ( property is PropertyInfo propertyInfo )
			{
				if ( typeof ( IApplicationItem ) . IsAssignableFrom ( propertyInfo . PropertyType ) )
				{
					( ( IApplicationItem ) propertyInfo . GetValue ( this ) ) . Application = this ;
				}
			}
		}

		/// <summary>
		///     Starts this
		///     <code>Application</code>
		///     .
		/// </summary>
		public void Start ( )
		{
			ViewRoot . Content = PrepareViewRoot ? . Invoke ( ) ;

			Console . KeyPressed += KeyWatcherOnKeyPressed ;
			Console . Start ( ) ;

			ViewRoot . Enabled = true ;
			ViewRoot . ResumeRedraw ( ) ;
		}

		public event EventHandler Stopped ;

		/// <summary>
		///     Stops this
		///     <code>Application</code>
		///     .
		/// </summary>
		public void Stop ( )
		{
			if ( ViewRoot != null )
			{
				ViewRoot . Enabled = false ;
			}

			Console . KeyPressed -= KeyWatcherOnKeyPressed ;
			Console . Stop ( ) ;

			Stopped ? . Invoke ( this , EventArgs . Empty ) ;
		}

		private void KeyWatcherOnKeyPressed ( object sender , KeyPressedEventArgs eventArgs )
		{
			KeyBindingManager ? . HandleKey ( eventArgs ) ;

			if ( ! eventArgs . Handled )
			{
				IHandleKeyInput currentHandler = FocusManager ? . FocusedControl ;
				currentHandler ? . HandleKeyInput ( eventArgs ) ;

				if ( ! eventArgs . Handled )
				{
					FocusManager ? . HandleKeyInput ( eventArgs ) ;
				}
			}
		}

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged ( [CallerMemberName] string propertyName = null )
		{
			PropertyChanged ? . Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
		}

	}

}
