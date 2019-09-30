using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . FoggyConsole . Controls . Renderers
{

	/// <summary>
	///     Base class for all ControlDrawers
	/// </summary>
	public abstract class ControlRenderer <T> : IControlRenderer where T : Control
	{

		/// <summary>
		///     The Control which should be drawn
		/// </summary>
		private T _control ;

		/// <summary>
		///     The Control which should be drawn
		/// </summary>
		/// <exception cref="ArgumentException">Thrown if the Control which should be set already has an other renderer assigned</exception>
		public T Control
		{
			get => _control ;
			set
			{
				if ( value . Renderer    != null
					 && value . Renderer != this )
				{
					throw new ArgumentException (
												 $"{nameof ( Control )} already has a Renderer assigned" ,
												 nameof ( value ) ) ;
				}

				_control = value ;
			}
		}

		Control IControlRenderer . Control
		{
			get => Control ;
			set
				=> Control = value as T
							 ?? throw new ArgumentException (
															 $"{nameof ( Control )} has to be of {typeof ( T ) . Name}" )
			;
		}

		public virtual void Draw ( Application application , ConsoleArea area )
		{
			area . Fill ( Control . ActualBackgroundColor ) ;

			if ( Control . BoarderStyle is LineStyle boarderStyle )
			{
				area . DrawBoarder (
									boarderStyle ,
									Control . ActualForegroundColor ,
									Control . ActualBackgroundColor ) ;
			}

			if ( Control . ContentArea . IsNotEmpty ( ) )
			{
				DrawOverride ( application , area . CreateSub ( Control . ContentArea . Value ) ) ;
			}
		}

		/// <summary>
		///     Draws the Control stored in the Control-Property
		/// </summary>
		public abstract void DrawOverride ( Application application , ConsoleArea area ) ;

	}

}
