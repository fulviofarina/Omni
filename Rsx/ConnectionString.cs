using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Smo.Wmi;
using Microsoft.Win32;

namespace Rsx
{

    public partial class SQL
    {
        public class ConnectionString
        {
            public string DBTag = "Initial Catalog";
            public string DB;
            public string Login;
            public string LoginTag = "UserID";
            public string PasswordTag = "Password";
            public string Password;
            public string SecurityInfoTag = "Persist Security Info";
            public string SecurityInfo = "True";
            public string ServerTag = "Data Source";
            public string Server;
            public string TimeoutTag = "Connect Timeout";
            public string Timeout = "10";

            public string WindowsIdentityTag = "Integrated Security";
            public string WindowsIdentityValue = "True";
            private IList<dynamic> boxes = null;

            // private string "Integrated Security = True"
            public void SetUI(ref IList<dynamic> Boxes)
            {
                boxes = Boxes;

              
            }

            public string GetUpdatedConnectionString

            {
                get
                {
                    string formed = string.Empty;

                    //use the tags
                    foreach (dynamic t in boxes)
                    {
                        if (string.IsNullOrEmpty(t.Text)) continue;

                        formed += t.Tag.ToString() + "=" + t.Text + ";";
                    }
                    //no login?? then use Windows Identity
                    if (!formed.Contains(LoginTag))
                    {
                        formed += WindowsIdentityTag + "=" + WindowsIdentityValue;
                    }
                    else formed = formed.Remove(formed.Length - 1, 1);

                    return formed;
                }
            }

            /// <summary>
            /// Makes a Connection String Structure, you can use the fields already
            /// </summary>
            /// <param name="connectionString"></param>
            public ConnectionString(string connectionString)
            {
                string[] hstring = connectionString.Split(';');

                string[] arr = hstring;

                Server = arr[0].Split('=')[1];

                // ServerTag = arr[0].Split('=')[0];

                DB = arr[1].Split('=')[1];

                // DBTag = arr[1].Split('=')[0];

                // SecurityInfoTag = arr[2].Split('=')[0];
                SecurityInfo = arr[2].Split('=')[1];

                int starter = 3;
                if (hstring.Count() == 6)
                {
                    Login = arr[starter].Split('=')[1];
                    Password = arr[starter + 1].Split('=')[1];
                    // LoginTag = arr[starter].Split('=')[0]; PasswordTag = arr[starter + 1].Split('=')[0];
                    starter += 2;
                }
                else
                {
                    WindowsIdentityValue = arr[starter +1].Split('=')[1];
                }
                Timeout = arr[starter].Split('=')[1];
                // TimeoutTag = arr[starter].Split('=')[0];
            }
        }

    }
  
}