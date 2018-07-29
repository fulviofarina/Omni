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

        public override string CreateListItem(string baseFilename, string enumerator, ref string file,  string item)
        {
            string itemText = item;
            //si es una imagen
            if (itemText.Contains(IMAGE_EXTENSION))
            {
                file += baseFilename + PUNTO + itemText;
                //     Image img = Image.FromFile(file);
                itemText = itemText.Replace(IMAGE_EXTENSION, null);
            }
            //si es un archivo base sin extension, saltalo
            else if (itemText.CompareTo(baseFilename) == 0)
            {
                return string.Empty ;
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
            if (itemText.ToLower().Contains(csv) )
            {
                itemText =comma +" "+ separated;
            }
            //si es un .xls
            else if (itemText.ToLower().Contains(xls))
            {
                itemText = spreadsheet;
            }
            return itemText;
        }

        public override string[] SelectAndOrder(string baseFilename, string[] files)
        {
            files = files.Select(o => o.Replace(FOLDERPATH, null).Replace(baseFilename + PUNTO, null)).ToArray();
            files = files.OrderBy(o => o.Length).ToArray();
            return files;
        }

    }
}
