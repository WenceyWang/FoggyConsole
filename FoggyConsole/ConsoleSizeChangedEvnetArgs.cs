﻿using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;

namespace DreamRecorder . FoggyConsole
{

	public class ConsoleSizeChangedEvnetArgs : EventArgs
	{

		public Size NewSize { get ; set ; }

		public Size OldSize { get ; set ; }

	}

}
