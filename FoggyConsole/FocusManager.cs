using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using DreamRecorder.ToolBox.General;
using Microsoft . Extensions . Logging ;
using Microsoft.Extensions.DependencyInjection;

using WenceyWang . FoggyConsole . Controls ;

namespace WenceyWang . FoggyConsole
{

	/// <summary>
	///     Basic
	///     <code>IFocusManager</code>
	///     which just cycles through all controls when the user presses TAB
	/// </summary>
	public class FocusManager : IFocusManager
	{

		private Control _focusedControl ;

		private Frame Root { get ; }


		public ILogger Logger { get ; } = StaticServiceProvider.Provider.GetService<ILoggerFactory>().CreateLogger<FocusManager>();


        /// <summary>
        ///     Creates a new FocusManager
        /// </summary>
        /// <param name="root">The control which represents</param>
        /// <exception cref="ArgumentException">
        ///     Is thrown if
        ///     <paramref name="root" />
        ///     has an container
        /// </exception>
        public FocusManager ( Frame root )
		{
			Root = root ?? throw new ArgumentNullException ( nameof(root) ) ;
		}

		/// <summary>
		///     The currently focused control
		/// </summary>
		public Control FocusedControl
		{
			get => _focusedControl ;
			private set
			{
				if ( _focusedControl != null )
				{
					_focusedControl . IsFocused = false ;
				}
				_focusedControl = value ;
				if ( _focusedControl != null )
				{
					_focusedControl . IsFocused = true ;
				}
			}
		}

		public void ControlTreeChanged ( ) { }

		/// <summary>
		///     Handles the key user input which is given in
		///     <paramref name="args" />
		/// </summary>
		/// <returns>true if the key-press was handled, otherwise false</returns>
		/// <param name="args">The key-press to handle</param>
		public void HandleKeyInput ( KeyPressedEventArgs args )
		{
			List <Control> controlList = Root . GetAllItem ( ) .
												Where ( control =>
														{
															if ( control == null )
															{
																Logger . LogWarning ( $"{nameof(controlList)} of {Root . Name} contains null" ) ;
																return false ;
															}

															return control . CanFocus ;
														} ) .
												ToList ( ) ;
			if ( !controlList . Any() )
			{
				return ;
			}

            if (args.KeyInfo.Modifiers==ConsoleModifiers.Alt)
            {
                
            }

			switch ( args . KeyInfo . Key )
			{
				case ConsoleKey . RightArrow :
				case ConsoleKey . DownArrow :
				case ConsoleKey . Tab :
				{
					args . Handled = true ;
					FocusedControl =
						controlList [ ( Math . Max ( controlList . IndexOf ( FocusedControl ) , 0 ) + 1 ) % controlList . Count ] ;
					break ;
				}
				case ConsoleKey . UpArrow :
				case ConsoleKey . LeftArrow :
				{
					args . Handled = true ;
					FocusedControl =
						controlList [ ( Math . Max ( controlList . IndexOf ( FocusedControl ) , 0 ) + controlList . Count - 1 )
									% controlList . Count ] ;
					break ;
				}

                    //{
                    //    bool up = args.KeyInfo.Key == ConsoleKey.UpArrow;
                    //    bool down = args.KeyInfo.Key == ConsoleKey.DownArrow;
                    //    bool left = args.KeyInfo.Key == ConsoleKey.LeftArrow;
                    //    bool right = args.KeyInfo.Key == ConsoleKey.RightArrow;

                    //    bool upDown = up || down;
                    //    bool leftRight = left || right;


                    //    int[] controls = GetNearbyControls(FocusedControl,
                    //                                            leftRight,
                    //                                            upDown)
                    //        .OrderBy(
                    //            i => upDown ? _controls[i].Renderer.Boundary.Top : _controls[i].Renderer.Boundary.Left * -1).
                    //        ToArray();

                    //    if (controls.Length == 0)
                    //    {
                    //        return true;
                    //    }

                    //    for (int i = 0; i < controls.Length; i++)
                    //    {
                    //        if (controls[i] == _focusedIndex)
                    //        {
                    //            if ((up || right) &&
                    //                i != 0)
                    //            {
                    //                SetFocusedIndex(controls[i - 1]);
                    //            }
                    //            if ((down || left) &&
                    //                i != controls.Length - 1)
                    //            {
                    //                SetFocusedIndex(controls[i + 1]);
                    //            }
                    //            break;
                    //        }
                    //    }
                    //    break;
                    //}
            }
		}

	}

}
