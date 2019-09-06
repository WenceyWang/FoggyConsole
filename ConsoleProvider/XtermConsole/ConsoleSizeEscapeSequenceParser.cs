﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DreamRecorder.FoggyConsole.XtermConsole
{

    public class KeyEscapeSequenceParser : IInputEscapeSequenceParser
    {

        public Regex FullEscape = new Regex(@"^\u001b\[(A|B|C|D|H|F|3~|2~|B|6~|D|E|C|A|5~)");

        public Regex TryEscape = new Regex(@"^\u001b(?:\[|$)(A|B|C|D|H|F|3~|2~|B|6~|D|E|C|A|5~|$)");

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
                switch (match.Groups[1].Value)
                {
                    case "A":
                        {
                            console.InvokeKeyPressed(new ConsoleKeyInfo(default, ConsoleKey.UpArrow, false, false, false));
                            break;
                        }
                    case "B":
                        {
                            console.InvokeKeyPressed(new ConsoleKeyInfo(default, ConsoleKey.DownArrow, false, false, false));
                            break;
                        }
                    case "C":
                        {
                            console.InvokeKeyPressed(new ConsoleKeyInfo(default, ConsoleKey.RightArrow, false, false, false));
                            break;
                        }
                    case "D":
                        {
                            console.InvokeKeyPressed(new ConsoleKeyInfo(default, ConsoleKey.LeftArrow, false, false, false));
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }

                content.RemoveRange(0, match.Value.Length);
            }
        }

    }


    public class ConsoleSizeEscapeSequenceParser : IInputEscapeSequenceParser
    {

        public Regex FullEscape = new Regex(@"^\u001b\[8;(\d+);(\d+)t");

        public Regex TryEscape = new Regex(@"^\u001b(?:\[|$)(?:8|$)(?:;|$)(?:(\d+)|$)(?:;|$)(?:(\d+)|$)(?:t|$)");

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
