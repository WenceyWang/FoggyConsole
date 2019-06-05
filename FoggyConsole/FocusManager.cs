using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

using DreamRecorder . FoggyConsole . Controls ;
using DreamRecorder . ToolBox . General ;

using Microsoft . Extensions . DependencyInjection ;
using Microsoft . Extensions . Logging ;

namespace DreamRecorder . FoggyConsole
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

		private ILogger Logger { get ; } =
			StaticServiceProvider . Provider . GetService <ILoggerFactory> ( ) . CreateLogger <FocusManager> ( ) ;


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
			Root           = root ?? throw new ArgumentNullException ( nameof ( root ) ) ;
			FocusedControl = GetControlList ( ) . FirstOrDefault ( ) ;
		}

		/// <summary>
		///     The currently focused control
		/// </summary>
		public Control FocusedControl
		{
			get => _focusedControl ;
			private set
			{
				if ( value != _focusedControl )
				{
					Frame . Current ? . PauseRedraw ( ) ;

					if ( _focusedControl != null )
					{
						_focusedControl . IsFocused = false ;
					}

					_focusedControl = value ;

					if ( _focusedControl != null )
					{
						_focusedControl . IsFocused = true ;
					}

					Frame . Current ? . ResumeRedraw ( ) ;
				}
			}
		}

		private List <Control> GetControlList ( )
		{
			return Root . GetAllItem ( ) .
						Where (
								control =>
								{
									if ( control is null )
									{
										Logger . LogWarning ( $"Control List of {Root . Name} contains null" ) ;
										return false ;
									}

									return control . CanFocus ;
								} ) .
						ToList ( ) ;
		}

		private void SortByUp ( List <Control> controls , Point center )
		{
			controls . Sort (
							( x , y ) =>
							{
								Point xCenter = x . RenderArea . Center ;
								Point yCenter = y . RenderArea . Center ;

								int xHorizontalDiff = Math . Abs ( xCenter . X - center . X ) ;
								int yHorizontalDiff = Math . Abs ( yCenter . X - center . X ) ;

								if ( xHorizontalDiff == yHorizontalDiff )

								{
									int xVerticalDiff = xCenter . Y - center . Y ;
									int yVerticalDiff = yCenter . Y - center . Y ;

									if ( ( xVerticalDiff   < 0 && yVerticalDiff < 0 )
										|| ( xVerticalDiff > 0 && yVerticalDiff > 0 ) )
									{
										return yVerticalDiff - xVerticalDiff ;
									}
									else
									{
										return xVerticalDiff - yVerticalDiff ;
									}
								}
								else
								{
									return xHorizontalDiff - yHorizontalDiff ;
								}
							} ) ;
		}


		private void SortByDown ( List <Control> controls , Point center )
		{
			controls . Sort (
							( x , y ) =>
							{
								Point xCenter = x . RenderArea . Center ;
								Point yCenter = y . RenderArea . Center ;

								int xHorizontalDiff = Math . Abs ( xCenter . X - center . X ) ;
								int yHorizontalDiff = Math . Abs ( yCenter . X - center . X ) ;

								if ( xHorizontalDiff == yHorizontalDiff )

								{
									int xVerticalDiff = xCenter . Y - center . Y ;
									int yVerticalDiff = yCenter . Y - center . Y ;

									if ( ( xVerticalDiff   < 0 && yVerticalDiff < 0 )
										|| ( xVerticalDiff > 0 && yVerticalDiff > 0 ) )
									{
										return xVerticalDiff - yVerticalDiff ;
									}
									else
									{
										return yVerticalDiff - xVerticalDiff ;
									}
								}
								else
								{
									return xHorizontalDiff - yHorizontalDiff ;
								}
							} ) ;
		}


		private void SortByRight ( List <Control> controls , Point center )
		{
			controls . Sort (
							( x , y ) =>
							{
								Point xCenter = x . RenderArea . Center ;
								Point yCenter = y . RenderArea . Center ;

								int xVerticalDiff = Math . Abs ( xCenter . Y - center . Y ) ;
								int yVerticalDiff = Math . Abs ( yCenter . Y - center . Y ) ;

								if ( xVerticalDiff == yVerticalDiff )

								{
									int xHorizontalDiff = xCenter . X - center . X ;
									int yHorizontalDiff = yCenter . X - center . X ;

									if ( ( xHorizontalDiff   < 0 && yHorizontalDiff < 0 )
										|| ( xHorizontalDiff > 0 && yHorizontalDiff > 0 ) )
									{
										return xHorizontalDiff - yHorizontalDiff ;
									}
									else
									{
										return yHorizontalDiff - xHorizontalDiff ;
									}
								}
								else
								{
									return xVerticalDiff - yVerticalDiff ;
								}
							} ) ;
		}

		private void SortByLeft ( List <Control> controls , Point center )
		{
			controls . Sort (
							( x , y ) =>
							{
								Point xCenter = x . RenderArea . Center ;
								Point yCenter = y . RenderArea . Center ;

								int xVerticalDiff = Math . Abs ( xCenter . Y - center . Y ) ;
								int yVerticalDiff = Math . Abs ( yCenter . Y - center . Y ) ;

								if ( xVerticalDiff == yVerticalDiff )

								{
									int xHorizontalDiff = xCenter . X - center . X ;
									int yHorizontalDiff = yCenter . X - center . X ;

									if ( ( xHorizontalDiff   < 0 && yHorizontalDiff < 0 )
										|| ( xHorizontalDiff > 0 && yHorizontalDiff > 0 ) )
									{
										return yHorizontalDiff - xHorizontalDiff ;
									}
									else
									{
										return xHorizontalDiff - yHorizontalDiff ;
									}
								}
								else
								{
									return xVerticalDiff - yVerticalDiff ;
								}
							} ) ;
		}

		/// <summary>
		///     Handles the key user input which is given in
		///     <paramref name="args" />
		/// </summary>
		/// <returns>true if the key-press was handled, otherwise false</returns>
		/// <param name="args">The key-press to handle</param>
		public void HandleKeyInput ( KeyPressedEventArgs args )
		{
			List <Control> controlList = GetControlList ( ) ;
			if ( ! controlList . Any ( ) )
			{
				return ;
			}

			if ( args . KeyInfo . Modifiers == ConsoleModifiers . Alt )
			{
				Control biddenControl = controlList . FirstOrDefault (
																	control =>
																	{
																		if ( control . KeyBind is null )
																		{
																			return false ;
																		}
																		else
																		{
																			return char . ToUpperInvariant (
																											control .
																												KeyBind .
																												Value )
																					== char . ToUpperInvariant (
																												args .
																													KeyInfo .
																													KeyChar ) ;
																		}
																	} ) ;


				if ( ! ( biddenControl is null ) )
				{
					FocusedControl = biddenControl ;
				}
			}
			else
			{
				switch ( args . KeyInfo . Key )
				{
					case ConsoleKey . Tab :
					{
						if ( args . KeyInfo . Modifiers == ConsoleModifiers . Shift )
						{
							args . Handled = true ;
							FocusedControl =
								controlList [ ( Math . Max ( controlList . IndexOf ( FocusedControl ) , 0 )
												+ controlList . Count
												- 1 )
											% controlList . Count ] ;
						}
						else
						{
							args . Handled = true ;
							FocusedControl =
								controlList [ ( Math . Max ( controlList . IndexOf ( FocusedControl ) , 0 ) + 1 )
											% controlList . Count ] ;
						}

						break ;
					}

					case ConsoleKey . RightArrow :
					{
						SortByRight ( controlList , FocusedControl . RenderArea . Center ) ;
						controlList . Remove ( FocusedControl ) ;
						FocusedControl = controlList . FirstOrDefault ( ) ?? FocusedControl ;
						break ;
					}

					case ConsoleKey . DownArrow :
					{
						SortByDown ( controlList , FocusedControl . RenderArea . Center ) ;
						controlList . Remove ( FocusedControl ) ;
						FocusedControl = controlList . FirstOrDefault ( ) ?? FocusedControl ;
						break ;
					}

					case ConsoleKey . UpArrow :
					{
						SortByUp ( controlList , FocusedControl . RenderArea . Center ) ;
						controlList . Remove ( FocusedControl ) ;
						FocusedControl = controlList . FirstOrDefault ( ) ?? FocusedControl ;
						break ;
					}

					case ConsoleKey . LeftArrow :
					{
						SortByLeft ( controlList , FocusedControl . RenderArea . Center ) ;
						controlList . Remove ( FocusedControl ) ;
						FocusedControl = controlList . FirstOrDefault ( ) ?? FocusedControl ;
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

}
