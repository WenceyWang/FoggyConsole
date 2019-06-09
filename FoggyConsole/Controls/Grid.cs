using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Collections . ObjectModel ;
using System . Linq ;

using DreamRecorder . FoggyConsole . Controls . Renderers ;

namespace DreamRecorder . FoggyConsole . Controls
{

	public class Row
	{

		public int Height { get ; set ; }

		public bool Auto { get ; set ; }

		public int ActualHeight { get ; set ; }

	}

	public class Column
	{

		public int Width { get ; set ; }

		public bool Auto { get ; set ; }

		public int ActualWidth { get ; set ; }

	}

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


		public override bool CanFocus => false ;

		public Grid ( IControlRenderer renderer ) : base ( renderer ?? new GridRenderer ( ) ) { }


		public override void Measure ( Size availableSize )
		{
			int widthSum = 0 ;

			widthSum += StaticSizeColumns . Sum ( col => col . Width ) ;

			foreach ( Column column in AutoSizeColumns )
			{
			}

			foreach ( Control control in Items )
			{
			}

			base . Measure ( availableSize ) ;
		}

	}

}
