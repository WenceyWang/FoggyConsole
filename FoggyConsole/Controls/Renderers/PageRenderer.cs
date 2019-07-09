﻿using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . FoggyConsole . Controls . Renderers
{

	public class ContentControlRenderer : ControlRenderer <ContentControl>
	{

		public override void Draw ( ConsoleArea area ) { Control . Content ? . Draw ( area ) ; }

	}


	public class PageRenderer : ContentControlRenderer <Page>
	{

	}

}
