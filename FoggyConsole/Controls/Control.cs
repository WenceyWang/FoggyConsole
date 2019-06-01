using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . ComponentModel ;
using System . Linq ;
using System . Runtime . CompilerServices ;

using JetBrains . Annotations ;

using WenceyWang . FoggyConsole . Controls . Renderers ;

namespace WenceyWang . FoggyConsole . Controls
{

	/// <summary>
	///     The base class for all controls
	/// </summary>
	public abstract class Control : INotifyPropertyChanged
	{

		private ConsoleColor ? _backgroundColor ;

		private LineStyle ? _boarderStyle ;

		private bool _enabled = true ;

		private ConsoleColor ? _foregroundColor ;

		private bool _isFocused ;

		private IControlRenderer _renderer ;

		private Size _size ;

		/// <summary>
		///     The name of this Control, must be unique within its Container
		/// </summary>
		public string Name { get ; set ; }

		public abstract bool CanFocus { get ; }

		public virtual bool AutoWidth { get ; set ; } = true ;

		public virtual bool AutoHeight { get ; set ; } = true ;

		public virtual Size AutoDesiredSize
		{
			get
			{
				if ( BoarderStyle == null )
				{
					return new Size ( 1 , 1 ) ;
				}
				else
				{
					return new Size ( 2 , 2 ) ;
				}
			}
		}

		public virtual Size Size
		{
			get => _size ;
			set
			{
				if ( _size != value )
				{
					_size = value ;
					RequestMeasure ( ) ;
				}
			}
		}

		/// <summary>
		///     The width of this Control in characters
		/// </summary>
		public int Width
		{
			get => Size . Width ;
			set
			{
				if ( value < 0 )
				{
					throw new ArgumentException ( $"{nameof ( Width )} has to be bigger than zero." ) ;
				}

				if ( Width != value )
				{
					Size      = new Size ( value , Size . Height ) ;
					AutoWidth = false ;
				}
			}
		}

		/// <summary>
		///     The height of this Control in characters
		/// </summary>
		public int Height
		{
			get => Size . Height ;
			set
			{
				if ( value < 0 )
				{
					throw new ArgumentException ( $"{nameof ( Height )} has to be bigger than zero." ) ;
				}

				if ( Height != value )
				{
					Size       = new Size ( Size . Width , value ) ;
					AutoHeight = false ;
				}
			}
		}


		public Size DesiredSize { get ; protected set ; }

		public Rectangle RenderArea { get ; protected set ; }

		public Point RenderPoint => RenderArea . LeftTopPoint ;

		public Size ActualSize => RenderArea . Size ;

		public int ActualWidth => ActualSize . Width ;

		public int ActualHeight => ActualSize . Height ;

		public ConsoleColor ActualBackgroundColor
			=> _backgroundColor ?? Container ? . BackgroundColor ?? ConsoleColor . Black ;

		/// <summary>
		///     The background-color
		/// </summary>
		public ConsoleColor ? BackgroundColor
		{
			get => _backgroundColor ;
			set
			{
				if ( _backgroundColor != value )
				{
					_backgroundColor = value ;
					RequestRedraw ( ) ;
				}
			}
		}

		public ConsoleColor ActualForegroundColor
			=> _foregroundColor ?? Container . ForegroundColor ?? ConsoleColor . Gray ;

		/// <summary>
		///     The foreground-color
		/// </summary>
		public ConsoleColor ? ForegroundColor
		{
			get => _foregroundColor ;
			set
			{
				if ( _foregroundColor != value )
				{
					_foregroundColor = value ;
					RequestRedraw ( ) ;
				}
			}
		}

		public LineStyle ? BoarderStyle
		{
			get => _boarderStyle ;
			set
			{
				if ( _boarderStyle != value )
				{
					bool needRearrange =
						_boarderStyle == null && value != null || _boarderStyle != null && value == null ;

					_boarderStyle = value ;
					if ( needRearrange )
					{
						RequestMeasure ( ) ;
					}
					else
					{
						RequestRedraw ( ) ;
					}
				}
			}
		}

		/// <summary>
		///     True if the control is focuses, otherwise false
		/// </summary>
		public bool IsFocused
		{
			get => _isFocused ;
			set
			{
				if ( _isFocused != value )
				{
					_isFocused = value ;
					OnIsFocusedChanged ( ) ;
					RequestRedraw ( ) ;
				}
			}
		}

		/// <summary>
		///     Used to determine the order of controls when the user uses the TAB-key navigate between them
		/// </summary>
		public int TabIndex { get ; set ; }

		/// <summary>
		///     The
		///     <code>Container</code>
		///     in which this Control is placed in
		/// </summary>
		public Container Container { get ; set ; }

		/// <summary>
		///     An instance of a subclass of
		///     <code>ControlRenderer</code>
		///     which is able to draw this specific type of Control
		/// </summary>
		/// <exception cref="ArgumentException">
		///     Thrown if the ControlRenderer which should be set already has an other Control
		///     assigned
		/// </exception>
		public IControlRenderer Renderer
		{
			get => _renderer ;
			set
			{
				if ( value ? . Control != null
					&& value . Control != this )
				{
					throw new ArgumentException (
												$"{nameof ( Renderer )} already has an other {nameof ( Control )} assigned." ,
												nameof ( value ) ) ;
				}

				_renderer = value ;
			}
		}


		public Page Page => Container as Page ?? Container . Page ;

		public bool Enabled
		{
			get => _enabled ;
			set
			{
				if ( _enabled != value )
				{
					_enabled = value ;
					RequestRedraw ( ) ;
				}
			}
		}

		public virtual char ? KeyBind { get ; set ; } = null ;

		/// <summary>
		///     Creates a new
		///     <code>Control</code>
		/// </summary>
		/// <param name="renderer">
		///     The
		///     <code>ControlRenderer</code>
		///     to set
		/// </param>
		/// <exception cref="ArgumentException">
		///     Thrown if the ControlRenderer which should be set already has an other Control
		///     assigned
		/// </exception>
		public Control ( IControlRenderer renderer )
		{
			Renderer = renderer ;

			Renderer . Control = this ;
		}

		public event PropertyChangedEventHandler PropertyChanged ;


		/// <summary>
		///     Fired if the
		///     <code>IsFocused</code>
		///     -Property has been changed
		/// </summary>
		public event EventHandler IsFocusedChanged ;

		private void OnIsFocusedChanged ( ) { IsFocusedChanged ? . Invoke ( this , EventArgs . Empty ) ; }

		public virtual void KeyPressed ( KeyPressedEventArgs args ) { }

		public virtual void Measure ( Size availableSize )
		{
			int width ;
			if ( AutoHeight )
			{
				width = AutoDesiredSize . Width ;
			}
			else
			{
				width = Size . Width ;
			}

			int height ;
			if ( AutoHeight )
			{
				height = AutoDesiredSize . Height ;
			}
			else
			{
				height = Size . Height ;
			}

			DesiredSize = new Size ( width , height ) ;
		}

		public virtual void Arrange ( Rectangle finalRect ) { RenderArea = finalRect ; }

		public void Draw ( ) { Renderer . Draw ( ) ; }

		protected virtual void RequestMeasure ( ) { Container ? . RequestMeasure ( ) ; }

		protected virtual void RequestRedraw ( ) { Container ? . RequestRedraw ( ) ; }

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged ( [CallerMemberName] string propertyName = null )
		{
			PropertyChanged ? . Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
		}

	}

}
