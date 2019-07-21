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

		public static FocusManager Current { get ; private set ; }

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
			Current = this ;
			Root    = root ?? throw new ArgumentNullException ( nameof ( root ) ) ;

			_focusedControl = GetControlList ( ) . FirstOrDefault ( ) ;
			if ( _focusedControl != null )
			{
				_focusedControl . IsFocused = true ;
			}
		}

		/// <summary>
		///     The currently focused control
		/// </summary>
		public Control FocusedControl
		{
			get => _focusedControl ;
			set
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

					Logger . LogTrace ( $"Focused: {value ? . Name ?? "null"}" ) ;
				}
			}
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
				Control bondedControl = controlList . FirstOrDefault (
																	control =>
																	{
																		if ( control . KeyBind is null )
																		{
																			return false ;
																		}

																		return char . ToUpperInvariant (
																										control .
																											KeyBind .
																											Value )
																				== char . ToUpperInvariant (
																											args .
																												KeyInfo .
																												KeyChar ) ;
																	} ) ;


				if ( ! ( bondedControl is null ) )
				{
					FocusedControl = bondedControl ;
				}
			}
			else
			{
				//float tangentControl = 0.2f;
				//float yAxisCorrection = 2.3f;

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

					default :
					{
						Rectangle focusedArea = FocusedControl ? . RenderArea . GetValueOrDefault ( ) ?? default ;

						switch ( args . KeyInfo . Key )
						{
							case ConsoleKey . UpArrow :
							{
								controlList . Remove ( FocusedControl ) ;
								controlList . RemoveAll (
														control
															=> control . RenderArea ? . FloatCenter . Y
																>= FocusedControl ? . RenderArea ? . FloatCenter . Y ) ;

								controlList . Sort (
													ComparisonExtensions . Select <Control , int
																			> (
																				control => Math . Abs (
																										focusedArea .
																											MinColumnDiff (
																															control .
																																RenderArea .
																																GetValueOrDefault ( ) ) ) ) .
																			Union (
																					ComparisonExtensions .
																						Select <Control , int> (
																												control
																													=> Math .
																														Abs (
																															focusedArea .
																																MinRowDiff (
																																			control .
																																				RenderArea .
																																				GetValueOrDefault ( ) ) ) ) ) .
																			Union (
																					ComparisonExtensions .
																						Select <Control , int> (
																												control
																													=> focusedArea .
																														MaxColumnDiff (
																																		control .
																																			RenderArea .
																																			GetValueOrDefault ( ) ) ) ) .
																			Union (
																					ComparisonExtensions .
																						Select <Control , int> (
																												control
																													=> focusedArea .
																														MaxRowDiff (
																																	control .
																																		RenderArea .
																																		GetValueOrDefault ( ) ) ) ) .
																			Union (
																					ComparisonExtensions .
																						Select <Control , float> (
																												control
																													=> ( focusedArea .
																															FloatCenter
																														- control .
																														RenderArea .
																														GetValueOrDefault ( ) .
																														FloatCenter
																														) .
																													LengthSquared ( ) ) ) ) ;

								Control newFocusedControl = controlList . FirstOrDefault ( ) ?? FocusedControl ;

								if ( newFocusedControl != FocusedControl )
								{
									FocusedControl = newFocusedControl ;
									args . Handled = true ;
								}

								break ;
							}

							case ConsoleKey . DownArrow :
							{
								controlList . Remove ( FocusedControl ) ;
								controlList . RemoveAll (
														control
															=> control . RenderArea ? . FloatCenter . Y
																<= FocusedControl ? . RenderArea ? . FloatCenter . Y ) ;

								controlList . Sort (
													ComparisonExtensions . Select <Control , int
																			> (
																				control => Math . Abs (
																										focusedArea .
																											MinColumnDiff (
																															control .
																																RenderArea .
																																GetValueOrDefault ( ) ) ) ) .
																			Union (
																					ComparisonExtensions .
																						Select <Control , int> (
																												control
																													=> Math .
																														Abs (
																															focusedArea .
																																MinRowDiff (
																																			control .
																																				RenderArea .
																																				GetValueOrDefault ( ) ) ) ) ) .
																			Union (
																					ComparisonExtensions .
																						Select <Control , int> (
																												control
																													=> focusedArea .
																														MaxColumnDiff (
																																		control .
																																			RenderArea .
																																			GetValueOrDefault ( ) ) ) ) .
																			Union (
																					ComparisonExtensions .
																						Select <Control , int> (
																												control
																													=> focusedArea .
																														MaxRowDiff (
																																	control .
																																		RenderArea .
																																		GetValueOrDefault ( ) ) ) ) .
																			Union (
																					ComparisonExtensions .
																						Select <Control , float> (
																												control
																													=> ( focusedArea .
																															FloatCenter
																														- control .
																														RenderArea .
																														GetValueOrDefault ( ) .
																														FloatCenter
																														) .
																													LengthSquared ( ) ) ) ) ;

								Control newFocusedControl = controlList . FirstOrDefault ( ) ?? FocusedControl ;

								if ( newFocusedControl != FocusedControl )
								{
									FocusedControl = newFocusedControl ;
									args . Handled = true ;
								}

								break ;
							}

							case ConsoleKey . LeftArrow :
							{
								controlList . Remove ( FocusedControl ) ;
								controlList . RemoveAll (
														control
															=> control . RenderArea ? . FloatCenter . X
																>= FocusedControl ? . RenderArea ? . FloatCenter . X ) ;

								controlList . Sort (
													ComparisonExtensions . Select <Control , int
																			> (
																				control => Math . Abs (
																										focusedArea .
																											MinRowDiff (
																														control .
																															RenderArea .
																															GetValueOrDefault ( ) ) ) ) .
																			Union (
																					ComparisonExtensions .
																						Select <Control , int> (
																												control
																													=> Math .
																														Abs (
																															focusedArea .
																																MinColumnDiff (
																																				control .
																																					RenderArea .
																																					GetValueOrDefault ( ) ) ) ) ) .
																			Union (
																					ComparisonExtensions .
																						Select <Control , int> (
																												control
																													=> focusedArea .
																														MaxRowDiff (
																																	control .
																																		RenderArea .
																																		GetValueOrDefault ( ) ) ) ) .
																			Union (
																					ComparisonExtensions .
																						Select <Control , int> (
																												control
																													=> focusedArea .
																														MaxColumnDiff (
																																		control .
																																			RenderArea .
																																			GetValueOrDefault ( ) ) ) ) .
																			Union (
																					ComparisonExtensions .
																						Select <Control , float> (
																												control
																													=> ( focusedArea .
																															FloatCenter
																														- control .
																														RenderArea .
																														GetValueOrDefault ( ) .
																														FloatCenter
																														) .
																													LengthSquared ( ) ) ) ) ;

								Control newFocusedControl = controlList . FirstOrDefault ( ) ?? FocusedControl ;

								if ( newFocusedControl != FocusedControl )
								{
									FocusedControl = newFocusedControl ;
									args . Handled = true ;
								}

								break ;
							}

							case ConsoleKey . RightArrow :
							{
								controlList . Remove ( FocusedControl ) ;
								controlList . RemoveAll (
														control
															=> control . RenderArea ? . FloatCenter . X
																<= FocusedControl ? . RenderArea ? . FloatCenter . X ) ;
								controlList . Sort (
													ComparisonExtensions . Select <Control , int
																			> (
																				control => Math . Abs (
																										focusedArea .
																											MinRowDiff (
																														control .
																															RenderArea .
																															GetValueOrDefault ( ) ) ) ) .
																			Union (
																					ComparisonExtensions .
																						Select <Control , int> (
																												control
																													=> Math .
																														Abs (
																															focusedArea .
																																MinColumnDiff (
																																				control .
																																					RenderArea .
																																					GetValueOrDefault ( ) ) ) ) ) .
																			Union (
																					ComparisonExtensions .
																						Select <Control , int> (
																												control
																													=> focusedArea .
																														MaxRowDiff (
																																	control .
																																		RenderArea .
																																		GetValueOrDefault ( ) ) ) ) .
																			Union (
																					ComparisonExtensions .
																						Select <Control , int> (
																												control
																													=> focusedArea .
																														MaxColumnDiff (
																																		control .
																																			RenderArea .
																																			GetValueOrDefault ( ) ) ) ) .
																			Union (
																					ComparisonExtensions .
																						Select <Control , float> (
																												control
																													=> ( focusedArea .
																															FloatCenter
																														- control .
																														RenderArea .
																														GetValueOrDefault ( ) .
																														FloatCenter
																														) .
																													LengthSquared ( ) ) ) ) ;

								Control newFocusedControl = controlList . FirstOrDefault ( ) ?? FocusedControl ;

								if ( newFocusedControl != FocusedControl )
								{
									FocusedControl = newFocusedControl ;
									args . Handled = true ;
								}

								break ;
							}
						}

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

									return control . RenderArea . IsNotEmpty ( ) && control . CanFocusedOn ;
								} ) .
						ToList ( ) ;
		}

	}

}
