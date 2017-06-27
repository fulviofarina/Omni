using System;
using System.Data;

namespace DB.Tools
{
    public partial class Current
    {
        protected internal BS bs;
        protected internal Interface Interface;
        protected static string WINDOWS_USER = "WindowsUser";
        protected static string XML_EXT = ".xml";

        protected internal Func<DataRow, bool> selector
        {
            get
            {
                string label = WINDOWS_USER;
                return p => p.Field<string>(label).CompareTo(WindowsUser) == 0;
            }
        }
    }
}