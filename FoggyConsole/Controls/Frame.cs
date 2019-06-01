﻿using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . ToolBox . General ;

using Microsoft . Extensions . DependencyInjection ;
using Microsoft . Extensions . Logging ;

using WenceyWang . FoggyConsole . Controls . Renderers ;

namespace WenceyWang . FoggyConsole . Controls
{

	public class Frame : ContentControl
	{

		public static Frame Current { get ; set ; }

		public bool IsRedrawPaused { get ; private set ; }

		internal ILogger Logger { get ; } =
			StaticServiceProvider . Provider . GetService <ILoggerFactory> ( ) . CreateLogger <Frame> ( ) ;

		public override Size Size { get => Window . Size ; set => Window . Size = value ; }

		public override bool CanFocus => false ;

		public Page CurrentPage { get ; private set ; }

		public override Control Content { get => CurrentPage ; set => throw new InvalidOperationException ( ) ; }

		public Frame ( IControlRenderer renderer = null ) : base ( renderer ?? new FrameRenderer ( ) )
		{
			Enabled = false ;
			Current = this ;
		}

		public void PauseRedraw ( ) { IsRedrawPaused = true ; }

		public void ResumeRedraw ( )
		{
			IsRedrawPaused = false ;
			RequestUpdateDisplay ( ) ;
		}

		public override void Measure ( Size availableSize ) { CurrentPage ? . Measure ( availableSize ) ; }

		public override void Arrange ( Rectangle finalRect ) { CurrentPage ? . Arrange ( new Rectangle ( Size ) ) ; }

		public void RequestUpdateDisplay ( ) { RequestMeasure ( ) ; }

		protected override void RequestMeasure ( )
		{
			if ( ( ! IsRedrawPaused ) && Enabled )
			{
				Measure ( Size ) ;
				Arrange ( new Rectangle ( Size ) ) ;
				Draw ( ) ;
			}
		}

		protected override void RequestRedraw ( )
		{
			if ( ( ! IsRedrawPaused ) && Enabled )
			{
				Draw ( ) ;
			}
		}

		public void NavigateTo ( Page page )
		{
			if ( page == null )
			{
				throw new ArgumentNullException ( nameof ( page ) ) ;
			}

			if ( CurrentPage != page )
			{
				CurrentPage             = page ;
				CurrentPage . Container = this ;
				CurrentPage . OnNavigateTo ( ) ;
				Measure ( Size ) ;
				Arrange ( new Rectangle ( Size ) ) ;
				Draw ( ) ;
			}
		}

	}

}