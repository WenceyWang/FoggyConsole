﻿using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . ComponentModel ;
using System . Linq ;
using System . Runtime . CompilerServices ;

using DreamRecorder . FoggyConsole . Controls . Renderers ;

using JetBrains . Annotations ;

namespace DreamRecorder . FoggyConsole . Controls
{

	/// <summary>
	///     The base class for all controls
	/// </summary>
	public abstract class Control : INotifyPropertyChanged , IHandleKeyInput
	{

		private bool _allowSingleLine = true ;

		private ConsoleColor ? _backgroundColor ;

		private LineStyle ? _boarderStyle ;

		private bool _enabled = true ;

		private ConsoleColor ? _foregroundColor ;

		private ContentHorizontalAlign _horizontalAlign ;

		private bool _isFocused ;

		private IControlRenderer _renderer ;

		private Size _size ;

		private ContentVerticalAlign _verticalAlign ;

		/// <summary>
		///     The name of this Control, must be unique within its ContainerBase
		/// </summary>
		public string Name { get ; set ; }

		public abstract bool CanFocusedOn { get ; }

		public ContentHorizontalAlign HorizontalAlign
		{
			get => _horizontalAlign ;
			set
			{
				if ( value != _horizontalAlign )
				{
					_horizontalAlign = value ;
					RequestMeasure ( ) ;
				}
			}
		}

		public ContentVerticalAlign VerticalAlign
		{
			get => _verticalAlign ;
			set
			{
				if ( value != _verticalAlign )
				{
					_verticalAlign = value ;
					RequestMeasure ( ) ;
				}
			}
		}

		public virtual bool AutoWidth { get ; set ; } = true ;

		public virtual bool AutoHeight { get ; set ; } = true ;

		public virtual Size AutoDesiredSize
		{
			get
			{
				if ( BoarderStyle == null )
				{
					return Size . One ;
				}

				return Size . Empty ;
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

		public Rectangle ? RenderArea { get ; protected set ; }

		public Point ? RenderPoint => RenderArea ? . LeftTopPoint ;

		public Size ActualSize => RenderArea ? . Size ?? Size . Empty ;

		public int ActualWidth => ActualSize . Width ;

		public int ActualHeight => ActualSize . Height ;

		public Rectangle ? ContentArea
		{
			get
			{
				if ( BoarderStyle != null
					 && RenderArea is Rectangle renderArea )
				{
					if ( ActualHeight == 1 )
					{
						return new Rectangle (
											  renderArea . LeftTopPoint   + new Vector ( 1 ,   0 ) ,
											  renderArea . RightDownPoint + new Vector ( - 1 , 0 ) ) ;
					}
					else
					{
						return new Rectangle (
											  renderArea . LeftTopPoint   + new Vector ( 1 ,   1 ) ,
											  renderArea . RightDownPoint + new Vector ( - 1 , - 1 ) ) ;
					}
				}
				else
				{
					return RenderArea ;
				}
			}
		}

		public Size ContentSize => ContentArea ? . Size ?? Size . Empty ;

		public int ContentHeight => ContentSize . Height ;

		public int ContentWidth => ContentSize . Width ;

		public ConsoleColor ActualBackgroundColor
			=> _backgroundColor
			   ?? Container ? . ActualBackgroundColor
			   ?? ViewRoot ? . Application ? . DefaultBackgroundColor ?? ConsoleChar . DefaultBackgroundColor ;

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
			=> _foregroundColor
			   ?? Container ? . ActualForegroundColor
			   ?? ViewRoot ? . Application ? . DefaultForegroundColor ?? ConsoleChar . DefaultForegroundColor ;

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
					bool needRearrange = _boarderStyle    == null && value != null
										 || _boarderStyle != null && value == null ;

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

		public object Tag { get ; set ; }

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
		public int ? TabIndex { get ; set ; } = null ;

		/// <summary>
		///     The
		///     <code>ContainerBase</code>
		///     in which this Control is placed in
		/// </summary>
		public ContainerBase Container { get ; set ; }

		[CanBeNull]
		public virtual RootFrame ViewRoot => Container ? . ViewRoot ;

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
				if ( value ? . Control  != null
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
					RequestMeasure ( ) ;
				}
			}
		}

		public virtual char ? KeyBind { get ; set ; } = null ;

		public bool AllowSingleLine
		{
			get => _allowSingleLine ;
			set
			{
				if ( _allowSingleLine != value )
				{
					_allowSingleLine = value ;
					RequestMeasure ( ) ;
				}
			}
		}

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

		public void HandleKeyInput ( KeyPressedEventArgs args )
		{
			OnKeyPressed ( args ) ;
			if ( ! args . Handled )
			{
				Container ? . HandleKeyInput ( args ) ;
			}
		}


		public event PropertyChangedEventHandler PropertyChanged ;


		/// <summary>
		///     Fired if the
		///     <code>IsFocused</code>
		///     -Property has been changed
		/// </summary>
		public event EventHandler IsFocusedChanged ;

		private void OnIsFocusedChanged ( ) { IsFocusedChanged ? . Invoke ( this , EventArgs . Empty ) ; }

		public virtual void OnKeyPressed ( KeyPressedEventArgs args ) { }

		public virtual void Measure ( Size availableSize )
		{			
            int width  = availableSize. Width ;
			int height = availableSize. Height ;

            if (BoarderStyle != null)
            {
                if (height != 1
                    || !AllowSingleLine)
                {
                    height -= 2;
                }

                width -= 2;
            }

            Size sizeWithoutBoarder = MeasureOverride ( new Size(width,height)) ;

            width = sizeWithoutBoarder.Width;
            height = sizeWithoutBoarder.Height;

            if ( BoarderStyle != null )
			{
				if ( height != 1
					 || ! AllowSingleLine )
				{
					height += 2 ;
				}

				width += 2 ;
			}

			DesiredSize = new Size ( width , height ) ;
		}

		public virtual Size MeasureOverride ( Size availableSize )
		{
            Size autoDesiredSize = AutoDesiredSize;

            int width ;
			if ( AutoWidth )
			{
				width = autoDesiredSize . Width ;
			}
			else
			{
				width = Size . Width ;
			}

			int height ;
			if ( AutoHeight )
			{
				height = autoDesiredSize . Height ;
			}
			else
			{
				height = Size . Height ;
			}

			return new Size ( width , height ) ;
		}


		public virtual void Arrange ( Rectangle ? finalRect )
		{
			if ( finalRect . IsNotEmpty ( ) )
			{
				ArrangeOverride ( finalRect . Value ) ;
			}
			else
			{
				RenderArea = null ;
			}
		}

		public virtual void ArrangeOverride ( Rectangle finalRect ) { RenderArea = finalRect ; }

		public void Draw ( Application application , ConsoleArea area ) { Renderer . Draw ( application , area ) ; }

		protected virtual void RequestMeasure ( ) { Container ? . RequestMeasure ( ) ; }

		protected virtual void RequestRedraw ( ) { Container ? . RequestRedraw ( ) ; }

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged ( [CallerMemberName] string propertyName = null )
		{
			PropertyChanged ? . Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
		}

	}

}
