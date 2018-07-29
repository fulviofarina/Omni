using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace VTools
{
    public  partial class ucPicNav : UserControl
    {
        protected string FOLDERPATH = string.Empty;
        protected string IMAGE_EXTENSION = string.Empty;
        protected string nameForItemToSelect = string.Empty;
        protected string PUNTO = ".";

        private FileSystemWatcher watcher;


        /// <summary>
        /// Main List Generator for my program in question
        /// </summary>
        /// <param name="baseFilename"></param>
        /// <param name="enumerator"></param>
        /// <param name="files"></param>
        /// <param name="ls"></param>
        public void CreateListItems(string baseFilename, string enumerator, ref string[] files, ref List<ListViewItem> ls)
        {
            foreach (var item in files)
            {

                string file = FOLDERPATH;

                string itemText = CreateListItem(baseFilename, enumerator, ref file, item);

                if (String.IsNullOrEmpty(itemText)) continue;

                makeItem(ref ls, itemText, file);

            }
        }

        /// <summary>
        /// Function to override for list generation
        /// </summary>
        /// <param name="baseFilename"></param>
        /// <param name="enumerator"></param>
        /// <param name="file"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual string CreateListItem(string baseFilename, string enumerator, ref string file, string item)
        {

            return item;
        }
        protected void makeItem(ref List<ListViewItem> ls, string itemText, string file)
        {
            ListViewItem i = new ListViewItem(itemText.ToUpper());
            i.Tag = file; //attach to tag to open file later
            ls.Add(i);
        }
        /// <summary>
        /// cleans the list and hides
        /// </summary>
        /// <param name="hide"></param>
        public void HideList(bool hide)
        {
            listView1.Visible = !hide;
            if (hide)
            {
                showInBrowser(string.Empty);
                cleanList();
            }
        }

        public void NavigateTo(Uri helpFile)
        {
            webBrowser1.Navigate(helpFile);
        }

        /// <summary>
        /// Makes a list from basefileName.Filter and removes the enumerator from their names
        /// </summary>
        /// <param name="baseFilename">i.e. 84.</param>
        /// <param name="filter">84.*</param>
        /// <param name="enumerator">84.N##.filename.extension</param>
        public void RefreshList(string baseFilename, string filter, string enumerator)
        {
            //get files
            string[] files = Directory.GetFiles(FOLDERPATH, baseFilename + filter);
            files = SelectAndOrder(baseFilename, files);

            listView1.Visible = true;

            cleanList();

            List<ListViewItem> ls = new List<ListViewItem>();

            CreateListItems(baseFilename, enumerator, ref files, ref ls);

            addToGroups(ref ls);


            selectDefaultItem(nameForItemToSelect);
        }

        public virtual string[] SelectAndOrder(string baseFilename, string[] files)
        {
            return files;
        }
        public void Set(string path, string filter, string imageExt, string nameDefaultSelectedItem)
        {
            nameForItemToSelect = nameDefaultSelectedItem;
            FOLDERPATH = path;
            IMAGE_EXTENSION = imageExt;

            watcher.EnableRaisingEvents = false;
            watcher.Path = FOLDERPATH;
            watcher.Filter = filter + imageExt;
            watcher.EnableRaisingEvents = true;
        }

        private void addToGroups(ref List<ListViewItem> ls)
        {
            foreach (var item in ls)
            {
                string count = item.Text.Count().ToString();
                ListViewGroup g = listView1.Groups[count];

                if (g == null)
                {
                    g = new ListViewGroup(count, string.Empty);
                    g.HeaderAlignment = HorizontalAlignment.Left;
                    listView1.Groups.Add(g);
                }

                listView1.Items.Add(item);

                item.EnsureVisible();
                item.Group = g;
            }
        }

        private void cleanList()
        {
            if (listView1.Items.Count != 0) listView1.Items.Clear();
            if (listView1.Groups.Count != 0) listView1.Groups.Clear();
        }

        private void listViewItemChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            showInBrowser(e.Item.Tag.ToString());
        }

        private void selectDefaultItem(string nameForItemToSelect)
        {
            ListViewItem d = listView1.Items.Cast<ListViewItem>().FirstOrDefault(o => o.Text.CompareTo(nameForItemToSelect) == 0);
            if (d != null) d.Selected = true;
        }
        private void showInBrowser(string file)
        {
            try
            {
                Uri uri = new Uri("about:blank");
                if (System.IO.File.Exists(file))
                {
                    string newFile = Environment.GetFolderPath(Environment.SpecialFolder.InternetCache) + "\\";
                    newFile += file.Replace(FOLDERPATH, null);
                    newFile += Guid.NewGuid().ToString().Substring(0, 6);
                    File.Copy(file, newFile, true);
                    uri = new Uri(newFile);
                }

                NavigateTo(uri);
            }
            catch (Exception ex)
            {

            }
        }

        private void watcherFileChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                showInBrowser(e.FullPath);
            }
        }

        public ucPicNav()
        {
            InitializeComponent();

            watcher = new FileSystemWatcher();
            watcher.Changed += watcherFileChanged;

            listView1.ItemSelectionChanged += listViewItemChanged;
            listView1.ShowGroups = true;
            listView1.View = View.SmallIcon;

            Application.EnableVisualStyles();

            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.AllowNavigation = true;
        }
    }
}