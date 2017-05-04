using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DB.Reports;
using Rsx.Dumb; using Rsx;
using DB.Tools;

namespace DB.UI
{
    public partial class ucSSContent : UserControl
    {
        private DateTime minDate = DateTime.Now;

        public ucSSContent()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Changes the view with another form
        /// </summary>
/*
        public void DeLink()
        {
            Dumb.BS.DeLinkBS(ref this.BS);
        }

        /// <summary>
        /// links to the binding source
        /// </summary>
        public void Link(string Filter, string Sort)
        {
            Dumb.BS.LinkBS(ref this.BS, this.Linaa.SubSamples, Filter, Sort);
        }
        */

        public void Set(ref Interface inter)
        {
            //   DeLink();

            Dumb.FD(ref Linaa);
            Dumb.FD(ref BS);

        //    this.Linaa = inter.Get();

             BS = inter.IBS.SubSamples;

            DGV.DataSource = BS;
            DGV2.DataSource = BS;
            DGV3.DataSource = BS;
            DGV4.DataSource = BS;
        }

        /// <summary>
        /// Painter method
        /// </summary>
        /// <param name="sampleCellArgs"></param>
        public void PaintCells(object sender, DataGridViewCellPaintingEventArgs sampleCellArgs)
        {
            int ind = sampleCellArgs.ColumnIndex;
            if (ind == 0) SampleCellPaint(ref sampleCellArgs);
            else if (ind == this.InReactorColumn.Index) IrradiationCellPaint(ref sampleCellArgs);
        }

        /// <summary>
        /// Paint method for irradiation cell
        /// </summary>
        /// <param name="sampleCellArgs"></param>
        private void IrradiationCellPaint(ref DataGridViewCellPaintingEventArgs sampleCellArgs)
        {
            DateTime currentVal;
            bool parsed = DateTime.TryParse(sampleCellArgs.Value.ToString(), out currentVal);
            if (parsed)
            {
                if (currentVal < minDate)
                {
                    //minDate = Samples.Where(s => !s.HasErrors).Where(s => !s.IsInReactorNull()).Min(c => c.InReactor);
                    this.InReactorColumn.Tag = currentVal;
                    this.OutReactorColumn.Tag = currentVal;
                }
            }
        }

        /// <summary>
        /// Paint method for sample cell
        /// </summary>
        /// <param name="sampleCellArgs"></param>
        private void SampleCellPaint(ref DataGridViewCellPaintingEventArgs sampleCellArgs)
        {
            if (sampleCellArgs == null) return;
            if (sampleCellArgs.Value == null) return;
            //   if (sampleCellArgs.CellStyle.BackColor != Color.FromName("Window")) return;
            string aux = sampleCellArgs.Value.ToString();
            if (aux.Contains(DB.Properties.Samples.Std))
            {
                sampleCellArgs.CellStyle.BackColor = Color.Thistle;
            }
            else if (aux.Contains(DB.Properties.Samples.Mon))
            {
                sampleCellArgs.CellStyle.BackColor = Color.Salmon;
            }
            else if (aux.Contains(DB.Properties.Samples.RefMat))
            {
                sampleCellArgs.CellStyle.BackColor = Color.PapayaWhip;
            }
            else if (aux.Contains(DB.Properties.Samples.Smp))
            {
                sampleCellArgs.CellStyle.BackColor = Color.Salmon;
            }
            else sampleCellArgs.CellStyle.BackColor = Color.Honeydew;
        }

        public bool ShouldPaint(object sender, DataGridViewCellPaintingEventArgs e)
        {
            int ind = e.ColumnIndex;
            if (sender.Equals(this.DGV3))
            {
                if (ind != this.InReactorColumn.Index && ind != 0) return false;
            }
            else if (ind != 0) return false;
            return true;
        }
    }
}