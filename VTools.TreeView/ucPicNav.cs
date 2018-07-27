using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace VTools
{
    public partial class ucPicNav : UserControl
    {

        private FileSystemWatcher watcher;
        private string folderPath = string.Empty;
        private string imageExtension = string.Empty;
        public ucPicNav()
        {
            InitializeComponent();

            //  this.Load += UcPicNav_Load;
            watcher = new FileSystemWatcher();
            
            watcher.Changed += Watcher_Changed;
            listView1.ItemSelectionChanged += ListView1_ItemSelectionChanged;

            Application.EnableVisualStyles();
            // listView1.ItemActivate += ListView1_ItemActivate;
        }

        private void ListView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {

                     
            showInBrowser(e.Item.Tag.ToString());

        }

      

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                showInBrowser(e.FullPath);
            }
        }

        private void showInBrowser(string file)
        {
          //  string file = XCom.StartupPath + matrixID + XCOM.PictureExtension;

            Uri uri = new Uri("about:blank");
            if (System.IO.File.Exists(file))
            {
                string newFile = Environment.GetFolderPath(Environment.SpecialFolder.InternetCache) + "\\";
                newFile += file.Replace(folderPath, null);
                File.Copy(file, newFile, true);
                uri = new Uri(newFile);
            }
            webBrowser1.Navigate(uri);
        }

        public void Set(string path,  string filter, string imageExt)
        {
            folderPath = path;
            imageExtension = imageExt;

            watcher.EnableRaisingEvents = false;
            watcher.Path = folderPath;
            watcher.Filter = filter+ imageExtension;//= new FileSystemWatcher(path, filter);
            watcher.EnableRaisingEvents = true;
        }


       private  string punto = ".";

        public void HideList(bool hide)
        {
            listView1.Visible = !hide;
            if (hide)
            {
                showInBrowser(string.Empty);
                cleanList();
            }
        }
        public void RefreshList(string baseFilename, string filter)
        {


            //get files
            string[] files = Directory.GetFiles(folderPath, baseFilename + filter + imageExtension);
            files = files.Select(o => o.Replace(folderPath, null).Replace(baseFilename + punto, null)).ToArray();

      //      files = files.OrderBy(o => o.Substring(1, o.IndexOf(punto))).ToArray();
            files = files.OrderBy(o => o.Length).ToArray();


            listView1.Visible = true;
            listView1.ShowGroups = true;
            listView1.View = View.SmallIcon;

            cleanList();

     //       if (imageList1.Images.Count!=0)   imageList1.Images.Clear();

            List<ListViewItem> ls = new List<ListViewItem>();


            foreach (var item in files)
            {
                string file = folderPath + baseFilename + punto + item;
                Image img = Image.FromFile(file);
                string itemText = item.Replace(imageExtension, null);
        
                string itemName = baseFilename + punto;

                //si contiene punto aun está numerado
                if (itemText.Contains(punto))
                {
                    int numberIndex = itemText.IndexOf(punto);
                    string number = itemText.Substring(0, numberIndex);
                    itemName += number + punto;
                    itemText = itemText.Substring(numberIndex+1);
                }
         
                ListViewItem i = new ListViewItem(itemText);
             
                i.Tag = folderPath + itemName + itemText + imageExtension;

                ls.Add(i);

            }

            foreach (var item in ls)
            {
                string count = item.Text.Count().ToString();
                 ListViewGroup g = listView1.Groups[count];

                 if (g == null)
                  {
                
                    g = new ListViewGroup(count, string.Empty);
                    g.HeaderAlignment = HorizontalAlignment.Left;
                 
                    listView1.Groups.Add(g);
                 
             
                  //  g.ListView.View = View.Tile;
                }
            

           
             
                listView1.Items.Add(item);
            
                item.EnsureVisible();
                item.Group = g;
           //     g.Items.Add(item);

                // item.Group = g;

            }

         //   ls.Clear();
      //      listView1.ShowGroups = true;
        //    listView1.PerformLayout();
            //   listView1.RedrawItems(0, listView1.Items.Count, false); 
            //      listView1.VirtualMode = true;
            ListViewItem d = listView1.Groups[0].Items[0];
            d.Selected = true;

       

        }

        private void cleanList()
        {

            if (listView1.Items.Count != 0) listView1.Items.Clear();
            if (listView1.Groups.Count != 0) listView1.Groups.Clear();
        }
    }
}
