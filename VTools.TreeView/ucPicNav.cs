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

            try
            {
                Uri uri = new Uri("about:blank");
                if (System.IO.File.Exists(file))
                {
                    string newFile = Environment.GetFolderPath(Environment.SpecialFolder.InternetCache) + "\\";
                    newFile += file.Replace(folderPath, Guid.NewGuid().ToString().Substring(0, 8));
                   // newFile +=".html";
                    File.Copy(file, newFile, true);
                    uri = new Uri(newFile);
                }
               
                webBrowser1.Navigate(uri);
            }
            catch (Exception)
            {

               
            }
          
        }

        public void Set(string path,  string filter, string imageExt)
        {
            folderPath = path;
            imageExtension = imageExt;

            watcher.EnableRaisingEvents = false;
            watcher.Path = folderPath;
            watcher.Filter = filter+ imageExt;//= new FileSystemWatcher(path, filter);
            watcher.EnableRaisingEvents = true;
       //     watcher.NotifyFilter = NotifyFilters.LastWrite;
         
      
        }

      


        private string punto = ".";

        public void HideList(bool hide)
        {
            listView1.Visible = !hide;
            if (hide)
            {
                showInBrowser(string.Empty);
                cleanList();
            }
        }
        public void RefreshList(string baseFilename, string filter, string enumerator)
        {


            //get files
            string[] files = Directory.GetFiles(folderPath, baseFilename + filter);
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
                string itemText = item;
                string file = folderPath;
                //si contiene punto aun está numerado
                //   file+= baseFilename + punto;


                if (itemText.Contains(imageExtension))
                {
                    //   file += baseFilename + punto;

                    file += baseFilename + punto + itemText;

                    Image img = Image.FromFile(file);
                    itemText = itemText.Replace(imageExtension, null);


                }
                else if (itemText.CompareTo(baseFilename) == 0)
                {
                    continue;
                }
                else
                {
                    file += baseFilename + punto + itemText;
                }



                if (itemText.Contains(punto) && itemText.Contains(enumerator))
                {
                    //   file += baseFilename + punto;

                    int numberIndex = itemText.IndexOf(punto);
                    string number = itemText.Substring(0, numberIndex);

                    itemText = itemText.Substring(numberIndex + 1);


                }
             



                ListViewItem i = new ListViewItem(itemText);
             
                i.Tag = file;

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
            ListViewItem d = listView1.Items.Find("FULL",true).FirstOrDefault();
            if(d!=null) d.Selected = true;

       

        }

        private void cleanList()
        {

            if (listView1.Items.Count != 0) listView1.Items.Clear();
            if (listView1.Groups.Count != 0) listView1.Groups.Clear();
        }
    }
}
