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
           // listView1.ItemActivate += ListView1_ItemActivate;
        }

        private void ListView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {

            showInBrowser(folderPath +e.Item.Name  + e.Item.Text + imageExtension);

        }

      

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            //  throw new NotImplementedException();
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
             
                //     i = imageList1.Images.Keys.IndexOf(e.Name);
              //  
              //  addImage(e.Name,e.FullPath);
                showInBrowser(e.FullPath);
                //listView1.Items.Clear();


                //     webBrowser1

            }
        }

        private void addImage(string name, string fullPath)
        {
            Image img = Image.FromFile(fullPath);
         
            if (imageList1.Images.Keys.Contains(name))
            {
                int i = imageList1.Images.Keys.IndexOf(name);
                imageList1.Images.RemoveAt(i);
            }
            imageList1.Images.Add(name, img);
        }

        private void showInBrowser(string file)
        {
          //  string file = XCom.StartupPath + matrixID + XCOM.PictureExtension;
            Uri uri = new Uri("about:blank");
            if (System.IO.File.Exists(file))
            {
                uri = new Uri(file);
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

        public void RefreshList(string baseFilename, string filter)
        {

            listView1.Items.Clear();
            imageList1.Images.Clear();

            string[] files = Directory.GetFiles(folderPath, baseFilename+filter+imageExtension);
            files = files.Select(o => o.Replace(folderPath, null)).ToArray();
            foreach (var item in files)
            {
                Image img = Image.FromFile(folderPath + item);
                string itemName = item.Replace(imageExtension, null);
                itemName = itemName.Replace(baseFilename, null);
                itemName = itemName.Replace(".", null);
                imageList1.Images.Add(itemName, img); 
            }

                foreach (var item in imageList1.Images.Keys)
            {
                ListViewItem i = new ListViewItem(item, imageList1.Images.IndexOfKey(item));
                i.Name = baseFilename + ".";

                listView1.Items.Add(i);
            }
          //  listView1.RedrawItems(i, i, false);

        }

    }
}
