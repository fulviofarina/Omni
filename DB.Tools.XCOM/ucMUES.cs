using System;
using System.IO;
using System.Windows.Forms;

using DB.Tools;
using Rsx.Dumb;

namespace DB.UI
{
    public partial class ucMUES : UserControl
    {
        protected internal bool advanced = true;
        protected internal Interface Interface = null;

        /// <summary>
        /// DGV ITEM SELECTED
        /// </summary>

        public void Set(ref Interface LinaaInterface, ref IXCOMPreferences pref)
        {
            try
            {
                Dumb.FD(ref this.Linaa);
                this.compositionsDGV.DataSource = null;
                Dumb.FD(ref this.bs);

                Interface = LinaaInterface;
                // BindingSource bsCompositions = null;

                this.compositionsDGV.BackgroundColor = System.Drawing.Color.Thistle;

                // bsCompositions = Interface.IBS.MUES;

                // this.compositionsDGV.DataMember = string.Empty;
                this.compositionsDGV.DataSource = Interface.IBS.MUES;
                this.compositionsDGV.RowHeadersVisible = false;



                // this.compositionsDGV.MouseDoubleClick += WebBrowser_MouseDoubleClick;
                //   this.webBrowser.cl += WebBrowser_MouseDoubleClick;

                // string column = Interface.IDB.Matrix.MatrixCompositionColumn.ColumnName; Binding
                // mcompoBin = Rsx.Dumb.BS.ABinding(ref Bind, column);
                // this.matrixRTB.DataBindings.Add(mcompoBin); this.matrixRTB.MouseLeave += focus;
                // this.compositionsDGV.MouseHover += focus;
         
                pref.RoundingChanged += delegate
                  {
                      string rounding2 = Interface.IPreferences.CurrentXCOMPref.Rounding;
                      SetDGVRounding(rounding2);
                  };

                string rounding = Interface.IPreferences.CurrentXCOMPref.Rounding;
                SetDGVRounding(rounding);


                Focus(true);
                    // focus(null, EventArgs.Empty);
            }
            catch (System.Exception ex)
            {
                Interface.IStore.AddException(ex);
            }
        }

        private void WebBrowser_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        
            Focus(advanced);
            advanced = !advanced;
        }

        public void Focus(bool table)
        {
            SC.Panel2Collapsed = table;
            SC.Panel1Collapsed = !table;
    
        }

        public ucMUES()
        {
            InitializeComponent();
        }
       
        public void SetDGVRounding(string rounding)
        {
            foreach (DataGridViewColumn item in compositionsDGV.Columns)
            {
                bool okCol = item.DataPropertyName[0].CompareTo('M') == 0;
                okCol = okCol || item.DataPropertyName[0].CompareTo('P') == 0;
                if (okCol)
                {
                    item.DefaultCellStyle.Format = rounding;
                }
            }
        }

        public void SendToBrowser(string tempFile, string response)
        {
            File.WriteAllText(tempFile, response);
            webBrowser.Url = null;
            Uri uri = new Uri(tempFile);
            webBrowser.Url = uri;
            webBrowser.Show();
        }
    }
}