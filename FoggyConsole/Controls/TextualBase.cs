using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Collections . ObjectModel ;
using System . Linq ;

using DreamRecorder . FoggyConsole . Controls . Renderers ;

namespace DreamRecorder . FoggyConsole . Controls
{

	/// <summary>
	///     Base class for
	///     <code>Label</code>
	///     ,
	///     <code>Button</code>
	///     and
	///     <code>CheckBox</code>
	///     .
	///     A control which is able to display a single line of text.
	/// </summary>
	public abstract class TextualBase : Control
	{

		private string _text = string . Empty ;

		public ReadOnlyCollection <string> Lines { get ; private set ; }

		/// <summary>
		///     Gets or sets the text which is drawn onto this Control.
		/// </summary>
		public string Text
		{
			get => _text ;
			set
			{
				if ( value == null )
				{
					throw new ArgumentNullException ( nameof ( value ) ) ;
				}

				if ( _text != value )
				{
					_text = value ;
					TextChanged ? . Invoke ( this , EventArgs . Empty ) ;
					RequestMeasure ( ) ;
				}
			}
		}

		public override Size AutoDesiredSize => new Size ( Lines . Max ( str => str . Length ) , Lines . Count ) ;

        public override Size MeasureOverride(Size availableSize)
        {
            Size autoDesiredSize = AutoDesiredSize;

            if (availableSize.Width>=autoDesiredSize.Width)
            {
                return base.MeasureOverride(availableSize);
            }
            else
            {
                int requireHeight = Lines.Sum((line) => (line.Length / availableSize.Width) + 1);

                int width;
                if (AutoWidth)
                {
                    width = autoDesiredSize.Width;
                }
                else
                {
                    width = Size.Width;
                }

                int height;
                if (AutoHeight)
                {
                    height = Math.Max(requireHeight,autoDesiredSize.Height);
                }
                else
                {
                    height = Size.Height;
                }

                return new Size(width, height);
            }
        }

        protected TextualBase ( IControlRenderer renderer ) : base ( renderer )
			=> TextChanged += TextualBase_TextChanged ;

		public void UpdateLines ( )
		{
			Lines = new ReadOnlyCollection <string> (
													 Text . Split (
																   new [ ] { Environment . NewLine } ,
																   StringSplitOptions . None ) ) ;
		}

		private void TextualBase_TextChanged ( object sender , EventArgs e ) => UpdateLines ( ) ;

		public event EventHandler TextChanged ;

	}

}
