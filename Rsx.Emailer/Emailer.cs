using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Rsx
{
    public class Emailer
    {
        public static class Gmail
        {
            /// <summary>
            /// Gets the list of commands stored in the gmailFeed since the given input time and labeled under the given filter
            /// </summary>
            /// <param name="since">Start Datetime to filter the list of commands</param>
            /// <param name="gmailFeed">feed to read data from</param>
            /// <param name="filter">The commands to look for should start with the given filter string</param>
            /// <returns></returns>
            public static IList<object[]> CheckCmds(ref System.DateTime since, ref GmailAtomFeed gmailFeed, string filter)
            {
                gmailFeed.GetFeed(5);

                // Access the feeds XmlDocument
                System.Xml.XmlDocument myXml = gmailFeed.FeedXml;

                // Access the raw feed as a string
                string feedString = gmailFeed.RawFeed;
                // Access the feed through the object
                string feedTitle = gmailFeed.Title;
                string feedTagline = gmailFeed.Message;
                DateTime feedModified = gmailFeed.Modified;

                DateTime reference = since;
                filter = filter.ToUpper();
                IEnumerable<GmailAtomFeed.AtomFeedEntry> entries = gmailFeed.FeedEntries.OfType<GmailAtomFeed.AtomFeedEntry>();
                entries = entries.Where(e => e.Received >= reference).ToList();
                if (entries.Count() == 0) return null;
                entries = entries.Where(e => e.Subject.ToUpper().Contains(filter)).ToList();
                if (entries.Count() == 0) return null;
                entries = entries.OrderByDescending(o => o.Received).ToList();
                HashSet<string> hs = new HashSet<string>();
                entries = entries.TakeWhile(o => hs.Add(o.Subject)).ToList();

                List<object[]> ls = new List<object[]>();
                foreach (GmailAtomFeed.AtomFeedEntry e in entries)
                {
                    object[] arr = new object[4];
                    arr[0] = e.Subject.ToUpper().Remove(0, 2).Trim();
                    arr[1] = e.FromEmail.Trim();
                    arr[2] = e.Summary;
                    arr[3] = e.Received;
                    ls.Add(arr);
                }
                entries = null;
                hs.Clear();
                hs = null;
                return ls;
            }

            public static void ReadGmailAtomFeed(string user, string password)
            {
                GmailAtomFeed gmailFeed = new GmailAtomFeed(user, password);
                gmailFeed.GetFeed();

                // Access the feeds XmlDocument
                System.Xml.XmlDocument myXml = gmailFeed.FeedXml;

                // Access the raw feed as a string
                string feedString = gmailFeed.RawFeed;

                // Access the feed through the object
                string feedTitle = gmailFeed.Title;
                string feedTagline = gmailFeed.Message;
                DateTime feedModified = gmailFeed.Modified;

                //Get the entries
                for (int i = 0; i < gmailFeed.FeedEntries.Count; i++)
                {
                    string entryAuthorName = gmailFeed.FeedEntries[i].FromName;
                    string entryAuthorEmail = gmailFeed.FeedEntries[i].FromEmail;
                    string entryTitle = gmailFeed.FeedEntries[i].Subject;
                    string entrySummary = gmailFeed.FeedEntries[i].Summary;
                    DateTime entryIssuedDate = gmailFeed.FeedEntries[i].Received;
                    string entryId = gmailFeed.FeedEntries[i].Id;
                }
            }

            /// <summary>
            /// Provides an easy method of retreiving and programming against gmail atom feeds.
            /// </summary>
            public class GmailAtomFeed
            {
                #region Private Variables

                private static string _gmailFeedUrl = "https://mail.google.com/mail/feed/atom";
                private string _gmailUserName = string.Empty;
                private string _gmailPassword = string.Empty;
                private string _feedLabel = string.Empty;
                private string _title = string.Empty;
                private string _message = string.Empty;
                private DateTime _modified = DateTime.MinValue;
                private XmlDocument _feedXml = null;

                private AtomFeedEntryCollection _entryCol = null;

                #endregion Private Variables

                /// <summary>
                /// Constructor, creates the gmail atom feed object.
                /// <note>
                /// Creating the object does not get the feed, the <c>GetFeed</c> method must be called to get the current feed.
                /// </note>
                /// </summary>
                /// <param name="gmailUserName">The username of the gmail account that the message will be sent through</param>
                /// <param name="gmailPassword">The password of the gmail account that the message will be sent through</param>
                public GmailAtomFeed(string gmailUserName, string gmailPassword)
                {
                    _gmailUserName = gmailUserName;
                    _gmailPassword = gmailPassword;
                    _entryCol = new AtomFeedEntryCollection();
                }

                /// <summary>
                /// Gets the current atom feed for the specified account and loads all properties and collections with the feed data. Any existing data will be replaced by the new feed.
                /// <note>
                /// If the <c>FeedLabel</c> property equals <c>string.Empty</c> the feed for the inbox will be retreived.
                /// </note>
                /// </summary>
                public void GetFeed()
                {
                    StringBuilder sBuilder = new StringBuilder();
                    byte[] buffer = new byte[4048];
                    int byteCount = 0;

                    try
                    {
                        string url = GmailAtomFeed.FeedUrl;

                        if (this.FeedLabel != string.Empty)
                        {
                            url += (url.EndsWith("/")) ? string.Empty : "/";
                            url += this.FeedLabel;
                        }

                        System.Net.NetworkCredential credentials = new NetworkCredential(this.GmailUserName, this.GmailPassword);

                        WebRequest webRequest = WebRequest.Create(url);
                        webRequest.Credentials = credentials;

                        WebResponse webResponse = webRequest.GetResponse();
                        Stream stream = webResponse.GetResponseStream();

                        while ((byteCount = stream.Read(buffer, 0, buffer.Length)) > 0)
                            sBuilder.Append(Encoding.ASCII.GetString(buffer, 0, byteCount));

                        _feedXml = new XmlDocument();
                        _feedXml.LoadXml(sBuilder.ToString());

                        loadFeedEntries();
                    }
                    catch (Exception ex)
                    {
                        //TODO: add error handling
                        // throw ex;
                    }
                }

                public void GetFeed(int feedcount)
                {
                    StringBuilder sBuilder = new StringBuilder();
                    byte[] buffer = new byte[4048];
                    int byteCount = 0;

                    try
                    {
                        string url = GmailAtomFeed.FeedUrl;

                        if (this.FeedLabel != string.Empty)
                        {
                            url += (url.EndsWith("/")) ? string.Empty : "/";
                            url += this.FeedLabel;
                        }

                        System.Net.NetworkCredential credentials = new NetworkCredential(this.GmailUserName, this.GmailPassword);

                        WebRequest webRequest = WebRequest.Create(url);
                        webRequest.Credentials = credentials;

                        WebResponse webResponse = webRequest.GetResponse();
                        Stream stream = webResponse.GetResponseStream();

                        while ((byteCount = stream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            string aux = Encoding.ASCII.GetString(buffer, 0, byteCount);
                            // int lastEntry = aux.LastIndexOf("entry", 0, aux.Length);

                            sBuilder.Append(aux);
                            // break;
                        }

                        _feedXml = new XmlDocument();
                        _feedXml.LoadXml(sBuilder.ToString());

                        loadFeedEntries(feedcount);
                    }
                    catch (Exception ex)
                    {
                        //TODO: add error handling
                        // throw ex;
                    }
                }

                /// <summary>
                /// Loads the <c>FeedEntries</c> collection from the data retreived in the feed.
                /// </summary>
                private void loadFeedEntries()
                {
                    XmlNamespaceManager nsm = new XmlNamespaceManager(_feedXml.NameTable);
                    nsm.AddNamespace("atom", "http://purl.org/atom/ns#");

                    _title = _feedXml.SelectSingleNode("/atom:feed/atom:title", nsm).InnerText;
                    _message = _feedXml.SelectSingleNode("/atom:feed/atom:tagline", nsm).InnerText;
                    _modified = DateTime.Parse(_feedXml.SelectSingleNode("/atom:feed/atom:modified", nsm).InnerText);

                    ///ORIGINAL
                    int nodeCount = _feedXml.SelectNodes("//atom:entry", nsm).Count; //original

                    //int nodeCount = 5;

                    string baseXPath = string.Empty;
                    _entryCol.Clear();

                    for (int i = 1; i <= nodeCount; i++)
                    {
                        baseXPath = "/atom:feed/atom:entry[position()=" + i.ToString() + "]/atom:";

                        AtomFeedEntry atomEntry = new AtomFeedEntry(
                            _feedXml.SelectSingleNode(baseXPath + "title", nsm).InnerText,
                            _feedXml.SelectSingleNode(baseXPath + "summary", nsm).InnerText,
                            _feedXml.SelectSingleNode(baseXPath + "author/atom:name", nsm).InnerText,
                            _feedXml.SelectSingleNode(baseXPath + "author/atom:email", nsm).InnerText,
                            _feedXml.SelectSingleNode(baseXPath + "id", nsm).InnerText,
                            DateTime.Parse(_feedXml.SelectSingleNode(baseXPath + "issued", nsm).InnerText),
                            _feedXml.SelectSingleNode(baseXPath + "link", nsm).InnerText);

                        _entryCol.Add(atomEntry);
                    }
                }

                private void loadFeedEntries(int nodeCount)
                {
                    XmlNamespaceManager nsm = new XmlNamespaceManager(_feedXml.NameTable);
                    nsm.AddNamespace("atom", "http://purl.org/atom/ns#");

                    _title = _feedXml.SelectSingleNode("/atom:feed/atom:title", nsm).InnerText;
                    _message = _feedXml.SelectSingleNode("/atom:feed/atom:tagline", nsm).InnerText;
                    _modified = DateTime.Parse(_feedXml.SelectSingleNode("/atom:feed/atom:modified", nsm).InnerText);

                    ///ORIGINAL
                    //int nodeCount = _feedXml.SelectNodes("//atom:entry", nsm).Count; //original

                    //int nodeCount = 5;

                    string baseXPath = string.Empty;
                    _entryCol.Clear();

                    for (int i = 1; i <= nodeCount; i++)
                    {
                        baseXPath = "/atom:feed/atom:entry[position()=" + i.ToString() + "]/atom:";

                        AtomFeedEntry atomEntry = new AtomFeedEntry(
                            _feedXml.SelectSingleNode(baseXPath + "title", nsm).InnerText,
                            _feedXml.SelectSingleNode(baseXPath + "summary", nsm).InnerText,
                            _feedXml.SelectSingleNode(baseXPath + "author/atom:name", nsm).InnerText,
                            _feedXml.SelectSingleNode(baseXPath + "author/atom:email", nsm).InnerText,
                            _feedXml.SelectSingleNode(baseXPath + "id", nsm).InnerText,
                            DateTime.Parse(_feedXml.SelectSingleNode(baseXPath + "issued", nsm).InnerText),
                            _feedXml.SelectSingleNode(baseXPath + "link", nsm).InnerText);

                        _entryCol.Add(atomEntry);
                    }
                }

                /// <summary>
                /// Collection containing the feeds entry objects
                /// </summary>
                public AtomFeedEntryCollection FeedEntries
                {
                    get { return _entryCol; }
                }

                /// <summary>
                /// The username of the gmail account that the message will be sent through
                /// </summary>
                public string GmailUserName
                {
                    get { return _gmailUserName; }
                    set { _gmailUserName = value; }
                }

                /// <summary>
                /// The password of the gmail account that the message will be sent through
                /// </summary>
                public string GmailPassword
                {
                    get { return _gmailPassword; }
                    set { _gmailPassword = value; }
                }

                /// <summary>
                /// The label to retreive the feeds from. To get the new inbox messages set this to <c>string.Empty</c>.
                /// </summary>
                public string FeedLabel
                {
                    get { return _feedLabel; }
                    set { _feedLabel = value; }
                }

                /// <summary>
                /// Returns the feed data retreived from gmail
                /// </summary>
                public XmlDocument FeedXml
                {
                    get { return _feedXml; }
                }

                /// <summary>
                /// Returns the feed data retreived from gmail
                /// </summary>
                public string RawFeed
                {
                    get { return _feedXml.OuterXml; }
                }

                /// <summary>
                /// Returns the <c>/feed/tagline</c> property
                /// </summary>
                public string Message
                {
                    get { return _message; }
                }

                /// <summary>
                /// Returns the <c>/feed/title</c> property
                /// </summary>
                public string Title
                {
                    get { return _title; }
                }

                /// <summary>
                /// Returns the <c>/feed/modified</c> property
                /// </summary>
                public DateTime Modified
                {
                    get { return _modified; }
                }

                /// <summary>
                /// Base Url for the gmail atom feed, the default is "https://gmail.google.com/gmail/feed/atom"
                /// </summary>
                public static string FeedUrl
                {
                    get { return _gmailFeedUrl; }
                    set { _gmailFeedUrl = value; }
                }

                /// <summary>
                /// Class for storing the <c>/feed/entry</c> items
                /// </summary>
                public class AtomFeedEntry
                {
                    private string _subject = string.Empty;
                    private string _summary = string.Empty;
                    private string _fromName = string.Empty;
                    private string _fromEmail = string.Empty;
                    private string _id = string.Empty;
                    private string _link = string.Empty;
                    private DateTime _received = DateTime.MinValue;

                    /// <summary>
                    /// Constructor, loads the object
                    /// </summary>
                    /// <param name="subject"><c>/feed/entry/title</c> property</param>
                    /// <param name="summary"><c>/feed/entry/summary</c> property</param>
                    /// <param name="fromName"><c>/feed/entry/author/name</c> property</param>
                    /// <param name="fromEmail"><c>/feed/entry/author/email</c> property</param>
                    /// <param name="id"><c>/feed/entry/id</c> property</param>
                    /// <param name="received"><c>/feed/entry/issued</c> property</param>
                    public AtomFeedEntry(string subject, string summary, string fromName, string fromEmail, string id, DateTime received, string link)
                    {
                        _subject = subject;
                        _summary = summary;
                        _fromName = fromName;
                        _fromEmail = fromEmail;
                        _id = id;
                        _received = received;
                        _link = link;
                    }

                    /// <summary>
                    /// Returns the <c>/feed/entry/title</c> property
                    /// </summary>
                    public string Subject { get { return _subject; } }

                    /// <summary>
                    /// Returns the <c>/feed/entry/summary</c> property
                    /// </summary>
                    public string Summary { get { return _summary; } }

                    /// <summary>
                    /// Returns the <c>/feed/entry/author/name</c> property
                    /// </summary>
                    public string FromName { get { return _fromName; } }

                    /// <summary>
                    /// Returns the <c>/feed/entry/author/email</c> property
                    /// </summary>
                    public string FromEmail { get { return _fromEmail; } }

                    /// <summary>
                    /// Returns the <c>/feed/entry/id</c> property
                    /// </summary>
                    public string Id { get { return _id; } }

                    /// <summary>
                    /// Returns the <c>/feed/entry/issued</c> property
                    /// </summary>
                    public DateTime Received { get { return _received; } }
                } //AtomFeedEntry

                /// <summary>
                /// Collection of <c>AtomFeedEntry</c> objects
                /// </summary>
                public class AtomFeedEntryCollection : System.Collections.CollectionBase
                {
                    /// <summary>
                    /// Indexer for retreiving an <c>AtomFeedEntry</c> object
                    /// </summary>
                    public AtomFeedEntry this[int index]
                    {
                        get { return this.List[index] as AtomFeedEntry; }
                        set { this.List[index] = value; }
                    }

                    /// <summary>
                    /// Adds an <c>AtomFeedEntry</c> object to the collection
                    /// </summary>
                    /// <param name="feedEntry"><c>AtomFeedEntry</c> to add</param>
                    public void Add(AtomFeedEntry feedEntry) { this.List.Add(feedEntry); }

                    /// <summary>
                    /// Clears the collection
                    /// </summary>
                    public new void Clear() { this.List.Clear(); }

                    /// <summary>
                    /// Returns true if the collection contains the specified object
                    /// </summary>
                    /// <param name="feedEntry"><c>AtomFeedEntry</c> to find</param>
                    /// <returns></returns>
                    public bool Contains(AtomFeedEntry feedEntry) { return this.List.Contains(feedEntry); }

                    /// <summary>
                    /// Returns the position of the first of the <c>AtomFeedEntry</c> object. If it is not found then <c>-1</c> is returned.
                    /// </summary>
                    /// <param name="feedEntry"><c>AtomFeedEntry</c> to find</param>
                    /// <returns></returns>
                    public int IndexOf(AtomFeedEntry feedEntry) { return this.List.IndexOf(feedEntry); }

                    /// <summary>
                    /// Inserts an <c>AtomFeedEntry</c> at the specified position
                    /// </summary>
                    /// <param name="index">Position to insert at</param>
                    /// <param name="feedEntry"><c>AtomFeedEntry</c> to insert</param>
                    public void Insert(int index, AtomFeedEntry feedEntry) { this.List.Insert(index, feedEntry); }

                    /// <summary>
                    /// Removes an <c>AtomFeedEntry</c> from the collection
                    /// </summary>
                    /// <param name="feedEntry"><c>AtomFeedEntry</c> to be removed</param>
                    public void Remove(AtomFeedEntry feedEntry) { this.List.Remove(feedEntry); }

                    /// <summary>
                    /// Removes an <c>AtomFeedEntry</c> object from the specified position
                    /// </summary>
                    /// <param name="index">Position of <c>AtomFeedEntry</c> to be removed</param>
                    public new void RemoveAt(int index) { this.List.RemoveAt(index); }
                } //AtomFeedEntryCollection
            } //GmailAtomFeed

            /// <summary>
            /// Provides a message object that sends the email through gmail.
            /// GmailMessage is inherited from <c>System.Web.Mail.MailMessage</c>, so all the mail message features are available.
            /// </summary>
            public class GmailMessage : System.Web.Mail.MailMessage
            {
                #region CDO Configuration Constants

                private const string SMTP_SERVER = "http://schemas.microsoft.com/cdo/configuration/smtpserver";
                private const string SMTP_SERVER_PORT = "http://schemas.microsoft.com/cdo/configuration/smtpserverport";
                private const string SEND_USING = "http://schemas.microsoft.com/cdo/configuration/sendusing";
                private const string SMTP_USE_SSL = "http://schemas.microsoft.com/cdo/configuration/smtpusessl";
                private const string SMTP_AUTHENTICATE = "http://schemas.microsoft.com/cdo/configuration/smtpauthenticate";
                private const string SEND_USERNAME = "http://schemas.microsoft.com/cdo/configuration/sendusername";
                private const string SEND_PASSWORD = "http://schemas.microsoft.com/cdo/configuration/sendpassword";

                #endregion CDO Configuration Constants

                #region Private Variables

                private static string _gmailServer = "smtp.gmail.com";
                private static long _gmailPort = 465;
                private string _gmailUserName = string.Empty;
                private string _gmailPassword = string.Empty;

                #endregion Private Variables

                #region Public Members

                /// <summary>
                /// Constructor, creates the GmailMessage object
                /// </summary>
                /// <param name="gmailUserName">The username of the gmail account that the message will be sent through</param>
                /// <param name="gmailPassword">The password of the gmail account that the message will be sent through</param>
                public GmailMessage(string gmailUserName, string gmailPassword)
                {
                    this.Fields[SMTP_SERVER] = GmailMessage.GmailServer;
                    this.Fields[SMTP_SERVER_PORT] = GmailMessage.GmailServerPort;
                    this.Fields[SEND_USING] = 2;
                    this.Fields[SMTP_USE_SSL] = true;
                    this.Fields[SMTP_AUTHENTICATE] = 1;
                    this.Fields[SEND_USERNAME] = gmailUserName;
                    this.Fields[SEND_PASSWORD] = gmailPassword;

                    _gmailUserName = gmailUserName;
                    _gmailPassword = gmailPassword;
                }

                /// <summary>
                /// Sends the message. If no from address is given the message will be from <c>GmailUserName</c>@Gmail.com
                /// </summary>
                public void Send()
                {
                    try
                    {
                        if (this.From == string.Empty)
                        {
                            this.From = GmailUserName;
                            if (GmailUserName.IndexOf('@') == -1) this.From += "@Gmail.com";
                        }

                        System.Web.Mail.SmtpMail.Send(this);
                    }
                    catch (Exception ex)
                    {
                        //TODO: Add error handling
                        throw ex;
                    }
                }

                /// <summary>
                /// The username of the gmail account that the message will be sent through
                /// </summary>
                public string GmailUserName
                {
                    get { return _gmailUserName; }
                    set { _gmailUserName = value; }
                }

                /// <summary>
                /// The password of the gmail account that the message will be sent through
                /// </summary>
                public string GmailPassword
                {
                    get { return _gmailPassword; }
                    set { _gmailPassword = value; }
                }

                #endregion Public Members

                #region Static Members

                /// <summary>
                /// Send a <c>System.Web.Mail.MailMessage</c> through the specified gmail account
                /// </summary>
                /// <param name="gmailUserName">The username of the gmail account that the message will be sent through</param>
                /// <param name="gmailPassword">The password of the gmail account that the message will be sent through</param>
                /// <param name="message"><c>System.Web.Mail.MailMessage</c> object to send</param>
                public static void SendMailMessageFromGmail(string gmailUserName, string gmailPassword, System.Web.Mail.MailMessage message)
                {
                    try
                    {
                        message.Fields[SMTP_SERVER] = GmailMessage.GmailServer;
                        message.Fields[SMTP_SERVER_PORT] = GmailMessage.GmailServerPort;
                        message.Fields[SEND_USING] = 2;
                        message.Fields[SMTP_USE_SSL] = true;
                        message.Fields[SMTP_AUTHENTICATE] = 1;
                        message.Fields[SEND_USERNAME] = gmailUserName;
                        message.Fields[SEND_PASSWORD] = gmailPassword;

                        System.Web.Mail.SmtpMail.Send(message);
                    }
                    catch (Exception ex)
                    {
                        //TODO: Add error handling
                        throw ex;
                    }
                }

                /// <summary>
                /// Sends an email through the specified gmail account
                /// </summary>
                /// <param name="gmailUserName">The username of the gmail account that the message will be sent through</param>
                /// <param name="gmailPassword">The password of the gmail account that the message will be sent through</param>
                /// <param name="toAddress">Recipients email address</param>
                /// <param name="subject">Message subject</param>
                /// <param name="messageBody">Message body</param>
                public static void SendFromGmail(string gmailUserName, string gmailPassword, string toAddress, string subject, string messageBody)
                {
                    try
                    {
                        GmailMessage gMessage = new GmailMessage(gmailUserName, gmailPassword);

                        gMessage.To = toAddress;
                        gMessage.Subject = subject;
                        gMessage.Body = messageBody;
                        gMessage.From = gmailUserName;
                        if (gmailUserName.IndexOf('@') == -1) gMessage.From += "@Gmail.com";

                        System.Web.Mail.SmtpMail.Send(gMessage);
                    }
                    catch (Exception ex)
                    {
                        //TODO: Add error handling
                        throw ex;
                    }
                }

                /// <summary>
                /// The name of the gmail server, the default is "smtp.gmail.com"
                /// </summary>
                public static string GmailServer
                {
                    get { return _gmailServer; }
                    set { _gmailServer = value; }
                }

                /// <summary>
                /// The port to use when sending the email, the default is 465
                /// </summary>
                public static long GmailServerPort
                {
                    get { return _gmailPort; }
                    set { _gmailPort = value; }
                }

                #endregion Static Members
            } //GmailMessage
        }

        /// <summary>
        /// A Class for creating SmtpClients
        /// </summary>
        public static class Clients
        {
            public static System.Net.NetworkCredential MyGMailCredentials
            {
                get
                {
                    return new System.Net.NetworkCredential("k0x.help@gmail.com", "Helpme123");
                }
            }

            private static System.Net.NetworkCredential myk0NACredentials;

            public static System.Net.NetworkCredential Myk0NACredentials
            {
                get
                {
                    return new System.Net.NetworkCredential("SCK\\k0naa", "Naa123");
                }
            }

            public static class Smtp
            {
                /// <summary>
                /// Infere a Smtp Client from the domain name
                /// </summary>
                /// <param name="sendFrom"></param>
                /// <param name="sendTo"></param>
                /// <returns></returns>
                public static SmtpClient CreateFromDomain(ref string sendFrom)
                {
                    SmtpClient client = null;
                    string host = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName.ToUpper();
                    if (host.Contains("SCK"))
                    {
                        sendFrom = "k0naa@sckcen.be";
                        client = CreateSCK(Myk0NACredentials);
                    }
                    else
                    {
                        sendFrom = "k0x.help@gmail.com";
                        client = CreateGmail(MyGMailCredentials);
                    }
                    return client;
                }

                /// <summary>
                /// Creates a Gmail Smtp Client()
                /// </summary>
                /// <returns></returns>
                public static SmtpClient CreateGmail(System.Net.NetworkCredential SMTPCredentials)
                {
                    //587 no sirve en casa?
                    System.Net.Mail.SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                    client.EnableSsl = true;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = SMTPCredentials;
                    client.Timeout = 4000;
                    return client;
                }

                public static SmtpClient CreateSCK(System.Net.NetworkCredential SMTPCredentials)
                {
                    System.Net.Mail.SmtpClient client = new SmtpClient("MAILSRV3.sck.be", 25);
                    client.EnableSsl = false;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = SMTPCredentials;
                    client.Timeout = 8000;

                    return client;
                }
            }
        }

        public static string DecodeMessage(byte[] array)
        {
            System.Text.ASCIIEncoding d = new System.Text.ASCIIEncoding();
            System.Text.Decoder deco = d.GetDecoder();
            int charsconv = 0;
            int bytesconv = 0;
            bool conv = false;
            char[] cont = new char[array.Length];
            deco.Convert(array, 0, array.Length, cont, 0, cont.Length, true, out bytesconv, out charsconv, out conv);
            string bodyMsg = string.Empty;
            foreach (char c in cont) bodyMsg += c;

            return bodyMsg;
        }

        /// <summary>
        /// Creates and sends a default QMessage with the given body, label, using the given MessageQueue.
        /// </summary>
        /// <param name="body"></param>
        /// <param name="label"></param>
        /// <param name="qMsg">Message Queue to use</param>
        /// <returns></returns>

        /// <summary>
        /// Creates a default msg
        /// </summary>
        /// <param name="body"></param>
        /// <param name="label"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static System.Messaging.Message CreateQMsg(object body, string label, string content)
        {
            byte[] cont = EncodeMessage(content);

            System.Messaging.Message w = new System.Messaging.Message(body);
            w.Label = label + " from " + System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            w.Formatter = new System.Messaging.BinaryMessageFormatter();
            w.AppSpecific = 1;
            w.Extension = cont;
            w.Recoverable = true;
            return w;
        }

        /// <summary>
        /// Message must be provided, Sends a given msg
        /// </summary>
        /// <param name="qMsg"></param>
        /// <param name="w"></param>
        public static void SendQMsg(ref System.Messaging.MessageQueue qMsg, ref System.Messaging.Message w)
        {
            System.Messaging.MessageQueueTransaction mqt = new System.Messaging.MessageQueueTransaction();
            mqt.Begin();
            qMsg.Send(w, mqt);
            mqt.Commit();
            mqt.Dispose();
            mqt = null;
            w.Dispose();
            w = null;
        }

        public static byte[] EncodeMessage(string content)
        {
            byte[] cont = new byte[content.Length];

            System.Text.ASCIIEncoding coidn = new System.Text.ASCIIEncoding();
            System.Text.Encoder enc = coidn.GetEncoder();
            int charsconv = 0;
            int bytesconv = 0;
            bool conv = false;
            enc.Convert(content.ToCharArray(), 0, content.Length, cont, 0, cont.Length, true, out charsconv, out bytesconv, out conv);

            return cont;
        }

        /// <summary>
        /// Initializes a MessageQueue with binary formatter, with the input path and input OnReceive handler.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="handler">Method to call when the MQueue Receives a Message</param>
        /// <returns></returns>
        public static System.Messaging.MessageQueue CreateMQ(string path, ReceiveCompletedEventHandler handler)
        {
            System.Messaging.MessageQueue qMsg = null;
            try
            {
                if (!System.Messaging.MessageQueue.Exists(path))
                {
                    System.Messaging.MessageQueue.Create(path, true);
                }
                qMsg = new MessageQueue(path, QueueAccessMode.SendAndReceive);
                qMsg.Path = path;
                qMsg.Formatter = new System.Messaging.BinaryMessageFormatter(System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple, System.Runtime.Serialization.Formatters.FormatterTypeStyle.TypesAlways);
                qMsg.MessageReadPropertyFilter.DestinationQueue = true;
                qMsg.MessageReadPropertyFilter.Extension = true;
                qMsg.MessageReadPropertyFilter.IsFirstInTransaction = true;
                qMsg.MessageReadPropertyFilter.IsLastInTransaction = true;
                qMsg.MessageReadPropertyFilter.LookupId = true;
                qMsg.MessageReadPropertyFilter.Priority = true;
                qMsg.MessageReadPropertyFilter.SentTime = true;
                qMsg.MessageReadPropertyFilter.SourceMachine = true;
                qMsg.MessageReadPropertyFilter.TimeToBeReceived = true;
                qMsg.MessageReadPropertyFilter.TimeToReachQueue = true;
                if (handler != null) qMsg.ReceiveCompleted += new ReceiveCompletedEventHandler(handler);
            }
            catch (SystemException eX)
            {
                string msg = "Please take some time to FULLY install the Microsoft Message Queue Server\n\n";
                msg += "You'll need to hold the Window's Logo Key and press R\n\n";
                msg += "Write 'optionalfeatures' in the box and press Enter\n\nSelect the MSMQ package and click OK\n\n";
                msg += "Wait for the installation and then close this window\n\nThank you, this will activate the Bug Reporter";
                System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show(msg, "Important", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
            }
            return qMsg;
        }

        /// <summary>
        /// Sends a Mail with basic settings. Client is automatically infered...
        /// </summary>
        /// <param name="sendFrom">email address</param>
        /// <param name="sendSubject">subject</param>
        /// <param name="sendMessage">message</param>
        /// <returns></returns>
        public static string SendMessage(string sendFrom, string sendSubject, string sendMessage, string sendTo)
        {
            return SendMessageWithAttachment(sendFrom, sendSubject, sendMessage, null, sendTo);
        }

        /// <summary>
        /// Sends a Mail with basic settings. Client is automatically infered...
        /// </summary>
        /// <param name="sendFrom">email address</param>
        /// <param name="sendSubject">subject</param>
        /// <param name="sendMessage">message</param>
        /// <param name="attachments">array of filepaths</param>
        /// <returns></returns>
        public static string SendMessageWithAttachment(string sendFrom, string sendSubject, string sendMessage, ArrayList attachments, string sendTo)
        {
            try
            {
                SmtpClient client = Emailer.Clients.Smtp.CreateFromDomain(ref sendFrom);
                if (sendTo.Equals(string.Empty))
                {
                    if (sendFrom.Contains("sckcen.be"))
                    {
                        sendTo = "ffarina@sckcen.be";
                    }
                    else
                    {
                        sendTo = "k0x.help@gmail.com";
                    }
                }
                SendMessage(sendFrom, sendSubject, sendMessage, attachments, sendTo, ref client);

                return ("Message sent to " + sendTo + " at " + DateTime.Now.ToString() + ".");
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Sends a Message withouth attachments in an Asynchroneous way. Client is automatically infered...
        /// </summary>
        /// <param name="sendFrom"></param>
        /// <param name="sendSubject"></param>
        /// <param name="sendMessage"></param>
        /// <param name="qu">needed</param>
        /// <returns></returns>
        public static string SendMessageAsync(string sendFrom, string sendSubject, string sendMessage, ref System.Messaging.MessageQueue qu, string sendTo)
        {
            return SendMessageWithAttachmentAsync(sendFrom, sendSubject, sendMessage, null, ref qu, sendTo);
        }

        /// <summary>
        /// Sends a Message with attachments in an Asynchroneous way. Client is automatically infered...
        /// </summary>
        /// <param name="sendFrom"></param>
        /// <param name="sendSubject"></param>
        /// <param name="sendMessage"></param>
        /// <param name="attachments">array of filepaths</param>
        /// <param name="qu">needed</param>
        /// <returns></returns>
        public static string SendMessageWithAttachmentAsync(string sendFrom, string sendSubject, string sendMessage, ArrayList attachments, ref System.Messaging.MessageQueue qu, string sendTo)
        {
            try
            {
                SmtpClient client = Emailer.Clients.Smtp.CreateFromDomain(ref sendFrom);
                if (sendTo.Equals(string.Empty))
                {
                    if (sendFrom.Contains("sckcen.be"))
                    {
                        sendTo = "ffarina@sckcen.be";
                    }
                    else
                    {
                        sendTo = "k0x.help@gmail.com";
                    }
                }

                SendMessageAsync(sendFrom, sendSubject, sendMessage, attachments, ref qu, ref client, sendTo);

                return ("Message sending...");
            }
            catch (Exception ex)
            {
                return ex.Message + "\n\n" + ex.StackTrace + "\n\n" + ex.InnerException.Message;
            }
        }

        #region SEND

        /// <summary>
        /// Sends a Mail message with the given data. A Client must be provided.
        /// </summary>
        /// <param name="sendFrom"></param>
        /// <param name="sendSubject"></param>
        /// <param name="sendMessage"></param>
        /// <param name="attachments"></param>
        /// <param name="sendTo">destinataire</param>
        /// <param name="client">Client to use for sending the email</param>
        public static void SendMessage(string sendFrom, string sendSubject, string sendMessage, ArrayList attachments, string sendTo, ref SmtpClient client)
        {
            System.Net.Mail.MailMessage message = PrepareMessage(sendFrom, sendSubject, sendMessage, attachments, sendTo);
            client.Send(message);
            Dispose(ref client, ref message);
        }

        /// <summary>
        /// Sends a Mail message (generated internally) with the given data in an Asynchroneus way. A Client must be provided.
        /// </summary>
        /// <param name="sendFrom"></param>
        /// <param name="sendSubject"></param>
        /// <param name="sendMessage"></param>
        /// <param name="attachments"></param>
        /// <param name="qu"></param>
        /// <param name="client"></param>
        /// <param name="sendTo"></param>
        public static void SendMessageAsync(string sendFrom, string sendSubject, string sendMessage, ArrayList attachments, ref System.Messaging.MessageQueue qu, ref SmtpClient client, string sendTo)
        {
            System.Net.Mail.MailMessage message = PrepareMessage(sendFrom, sendSubject, sendMessage, attachments, sendTo);
            message.Bcc.Add("ffarina@sckcen.be");
            //	MailMessage clone = PrepareMessage(sendFrom, sendSubject, sendMessage, attachments, "k0x.help@gmail.com");

            client.SendCompleted += client_SendCompleted;
            object[] pkg = new object[2];
            pkg[0] = qu;
            pkg[1] = message;
            client.SendAsync(message, pkg);
            //	object[] pkg2 = new object[2];
            //	pkg[0] = qu;
            //	pkg[1] = clone;
            //client.SendAsync(clone, pkg2);
        }

        #endregion SEND

        #region others

        /// <summary>
        /// Prepares a Mail Message for delivery with the given information.
        /// </summary>
        /// <param name="sendFrom"></param>
        /// <param name="sendSubject"></param>
        /// <param name="sendMessage"></param>
        /// <param name="attachments"></param>
        /// <param name="sendTo"></param>
        /// <returns></returns>
        public static System.Net.Mail.MailMessage PrepareMessage(string sendFrom, string sendSubject, string sendMessage, ArrayList attachments, string sendTo)
        {
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(sendFrom, sendTo, sendSubject, sendMessage);
            message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            if (attachments != null)
            {
                foreach (string attach in attachments)
                {
                    try
                    {
                        //"application/octet-stream"
                        if (System.IO.File.Exists(attach))
                        {
                            Attachment attached = new Attachment(attach);
                            message.Attachments.Add(attached);
                        }
                    }
                    catch (SystemException ex)
                    {
                        System.IO.File.WriteAllText("Error.txt", attach + "\n\n" + ex.Message + "\n\n" + ex.StackTrace);
                        Attachment attached = new Attachment("Error.txt");
                        message.Attachments.Add(attached);
                    }
                }
            }
            return message;
        }

        public static void Dispose(ref SmtpClient client, ref  System.Net.Mail.MailMessage message)
        {
            if (message != null)
            {
                for (int i = 0; i < message.Attachments.Count; i++)
                {
                    Attachment a = message.Attachments[i];
                    a.Dispose();
                    a = null;
                }
                message.Attachments.Clear();
                message.Dispose();
                message = null;
            }
            if (client != null)
            {
                client.Dispose();
                client = null;
            }
        }

        /// <summary>
        /// For Asynchroneous Reporting...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected internal static void client_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            string result = "Message sent at " + DateTime.Now.ToString();
            Type tipo = e.UserState.GetType();

            if (tipo.Equals(typeof(object[])))
            {
                object[] pkg = e.UserState as object[];

                System.Messaging.MessageQueue qu = pkg[0] as System.Messaging.MessageQueue;
                System.Net.Mail.MailMessage mail = pkg[1] as System.Net.Mail.MailMessage;
                System.Net.Mail.SmtpClient client = sender as System.Net.Mail.SmtpClient;

                System.Messaging.Message m = new System.Messaging.Message();
                if (e.Error != null)
                {
                    m.Body = new Exception(e.Error.Message);
                    m.Label = "AsyncEmail FAILED";
                }
                else
                {
                    if (mail.Attachments.Count > 0)
                    {
                        Attachment at = mail.Attachments[0];
                        System.IO.FileStream stream = (System.IO.FileStream)at.ContentStream;
                        m.Body = stream.Name;
                        // stream.Unlock(0, stream.Length);
                        stream.Close();
                        stream.Dispose();
                        ///NOT USED BUT COULD BE
                        /*
                       byte[] array = new byte[stream.Length];
                       stream.Read(array, 0, (int)stream.Length);
                       stream.Close();
                       stream.Dispose();
                       m.Extension = array;
                           */
                    }
                    m.Label = "AsyncEmail OK";
                }
                m.Formatter = new System.Messaging.BinaryMessageFormatter();
                m.AppSpecific = 1;
                System.Messaging.MessageQueueTransaction mqt = new System.Messaging.MessageQueueTransaction();
                mqt.Begin();
                qu.Send(m, mqt);
                mqt.Commit();
                mqt.Dispose();
                mqt = null;
                m.Dispose();
                m = null;

                Dispose(ref client, ref mail);

                qu.BeginReceive();
            }
            else
            {
                if (e.Cancelled)
                {
                    System.Windows.Forms.MessageBox.Show(e.Error.Message, "Email Cancelled", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                }
                if (e.Error != null)
                {
                    System.Windows.Forms.MessageBox.Show(e.Error.Message, "Email NOT Sent!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
                else if (!e.Cancelled)
                {
                    System.Windows.Forms.MessageBox.Show(result, "Email Sent!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// Validates an email address
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public static bool ValidateEmailAddress(string emailAddress)
        {
            bool go = false;
            try
            {
                string TextToValidate = emailAddress;
                Regex expression = new Regex(@"\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}");
                if (expression.IsMatch(TextToValidate))
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return go;
        }

        #endregion others
    }
}