using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

using JetBrains.Annotations;

using static DreamRecorder.FoggyConsole.XtermConsole.EscapeSequences;

namespace DreamRecorder.FoggyConsole.XtermConsole
{

    public class XtermConsole : IConsole
    {

        private Size _internalSize = new Size(80, 24);

        private ConsoleColor _backgroundColor;

        private ConsoleColor _foregroundColor;

        private bool _cursorVisible;

        private string _title;

        public XtermConsole([NotNull] Stream stream)
        {
            Stream = stream ?? throw new ArgumentNullException(nameof(stream));
        }

        public Stream Stream { get; set; }

        public int RefreshLimit { get; set; } = 1;

        public bool IsRunning { get; private set; }

        public Thread WatcherThread { get; private set; }

        public Thread PullSizeThread { get; private set; }

        public void Bell()
        {
            lock (Stream)
            {
                using (StreamWriter writer = new StreamWriter(Stream, Encoding.UTF8, 1024, true))
                {
                    //CSI Ps = 8; height; width t
                    writer.Write(Bel);
                    writer.Flush();
                }
            }
        }

        public event EventHandler<ConsoleSizeChangedEvnetArgs> SizeChanged;

        public Size InternalSize
        {
            get => _internalSize;
            set
            {
                if (value != _internalSize)
                {
                    Size oldSize = _internalSize;

                    _internalSize = value;

                    SizeChanged?.Invoke(this,
                                                                new ConsoleSizeChangedEvnetArgs()
                                                                {
                                                                    NewSize = value,
                                                                    OldSize = oldSize
                                                                });
                }
            }
        }


        public Size Size
        {
            get => InternalSize;
            set
            {
                if (value != InternalSize)
                {
                    lock (Stream)
                    {
                        using (StreamWriter writer = new StreamWriter(Stream, Encoding.UTF8, 1024, true))
                        {
                            //CSI Ps = 8; height; width t
                            writer.Write(Csi);
                            writer.Write($"8;{value.Height};{value.Width}t");
                            writer.Flush();
                        }
                    }
                    _internalSize = value;
                }
            }
        }

        /// <summary>
        ///     Is fired when a user presses an key
        /// </summary>
        public event EventHandler<KeyPressedEventArgs> KeyPressed;

        public void Start()
        {
            if (!IsRunning)
            {
                if (!Application.IsDebug)
                {
                    CursorVisible = false;
                }

                if (Application.Name != null)
                {
                    Title = Application.Name;
                }

                if (Application.AutoDefaultColor)
                {
                    Application.DefaultBackgroundColor = BackgroundColor;
                    Application.DefaultForegroundColor = ForegroundColor;
                }

                WatcherThread = new Thread(Watch) { Name = nameof(WatcherThread) };
                PullSizeThread = new Thread(PullSize) { Name = nameof(PullSizeThread) };

                IsRunning = true;

                PullSizeThread.Start();
                WatcherThread.Start();
            }
        }


        public bool CursorVisible
        {
            get => _cursorVisible;
            set
            {
                if (_cursorVisible != value)
                {
                    lock (Stream)
                    {
                        using (StreamWriter writer = new StreamWriter(Stream, Encoding.UTF8, 1024, true))
                        {
                            //CSI Ps = 8; height; width t
                            /*writer.Write(Esc);*/
                            writer.Write(Csi);
                            writer.Write("?25");
                            if (value)
                            {
                                writer.Write("h");
                            }
                            else
                            {
                                writer.Write("l");
                            }
                            writer.Flush();
                        }
                    }

                    _cursorVisible = value;

                }
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    lock (Stream)
                    {
                        using (StreamWriter writer = new StreamWriter(Stream, Encoding.UTF8, 1024, true))
                        {
                            writer.Write(Osc);
                            writer.Write($"2;{value}");
                            writer.Write(St);

                            writer.Flush();
                        }
                    }
                    _title = value;
                }

            }
        }

        public ConsoleColor BackgroundColor
        {
            get => _backgroundColor;
            set
            {
                if (value != _backgroundColor)
                {
                    lock (Stream)
                    {
                        using (StreamWriter writer = new StreamWriter(Stream, Encoding.UTF8, 1024, true))
                        {
                            //CSI Pm m  Character Attributes (SGR).
                            writer.Write(Csi);
                            writer.Write($"[{value.BackgroundColorToCode()}m");
                            writer.Flush();
                        }
                    }
                    _backgroundColor = value;
                }
            }
        }

        public ConsoleColor ForegroundColor
        {
            get => _foregroundColor;
            set
            {
                if (value != _foregroundColor)
                {
                    lock (Stream)
                    {
                        using (StreamWriter writer = new StreamWriter(Stream, Encoding.UTF8, 1024, true))
                        {
                            //CSI Pm m  Character Attributes (SGR).
                            writer.Write(Csi);
                            writer.Write($"{value.ForegroundColorToCode()}m");
                            writer.Flush();
                        }
                    }
                    _foregroundColor = value;
                }
            }
        }

        /// <summary>
        ///     Stops the internal watcher-thread
        /// </summary>
        public void Stop()
        {
            IsRunning = false;

            WatcherThread.Abort();

            ResetColor();
            SetCursorPosition(0, 0);
            Clear();
            CursorVisible = true;
        }

        private void ResetColor()
        {
            lock (Stream)
            {
                using (StreamWriter writer = new StreamWriter(Stream, Encoding.UTF8, 1024, true))
                {
                    //CSI Ps J Erase in Display(ED), VT100.
                    //	Ps = 0->Erase Below(default).
                    //	Ps = 1->Erase Above.
                    //	Ps = 2->Erase All.
                    //	Ps = 3->Erase Saved Lines(xterm).
                    /*writer.Write(Esc);*/
                    writer.Write(Csi);
                    writer.Write($"3J");
                    writer.Flush();
                }
            }
        }

        private void Clear()
        {
            lock (Stream)
            {
                using (StreamWriter writer = new StreamWriter(Stream, Encoding.UTF8, 1024, true))
                {
                    //CSI Ps J Erase in Display(ED), VT100.
                    //	Ps = 0->Erase Below(default).
                    //	Ps = 1->Erase Above.
                    //	Ps = 2->Erase All.
                    //	Ps = 3->Erase Saved Lines(xterm).
                    /*writer.Write(Esc);*/
                    writer.Write(Csi);
                    writer.Write($"3J");
                    writer.Flush();
                }
            }
        }

        private void SetCursorPosition(int x, int y)
        {
            lock (Stream)
            {
                using (StreamWriter writer = new StreamWriter(Stream, Encoding.UTF8, 1024, true))
                {
                    //CSI Ps; Ps H
                    //Cursor Position[row; column] (default =  [1, 1])(CUP)  
                    /*writer.Write(Esc);*/
                    writer.Write(Csi);
                    writer.Write($"{y + 1};{x + 1}H");
                    writer.Flush();
                }
            }
        }

        public void Draw(Point position, ConsoleArea area)
        {
            Draw(new Rectangle(position, area.Size), area.Content);
        }

        public Application Application { get; set; }

        private void PullSize()
        {
            DateTime lastUpdate = DateTime.Now + TimeSpan.FromMilliseconds(1000d / RefreshLimit);

            TimeSpan waitTime = lastUpdate - DateTime.Now;
            lastUpdate = DateTime.Now + TimeSpan.FromMilliseconds(1000d / RefreshLimit);

            while (IsRunning)
            {
                lock (Stream)
                {
                    ////CSI 1 8 t
                    using (StreamWriter writer = new StreamWriter(Stream, Encoding.UTF8, 1024, true))
                    {
                        writer.Write(Csi);
                        writer.Write("18t");
                        writer.Flush();
                    }
                }

                Thread.Yield();
                Thread.Sleep(Math.Max(Convert.ToInt32(waitTime.TotalMilliseconds), 0));

            }
        }


		public List<IInputEscapeSequenceParser> Parsers { get ; set ; }

        private void Watch()
        {
            StreamReader reader;
            lock (Stream)
            {
                reader = new StreamReader(Stream, Encoding.UTF8, false, 1024, true);
            }

            List<char> buffer = new List<char>();

            while (IsRunning)
            {
                int currentChar;

                currentChar = reader.Read();

                currentChar = Stream.ReadByte();

                if (currentChar == -1)
                {
                    return;
                }

                buffer.Add((char)currentChar);

               

            }


        }

        public void Draw(Rectangle position, ConsoleChar[,] content)
        {
            try
            {
                Rectangle consoleArea = new Rectangle(new Point(), Size);

                position = Rectangle.Intersect(position, consoleArea);

                StringBuilder stringBuilder = new StringBuilder(content.Length);

                for (int y = 0; y < position.Height; y++)
                {
                    SetCursorPosition(position.Left, position.Top + y);

                    for (int x = Math.Max(-position.X, 0); x < position.Width; x++)
                    {
                        ConsoleChar currentPosition = content[x, y];

                        ConsoleColor targetBackgroundColor = currentPosition.BackgroundColor;
                        ConsoleColor targetForegroundColor = currentPosition.ForegroundColor;

                        if (BackgroundColor != targetBackgroundColor
                            || ForegroundColor != targetForegroundColor
                            && !char.IsWhiteSpace(currentPosition.Character))
                        {
                            Write(stringBuilder);

                            BackgroundColor = targetBackgroundColor;
                            ForegroundColor = targetForegroundColor;
                        }

                        stringBuilder.Append(currentPosition.Character);
                    }

                    Write(stringBuilder);
                }

                SetCursorPosition(position.Left, position.Top);
            }

            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
                //Todo: Warning
            }
        }

        private void Write(StringBuilder stringBuilder)
        {
            if (stringBuilder.Length > 0)
            {
                lock (Stream)
                {
                    using (StreamWriter writer = new StreamWriter(Stream, Encoding.UTF8, stringBuilder.Length, true))
                    {
                        writer.Write(stringBuilder.ToString());
                        writer.Flush();
                    }
                }
                stringBuilder.Clear();
            }
        }

    }

    public enum ParseResult
    {
        CanNotEstablish,
        Established,
        Finished,
    }

    public interface IInputEscapeSequenceParser
    {

        ParseResult TryParse(List<char> content);

        void Apply(List<char> content, XtermConsole console);

    }

    public class InputEscapeSequenceParser : IInputEscapeSequenceParser
    {
        public Regex TryEscape = new Regex(@"^\u001b(?:\[|$)(?:8|$)(?:;|$)(?:(\d+)|$)(?:;|$)(?:(\d+)|$)(?:m|$)");

        public Regex FullEscape = new Regex(@"^\u001b\[8;(\d+);(\d+)m");

        public ParseResult TryParse(List<char> content)
        {
            string contentString = new string(content.ToArray());

            Match tryMatch = TryEscape.Match(contentString);

            if (tryMatch.Success)
            {
                Match fullMatch = FullEscape.Match(contentString);

                if (fullMatch.Success)
                {
                    return ParseResult.Finished;
                }
                else
                {
                    return ParseResult.Established;
                }

            }
            else
            {
                return ParseResult.CanNotEstablish;
            }
        }

        public void Apply(List<char> content, XtermConsole console)
        {
            string contentString = new string(content.ToArray());

            Match match = FullEscape.Match(contentString);

            if (match.Success)
            {
                int height = Convert.ToInt32(match.Groups[1].Value);
                int width = Convert.ToInt32(match.Groups[2].Value);

                console.InternalSize = new Size(width, height);

                content.RemoveRange(0, match.Value.Length);
            }

        }

    }

}