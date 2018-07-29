using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DB.Tools
{
    public class ucXCOMPicNav: VTools.ucPicNav
    {
        public ucXCOMPicNav() : base()
        {


        }

        /// <summary>
        /// Overrider for my program in question
        /// </summary>
        /// <param name="baseFilename"></param>
        /// <param name="enumerator"></param>
        /// <param name="files"></param>
        /// <param name="ls"></param>
        public override void CreateListItems(string baseFilename, string enumerator, ref string[] files, ref List<ListViewItem> ls)
        {
            foreach (var item in files)
            {
                string itemText = item;
                string file = FOLDERPATH;

                //si es una imagen
                if (itemText.Contains(IMAGE_EXTENSION))
                {
                    file += baseFilename + PUNTO + itemText;
                    Image img = Image.FromFile(file);
                    itemText = itemText.Replace(IMAGE_EXTENSION, null);
                }
                //si es un archivo base sin extension, saltalo
                else if (itemText.CompareTo(baseFilename) == 0)
                {
                    continue;
                }
                //otros
                else
                {
                    file += baseFilename + PUNTO + itemText;
                }

                //si es una archivo numerado con el enumerador, por ejemplo N40.algo
                if (itemText.Contains(PUNTO) && itemText.Contains(enumerator))
                {
                    int numberIndex = itemText.IndexOf(PUNTO);
                    string number = itemText.Substring(0, numberIndex);
                    itemText = itemText.Substring(numberIndex + 1);
                }
                //si es un .csv
                if (itemText.CompareTo("csv") == 0)
                {
                    itemText = "comma-separated";
                }
                //si es un .xls
                else if (itemText.CompareTo("xls") == 0)
                {
                    itemText = "excel";
                }

                ListViewItem i = new ListViewItem(itemText.ToUpper());
                i.Tag = file; //attach to tag to open file later
                ls.Add(i);
            }
        }

        public override string[] SelectAndOrder(string baseFilename, string[] files)
        {
            files = files.Select(o => o.Replace(FOLDERPATH, null).Replace(baseFilename + PUNTO, null)).ToArray();
            files = files.OrderBy(o => o.Length).ToArray();
            return files;
        }

    }
}
