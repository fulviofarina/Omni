using Rsx.Dumb;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using static DB.LINAA;
using static DB.Tools.XCOM.URIS;
namespace DB.Tools
{


   

        public partial class XCOM
        {

            public static class RSX
        {
            public static int MAXENERGIES = 75;
            public static string ENUME = "N";
            public static string DOT = ".";
            public static string COLON = ",";
            public static string DIVIDER = " - ";
            public static string HTML_EXT = ".html";

            public static string PIC_EXT = ".png";
            public static string ERROR = "Error";
        }


        public static class XCOMTXT
        {
          

            public static string CALCULATING_MSG = "Number of calculations pending ";
            public static string CALCULATING_TITLE = "Still calculating...";
            public static string XCOM_ERROR = "Problems comunicating with Server";
            public static string XCOM_TITLE = "The server did not answer the query";

            public static string NOMATRIX_ERROR = "No matrices were selected";
            public static string NOMATRIX_TITLE = "Nothing to do...";

            public static string INTERNET_MSG = "The Internet connection is functional";
            public static string INTERNET_TITLE = "Server available!";

            public static string NOINTERNET_ERROR = "Check your Internet connection";
            public static string NOINTERNET_TITLE = "Server not available!";
        }



        public static class URIS
        {

            public static string PIC_URI = "https://physics.nist.gov/PhysRefData/Xcom/tmp/graph";
            public static Uri XCOMTestUri = new Uri("https://physics.nist.gov/");
            public static Uri XCOMUri = new Uri("https://physics.nist.gov/cgi-bin/Xcom/xcom3_3-t");
            public static Uri XCOMUriPic = new Uri("https://physics.nist.gov/cgi-bin/Xcom/xcom3_3");

        }



     

    }

  

}