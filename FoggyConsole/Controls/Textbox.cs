using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using DreamRecorder.FoggyConsole.Controls.Renderers;

namespace DreamRecorder.FoggyConsole.Controls
{

    /// <summary>
    ///     A control which provides single line editing and text input
    /// </summary>
    public class TextBox : TextualBase
    {

        private Point _cursorPosition;


        /// <summary>
        ///     The position of the cursor within the TextBox
        /// </summary>
        public Point CursorPosition
        {
            get => _cursorPosition;
            set
            {
                if (_cursorPosition!=value)
                {
                    _cursorPosition = value;
                    RequestRedraw();
                }
            }
        }

        public bool MultiLine { get; set; }

        public override bool CanFocusedOn => Enabled;

        /// <summary>
        ///     Creates a new TextBox
        /// </summary>
        /// <param name="renderer">
        ///     The
        ///     <code>ControlRenderer</code>
        ///     to use. If null a new instance of
        ///     <code>TextBoxRenderer</code>
        ///     will be used.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     Thrown if the
        ///     <code>ControlRenderer</code>
        ///     which should be set already has an other
        ///     Control assigned
        /// </exception>
        public TextBox(TextBoxRenderer renderer = null) : base(renderer ?? new TextBoxRenderer()) { }

        public TextBox() : this(null) { }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler EnterPressed;

        public override void KeyPressed(KeyPressedEventArgs args)
        {
            if (!Enabled)
            {
                return;
            }

            switch (args.KeyInfo.Key)
            {
                case ConsoleKey.Tab:
                case ConsoleKey.Escape:
                    {
                        break;
                    }

                case ConsoleKey.Enter:
                    {
                        args.Handled = true;
                        EnterPressed?.Invoke(this, EventArgs.Empty);
                        if (MultiLine)
                        {
                            Text = Text.Insert(CursorPosition, Environment.NewLine);
                            CursorPosition += Environment.NewLine.Length;
                        }
                        break;
                    }

                case ConsoleKey.RightArrow:
                    {
                        args.Handled = true;
                        if (CursorPosition < Text.Length)
                        {
                            CursorPosition++;
                        }

                        break;
                    }

                case ConsoleKey.LeftArrow:
                    {
                        args.Handled = true;
                        if (CursorPosition > 0)
                        {
                            CursorPosition--;
                        }

                        break;
                    }

                case ConsoleKey.Backspace:
                    {
                        args.Handled = true;
                        if (Text.Length != 0 && CursorPosition > 0)
                        {
                            Text = Text.Remove(CursorPosition - 1,1);
                            CursorPosition--;
                        }

                        break;
                    }

                default:
                    {
                        args.Handled = true;
                        char newChar = args.KeyInfo.KeyChar;
                        Text = Text.Insert(CursorPosition, new string(newChar, 1));
                        CursorPosition++;
                        break;
                    }
            }
        }

    }

}
