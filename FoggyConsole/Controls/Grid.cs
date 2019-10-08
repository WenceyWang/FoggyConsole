using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Collections . ObjectModel ;
using System . Collections . Specialized ;
using System . ComponentModel ;
using System . Linq ;
using System . Runtime . CompilerServices ;
using System . Text ;

using DreamRecorder . FoggyConsole . Controls . Events ;
using DreamRecorder . FoggyConsole . Controls . Renderers ;

using JetBrains . Annotations ;

namespace DreamRecorder . FoggyConsole . Controls
{

	public class Grid : ItemsContainer , IItemDependencyContainer <Rectangle>
	{

		public string RowsData
		{
			get
			{
				StringBuilder builder = new StringBuilder ( ) ;
				foreach ( Row row in Rows )
				{
					builder . Append ( row . Height ) ;
					builder . Append ( ',' ) ;
				}

				return builder . ToString ( ) ;
			}
			set
			{
				Rows . Clear ( ) ;
				if ( value != null )
				{
					string [ ] rows = value . Split ( new [ ] { ',' } , StringSplitOptions . RemoveEmptyEntries ) ;
					foreach ( string row in rows )
					{
						string rowData = row ;

						bool auto = rowData . EndsWith ( "+" ) ;
						if ( auto )
						{
							rowData = rowData . TrimEnd ( '+' ) ;
						}

						Rows . Add ( new Row { Height = Convert . ToInt32 ( rowData ) } ) ;
					}
				}
			}
		}

		public string ColumnsData
		{
			get
			{
				StringBuilder builder = new StringBuilder ( ) ;
				foreach ( Column column in Columns )
				{
					builder . Append ( column . Width ) ;
					builder . Append ( ',' ) ;
				}

				return builder . ToString ( ) ;
			}
			set
			{
				Columns . Clear ( ) ;
				if ( value != null )
				{
					string [ ] columns = value . Split ( new [ ] { ',' } , StringSplitOptions . RemoveEmptyEntries ) ;
					foreach ( string column in columns )
					{
						string columnData = column ;

						bool auto = columnData . EndsWith ( "+" ) ;
						if ( auto )
						{
							columnData = columnData . TrimEnd ( '+' ) ;
						}

						Columns . Add ( new Column { Width = Convert . ToInt32 ( columnData ) , Auto = auto } ) ;
					}
				}
			}
		}

		protected Dictionary <Control , Point> StartAt { get ; } = new Dictionary <Control , Point> ( ) ;

		protected Dictionary <Control , Size> BlockSize { get ; } = new Dictionary <Control , Size> ( ) ;

		public ObservableCollection <Row> Rows { get ; }

		public ObservableCollection <Column> Columns { get ; }

		private List <Column> StaticSizeColumns
			=> Columns . Where ( col => col . Width >= 0 && ! col . Auto ) . ToList ( ) ;

		private List <Column> StarSizeColumns
			=> Columns . Where ( col => col . Width < 0 && ! col . Auto ) . ToList ( ) ;

		private List <Column> AutoSizeColumns => Columns . Where ( col => col . Auto ) . ToList ( ) ;

		private List <Row> StaticSizeRows => Rows . Where ( row => row . Height >= 0 && ! row . Auto ) . ToList ( ) ;

		private List <Row> StarSizeRows => Rows . Where ( row => row . Height < 0 && ! row . Auto ) . ToList ( ) ;

		private List <Row> AutoSizeRows => Rows . Where ( row => row . Auto ) . ToList ( ) ;

		public override bool CanFocusedOn => false ;

		public Grid ( IControlRenderer renderer ) : base ( renderer ?? new ItemsContainerRenderer ( ) )
		{
			Rows                        =  new ObservableCollection <Row> ( ) ;
			Rows . CollectionChanged    += ColumnsRows_CollectionChanged ;
			Columns                     =  new ObservableCollection <Column> ( ) ;
			Columns . CollectionChanged += ColumnsRows_CollectionChanged ;
			ItemsAdded                  += Grid_ItemsAdded ;
			ItemsRemoved                += Grid_ItemsRemoved ;
		}

		public Grid ( ) : this ( null ) { }

		public Rectangle this [ Control control ]
		{
			get => new Rectangle ( StartAt [ control ] , BlockSize [ control ] ) ;
			set
			{
				StartAt [ control ]   = value . LeftTopPoint ;
				BlockSize [ control ] = value . Size ;
				RequestMeasure ( ) ;
			}
		}

		private void ColumnsRows_CollectionChanged ( object sender , NotifyCollectionChangedEventArgs e )
		{
			if ( ! ( e . OldItems is null ) )
			{
				List <INotifyPropertyChanged> removedItems =
					e . OldItems . Cast <INotifyPropertyChanged> ( ) .
						Where ( item => ! e . NewItems ? . Contains ( item ) ?? true ) .
						ToList ( ) ;

				foreach ( INotifyPropertyChanged item in removedItems )
				{
					item . PropertyChanged -= Item_PropertyChanged ;
				}
			}

			if ( ! ( e . NewItems is null ) )
			{
				List <INotifyPropertyChanged> newItems =
					e . NewItems . Cast <INotifyPropertyChanged> ( ) .
						Where ( item => ! e . OldItems ? . Contains ( item ) ?? true ) .
						ToList ( ) ;

				foreach ( INotifyPropertyChanged item in newItems )
				{
					item . PropertyChanged += Item_PropertyChanged ;
				}
			}

			RequestMeasure ( ) ;
		}

		private void Item_PropertyChanged ( object sender , PropertyChangedEventArgs e ) { RequestMeasure ( ) ; }

		private void Grid_ItemsRemoved ( object sender , ContainerControlEventArgs e )
		{
			StartAt . Remove ( e . Control ) ;
			BlockSize . Remove ( e . Control ) ;
		}

		private void Grid_ItemsAdded ( object sender , ContainerControlEventArgs e )
		{
			StartAt [ e . Control ]   = new Point ( 0 , 0 ) ;
			BlockSize [ e . Control ] = Size . One ;
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

			return Items . Where (
								  control
									  => StartAt [ control ] . X          == columnIndex
										 && BlockSize [ control ] . Width == 1 ) .
						   ToList ( ) ;
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

			return Items . Where (
								  control
									  => StartAt [ control ] . Y == rowIndex && BlockSize [ control ] . Height == 1 ) .
						   ToList ( ) ;
		}

		public override Size MeasureOverride ( Size availableSize )
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

            List<Column> staticSizeColumns = StaticSizeColumns;

            int staticWidth = 0;

			foreach ( Column column in staticSizeColumns )
			{
				column . DesiredWidth = column . Width ;
                staticWidth += column.DesiredWidth;
			}

            int autoWidth = 0;

            foreach ( Column column in AutoSizeColumns )
			{
				IReadOnlyCollection <Control> controls = GetInsideControls ( column ) ;

				int resultWidth = 0 ;

				if ( controls . Any ( ) )
				{
					foreach ( Control control in controls )
					{
						control . Measure ( new Size ( int . MaxValue , int . MaxValue ) ) ;
					}

					resultWidth = controls . Max ( control => control . DesiredSize . Width ) ;
				}

				column . DesiredWidth = resultWidth ;
                autoWidth += column.DesiredWidth;
			}

			List <Column> starSizeColumns = StarSizeColumns ;

            int starWidthSum = starSizeColumns.Sum((col) => col.Star);

            int minStarWidth = starSizeColumns . Min ( col => col . Star ) ;

			List <Column> minStarWidthColumn =
				starSizeColumns . Where ( col => col . Star == minStarWidth ) . ToList ( ) ;

			int maxMinStarWidth = 0 ;

			foreach ( Column column in minStarWidthColumn )
			{
				IReadOnlyCollection <Control> controls = GetInsideControls ( column ) ;

				int resultWidth = 0 ;

				if ( controls . Any ( ) )
				{
					foreach ( Control control in controls )
					{
						control . Measure ( new Size ( int . MaxValue , int . MaxValue ) ) ;
					}

					resultWidth = controls . Max ( control => control . DesiredSize . Width ) ;
				}

				maxMinStarWidth = Math . Max ( resultWidth , maxMinStarWidth ) ;
			}


			double singleStarWidth =Math.Min( maxMinStarWidth / ( double ) minStarWidth,(availableSize.Width-staticWidth-autoWidth)/(double) starWidthSum) ;

			foreach ( Column column in starSizeColumns )
			{
				column . DesiredWidth = ( int ) ( singleStarWidth * column . Star ) ;
			}

			#endregion

			#region Measure Rows

			List <Row> staticSizeRows = StaticSizeRows ;

            int staticHeight = 0;

			foreach ( Row row in staticSizeRows )
			{
				row . DesiredHeight = row . Height ;
                staticHeight += row.DesiredHeight;
            }

            int autoHeight = 0;

			foreach ( Row row in AutoSizeRows )
			{
				IReadOnlyCollection <Control> controls = GetInsideControls ( row ) ;

				int resultHeight = 0 ;

				if ( controls . Any ( ) )
				{
					foreach ( Control control in controls )
					{
						control . Measure ( new Size ( int . MaxValue , int . MaxValue ) ) ;
					}

					resultHeight = controls . Max ( control => control . DesiredSize . Height ) ;
				}

				row . DesiredHeight = resultHeight ;
                autoHeight += row.DesiredHeight;
			}

			List <Row> starSizeRows = StarSizeRows ;

            int starHeightSum = starSizeRows.Sum((col) => col.Star);

            int minStarHeight = starSizeRows . Min ( row => row . Star ) ;

			List <Row> minStarHeightRow = starSizeRows . Where ( row => row . Star == minStarHeight ) . ToList ( ) ;

			int maxMinStarHeight = 0 ;

			foreach ( Row row in minStarHeightRow )
			{
				IReadOnlyCollection <Control> controls = GetInsideControls ( row ) ;

				int resultHeight = 0 ;

				if ( controls . Any ( ) )
				{
					foreach ( Control control in controls )
					{
						control . Measure ( new Size ( int . MaxValue , int . MaxValue ) ) ;
					}

					resultHeight = controls . Max ( control => control . DesiredSize . Height ) ;
				}

				maxMinStarHeight = Math . Max ( resultHeight , maxMinStarHeight ) ;
			}

            double singleStarHeight = Math.Min(maxMinStarHeight / (double)minStarHeight, (availableSize.Height - staticHeight - autoHeight) / (double)starHeightSum);

            foreach ( Row row in starSizeRows )
			{
				row . DesiredHeight = ( int ) ( singleStarHeight * row . Star ) ;
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

			return new Size ( widthSum , heightSum ) ;
		}

		public override void ArrangeOverride ( Rectangle finalRect )
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

			RenderArea = finalRect ;
		}

		public class Row : INotifyPropertyChanged
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

					throw new InvalidOperationException ( ) ;
				}
			}

			public event PropertyChangedEventHandler PropertyChanged ;

			[NotifyPropertyChangedInvocator]
			protected virtual void OnPropertyChanged ( [CallerMemberName] string propertyName = null )
			{
				PropertyChanged ? . Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
			}

		}

		public class Column : INotifyPropertyChanged
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

					throw new InvalidOperationException ( ) ;
				}
			}

			public event PropertyChangedEventHandler PropertyChanged ;

			[NotifyPropertyChangedInvocator]
			protected virtual void OnPropertyChanged ( [CallerMemberName] string propertyName = null )
			{
				PropertyChanged ? . Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
			}

		}

	}

}
