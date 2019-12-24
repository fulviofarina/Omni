using Rsx.Dumb;
using System;
using System.Collections.Generic;

namespace DB.Tools
{
    public partial class Creator
    {
        private const string NOTEPAD_APP = "notepad.exe";

        private static Interface Interface = null;

        private static EventHandler callBackLast = null;
        private static EventHandler callBackMain = null;

        public static IList<object> UserControls = new List<object>();

        // private static int toPopulate = 0;

        private static ILoader worker = null;
        /// <summary>
        /// disposes the worker that loads the data
        /// </summary>
    }
}