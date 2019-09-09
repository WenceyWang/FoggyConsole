using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using DreamRecorder.FoggyConsole.Controls;
using DreamRecorder.ToolBox.General;

namespace DreamRecorder.FoggyConsole.Example.Pages
{

    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ControlDisplayAttribute : Attribute
    {

    }

    public sealed class NewsPage : Page
    {

        public NewsPage() : base(
                                              XDocument.Parse(
                                                                 typeof(ControlDisplayPage).GetResourceFile(
                                                                                                                  $"{nameof(NewsPage)}.xml")).
                                                          Root)
        {
            
        } 

        public override void OnNavigateTo()
        {
           
        }

    }

    public sealed class ControlDisplayPage : Page
    {

        public Frame PageFrame { get; }

        public StackPanel PageList { get; }

        public ControlDisplayPage() : base(
                                              XDocument.Parse(
                                                                 typeof(ControlDisplayPage).GetResourceFile(
                                                                                                                  $"{nameof(ControlDisplayPage)}.xml")).
                                                          Root)
        {
            PageFrame = Find<Frame>(nameof(PageFrame));
            PageList = Find<StackPanel>(nameof(PageList));

            Pages = new List<Page>() { new NewsPage() };
        }

        public  List<Page> Pages { get; set; }

        public override void OnNavigateTo()
        {
            PageFrame.NavigateTo(Pages.First((page)=>page is NewsPage));

        }

    }

}
