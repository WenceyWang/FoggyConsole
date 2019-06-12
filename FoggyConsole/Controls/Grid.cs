using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Collections . ObjectModel ;
using System . Linq ;

using DreamRecorder . FoggyConsole . Controls . Events ;
using DreamRecorder . FoggyConsole . Controls . Renderers ;

using JetBrains . Annotations ;

namespace DreamRecorder . FoggyConsole . Controls
{

	public class Grid : ItemsContainer
	{

		public Dictionary <Control , Point> StartAt { get ; } = new Dictionary <Control , Point> ( ) ;

		public Dictionary <Control , Size> BlockSize { get ; } = new Dictionary <Control , Size> ( ) ;

		public ObservableCollection <Row> Rows { get ; } = new ObservableCollection <Row> ( ) ;

		public ObservableCollection <Column> Columns { get ; } = new ObservableCollection <Column> ( ) ;

		private List <Column> StaticSizeColumns
			=> Columns . Where ( col => col . Width >= 0 && ! col . Auto ) . ToList ( ) ;

		private List <Column> StarSizeColumns
			=> Columns . Where ( col => col . Width < 0 && ! col . Auto ) . ToList ( ) ;

		private List <Column> AutoSizeColumns => Columns . Where ( col => col . Auto ) . ToList ( ) ;

		private List <Row> StaticSizeRows => Rows . Where ( row => row . Height >= 0 && ! row . Auto ) . ToList ( ) ;

		private List <Row> StarSizeRows => Rows . Where ( row => row . Height < 0 && ! row . Auto ) . ToList ( ) ;

		private List <Row> AutoSizeRows => Rows . Where ( row => row . Auto ) . ToList ( ) ;

		public override bool CanFocus => false ;

		public Grid ( IControlRenderer renderer ) : base ( renderer ?? new GridRenderer ( ) )
		{
			ItemsAdded   += Grid_ItemsAdded ;
			ItemsRemoved += Grid_ItemsRemoved ;
		}

		public Grid ( ) : this ( null ) { }

		private void Grid_ItemsRemoved ( object sender , ContainerControlEventArgs e )
		{
			StartAt . Remove ( e . Control ) ;
			BlockSize . Remove ( e . Control ) ;
		}

		private void Grid_ItemsAdded ( object sender , ContainerControlEventArgs e )
		{
			StartAt [ e . Control ]   = new Point ( 0 , 0 ) ;
			BlockSize [ e . Control ] = new Size ( 1 , 1 ) ;
		}

		public IReadOnlyCollection <Control> GetInsideControls ( [NotNull] Column column )
		{
			if ( column == null )
			{
				throw new ArgumentNullException ( nameof ( column ) ) ;
			}

			int columnIndex = Columns . IndexOf ( column ) ;

			if ( columnIndex == - 1 )
			{
				return new List <Control> ( ) ;
			}
			else
			{
				return Items . Where (
									control
										=> StartAt [ control ] . X           == columnIndex
											&& BlockSize [ control ] . Width == 1 ) .
								ToList ( ) ;
			}
		}

		public IReadOnlyCollection <Control> GetInsideControls ( [NotNull] Row row )
		{
			if ( row == null )
			{
				throw new ArgumentNullException ( nameof ( row ) ) ;
			}

			int rowIndex = Rows . IndexOf ( row ) ;

			if ( rowIndex == - 1 )
			{
				return new List <Control> ( ) ;
			}
			else
			{
				return Items . Where (
									control
										=> StartAt [ control ] . Y            == rowIndex
											&& BlockSize [ control ] . Height == 1 ) .
								ToList ( ) ;
			}
		}

		public override void Measure ( Size availableSize )
		{
			if ( ! Rows . Any ( ) )
			{
				Rows . Add ( new Row { Height = - 1 } ) ;
			}

			if ( ! Columns . Any ( ) )
			{
				Columns . Add ( new Column { Width = - 1 } ) ;
			}

			#region Measure Columns

			List <Column> staticSizeColumns = StaticSizeColumns ;

			foreach ( Column column in staticSizeColumns )
			{
				column . DesiredWidth = column . Width ;
			}

			foreach ( Column column in AutoSizeColumns )
			{
				IReadOnlyCollection <Control> controls = GetInsideControls ( column ) ;

				foreach ( Control control in controls )
				{
					control . Measure ( new Size ( int . MaxValue , int . MaxValue ) ) ;
				}

				int resultWidth = controls . Max ( control => control . DesiredSize . Width ) ;

				column . DesiredWidth = resultWidth ;
			}

			List <Column> starSizeColumns = StarSizeColumns ;

			int minStarWidth = starSizeColumns . Min ( col => col . Star ) ;

			List <Column> minStarWidthColumn =
				starSizeColumns . Where ( col => col . Star == minStarWidth ) . ToList ( ) ;

			int maxMinStarWidth = 0 ;

			foreach ( Column column in minStarWidthColumn )
			{
				IReadOnlyCollection <Control> controls = GetInsideControls ( column ) ;

				foreach ( Control control in controls )
				{
					control . Measure ( new Size ( int . MaxValue , int . MaxValue ) ) ;
				}

				int resultWidth = controls . Max ( control => control . DesiredSize . Width ) ;

				maxMinStarWidth = Math . Max ( resultWidth , maxMinStarWidth ) ;
			}

			double starWidth = maxMinStarWidth / ( double ) minStarWidth ;

			foreach ( Column column in starSizeColumns )
			{
				column . DesiredWidth = ( int ) ( starWidth * column . Star ) ;
			}

			#endregion

			#region Measure Rows

			List <Row> staticSizeRows = StaticSizeRows ;

			foreach ( Row row in staticSizeRows )
			{
				row . DesiredHeight = row . Height ;
			}

			foreach ( Row row in AutoSizeRows )
			{
				IReadOnlyCollection <Control> controls = GetInsideControls ( row ) ;

				foreach ( Control control in controls )
				{
					control . Measure ( new Size ( int . MaxValue , int . MaxValue ) ) ;
				}

				int resultHeight = controls . Max ( control => control . DesiredSize . Height ) ;

				row . DesiredHeight = resultHeight ;
			}

			List <Row> starSizeRows = StarSizeRows ;

			int minStarHeight = starSizeRows . Min ( row => row . Star ) ;

			List <Row> minStarHeightRow = starSizeRows . Where ( row => row . Star == minStarHeight ) . ToList ( ) ;

			int maxMinStarHeight = 0 ;

			foreach ( Row row in minStarHeightRow )
			{
				IReadOnlyCollection <Control> controls = GetInsideControls ( row ) ;

				foreach ( Control control in controls )
				{
					control . Measure ( new Size ( int . MaxValue , int . MaxValue ) ) ;
				}

				int resultHeight = controls . Max ( control => control . DesiredSize . Height ) ;

				maxMinStarHeight = Math . Max ( resultHeight , maxMinStarHeight ) ;
			}

			double starHeight = maxMinStarHeight / ( double ) minStarHeight ;

			foreach ( Row row in starSizeRows )
			{
				row . DesiredHeight = ( int ) ( starHeight * row . Star ) ;
			}

			#endregion

			foreach ( Control control in Items )
			{
				Point startAt   = StartAt [ control ] ;
				Size  blockSize = BlockSize [ control ] ;
				int   width     = 0 ;
				for ( int i = startAt . X ; i < Columns . Count && i < startAt . X + blockSize . Width ; i++ )
				{
					width += Columns [ i ] . DesiredWidth ;
				}

				int height = 0 ;
				for ( int i = startAt . Y ; i < Rows . Count && i < startAt . Y + blockSize . Height ; i++ )
				{
					height += Rows [ i ] . DesiredHeight ;
				}

				control . Measure ( new Size ( width , height ) ) ;
			}

			int widthSum = Columns . Sum ( col => col . DesiredWidth ) ;

			if ( ! AutoWidth )
			{
				widthSum = Math . Max ( Width , widthSum ) ;
			}

			int heightSum = Rows . Sum ( row => row . DesiredHeight ) ;

			if ( ! AutoHeight )
			{
				heightSum = Math . Max ( Height , heightSum ) ;
			}

			DesiredSize = new Size ( widthSum , heightSum ) ;
		}

		public override void Arrange ( Rectangle finalRect )
		{
			int measuredWidth = 0 ;

			List <Column> staticSizeColumns = StaticSizeColumns ;

			foreach ( Column column in staticSizeColumns )
			{
				column . ActualWidth = column . DesiredWidth ;
			}

			measuredWidth += staticSizeColumns . Sum ( col => col . ActualWidth ) ;

			List <Column> autoSizeColumns = AutoSizeColumns ;

			foreach ( Column column in autoSizeColumns )
			{
				column . ActualWidth = column . DesiredWidth ;
			}

			measuredWidth += autoSizeColumns . Sum ( col => col . ActualWidth ) ;

			int remainedWidth = finalRect . Width - measuredWidth ;

			List <Column> starSizeColumns = StarSizeColumns ;

			int starWidthSum = starSizeColumns . Sum ( row => row . Star ) ;

			foreach ( Column column in starSizeColumns )
			{
				column . ActualWidth = ( int ) ( remainedWidth / ( double ) starWidthSum * column . Star ) ;
			}

			int measuredHeight = 0 ;

			List <Row> staticSizeRows = StaticSizeRows ;

			foreach ( Row row in staticSizeRows )
			{
				row . ActualHeight = row . DesiredHeight ;
			}

			measuredHeight += staticSizeRows . Sum ( col => col . ActualHeight ) ;

			List <Row> autoSizeRows = AutoSizeRows ;

			foreach ( Row row in autoSizeRows )
			{
				row . ActualHeight = row . DesiredHeight ;
			}

			measuredHeight += autoSizeRows . Sum ( col => col . ActualHeight ) ;

			int remainedHeight = finalRect . Height - measuredHeight ;

			List <Row> starSizeRows = StarSizeRows ;

			int starHeightSum = starSizeRows . Sum ( row => row . Star ) ;

			foreach ( Row row in starSizeRows )
			{
				row . ActualHeight = ( int ) ( remainedHeight / ( double ) starHeightSum * row . Star ) ;
			}

			foreach ( Control control in Items )
			{
				Point startAt   = StartAt [ control ] ;
				Size  blockSize = BlockSize [ control ] ;

				int left = 0 ;
				for ( int i = 0 ; i < startAt . X ; i++ )
				{
					left += Columns [ i ] . ActualWidth ;
				}

				int width = 0 ;
				for ( int i = startAt . X ; i < Columns . Count && i < startAt . X + blockSize . Width ; i++ )
				{
					width += Columns [ i ] . ActualWidth ;
				}

				int top = 0 ;
				for ( int i = 0 ; i < startAt . Y ; i++ )
				{
					top += Rows [ i ] . ActualHeight ;
				}

				int height = 0 ;
				for ( int i = startAt . Y ; i < Rows . Count && i < startAt . Y + blockSize . Height ; i++ )
				{
					height += Rows [ i ] . ActualHeight ;
				}

				switch ( control . HorizontalAlign )
				{
					case ContentHorizontalAlign . Left :
					{
						width = Math . Min ( width , control . DesiredSize . Width ) ;
						break ;
					}

					case ContentHorizontalAlign . Center :
					{
						left  += Math . Max ( width - control . DesiredSize . Width , 0 ) / 2 ;
						width =  Math . Min ( width , control . DesiredSize . Width ) ;
						break ;
					}

					case ContentHorizontalAlign . Right :
					{
						left  += Math . Max ( width - control . DesiredSize . Width , 0 ) ;
						width =  Math . Min ( width , control . DesiredSize . Width ) ;
						break ;
					}

					default :
					case ContentHorizontalAlign . Stretch :
					{
						break ;
					}
				}

				switch ( control . VerticalAlign )
				{
					case ContentVerticalAlign . Top :
					{
						height = Math . Min ( height , control . DesiredSize . Height ) ;
						break ;
					}

					case ContentVerticalAlign . Center :
					{
						top    += Math . Max ( height - control . DesiredSize . Height , 0 ) / 2 ;
						height =  Math . Min ( height , control . DesiredSize . Height ) ;
						break ;
					}

					case ContentVerticalAlign . Bottom :
					{
						top    += Math . Max ( height - control . DesiredSize . Height , 0 ) ;
						height =  Math . Min ( height , control . DesiredSize . Height ) ;
						break ;
					}

					default :
					case ContentVerticalAlign . Stretch :
					{
						break ;
					}
				}

				control . Arrange (
									new Rectangle (
													finalRect . LeftTopPoint . Offset ( left , top ) ,
													new Size ( width , height ) ) ) ;
			}

			base . Arrange ( finalRect ) ;
		}

		public class Row
		{

			public int Height { get ; set ; }

			public bool Auto { get ; set ; }

			public int DesiredHeight { get ; set ; }

			public int ActualHeight { get ; set ; }

			public int Star
			{
				get
				{
					if ( Height < 0 )
					{
						return - Height ;
					}
					else
					{
						throw new InvalidOperationException ( ) ;
					}
				}
			}

		}

		public class Column
		{

			public int Width { get ; set ; }

			public bool Auto { get ; set ; }

			public int DesiredWidth { get ; set ; }

			public int ActualWidth { get ; set ; }


			public int Star
			{
				get
				{
					if ( Width < 0 )
					{
						return - Width ;
					}
					else
					{
						throw new InvalidOperationException ( ) ;
					}
				}
			}

		}

	}

}
