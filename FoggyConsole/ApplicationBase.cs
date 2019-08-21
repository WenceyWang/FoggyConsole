using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DreamRecorder.FoggyConsole.Controls;
using DreamRecorder.ToolBox.CommandLine;

using Microsoft.Extensions.Logging;

namespace DreamRecorder.FoggyConsole
{
    /// <summary>
    ///     The actual Application.
    ///     It contains a
    ///     <code>RootControl</code>
    ///     in which all other
    ///     <code>Control</code>
    ///     instances are stored in a
    ///     tree-format.
    ///     It also manages user input and drawing.
    /// </summary>
    public abstract class ApplicationBase
    {

        /// <summary>
        ///     The root of the Control-Tree
        /// </summary>
        public RootFrame ViewRoot { get; set; }

        ///// <summary>
        /////     Used as the boundary for the ViewRoot if the terminal-size can't determined
        ///// </summary>
        //public static Size StandardRootBoundary { get ; } = new Size ( 80 , 24 ) ;

        /// <summary>
        ///     Responsible for focus-changes, for example when the user presses the TAB-key
        /// </summary>
        public FocusManager FocusManager { get; set; }

        public KeyBindingManager KeyBindingManager { get; set; }

        public IConsole Console { get; set; }

        /// <summary>
        ///     The name of this application
        /// </summary>
        public virtual string Name { get; set; }

        public virtual bool AutoDefaultColor => false;

        public bool IsDebug { get; set; }

        public abstract Frame PrepareViewRoot();

        /// <summary>
        ///     Starts this
        ///     <code>ApplicationBase</code>
        ///     .
        /// </summary>
        public void Start()
        {

            Console.KeyPressed += KeyWatcherOnKeyPressed;
            Console.Start();

            ViewRoot.Enabled = true;
            ViewRoot.ResumeRedraw();
        }

        /// <summary>
        ///     Stops this
        ///     <code>ApplicationBase</code>
        ///     .
        /// </summary>
        public void Stop()
        {
            if (ViewRoot!= null)
            {
                ViewRoot.Enabled = false;
            }

            Console.KeyPressed -= KeyWatcherOnKeyPressed;
            Console.Stop();
        }


        

        private void KeyWatcherOnKeyPressed(object sender, KeyPressedEventArgs eventArgs)
        {
            KeyBindingManager?.HandleKey(eventArgs);

            if (!eventArgs.Handled)
            {
                IHandleKeyInput currentHandler = FocusManager?.FocusedControl;
                currentHandler?.HandleKeyInput(eventArgs);

                if (!eventArgs.Handled)
                {
                    FocusManager?.HandleKeyInput(eventArgs);
                }
            }
        }

    }

}
