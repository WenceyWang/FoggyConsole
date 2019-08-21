using System;

namespace DreamRecorder.FoggyConsole
{
    public interface IConsole:IApplicationItem
    {
        void Start();

        void Stop();

        void Draw(Point position, ConsoleArea area);

        Size Size { get; set; }

        event EventHandler<ConsoleSizeChangedEvnetArgs> SizeChanged;

        void Bell();

        event EventHandler<KeyPressedEventArgs> KeyPressed;
    }

}