﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DB;
using DB.Reports;
using VTools;

namespace k0X
{
    public partial class ucSamples
    {

        private CReport Irepo = null;
        private void Import_Click(object sender, EventArgs e)
        {
            //When Importing --> MatSSF, Load Peaks (with re-transfer), Solang and recalculate (NAA)
            //When MatSSF ===> only MatSFF of coourse
            //When CalculateSolang =>  Load Peaks (without re-transfer unless not found), Solang and recalculate (NAA)...
            //When Recalculate --> Load Peaks (without re-transfer unless not found), recalculate (NAA)

            string toDo = "Run";

            if (sender.Equals(this.Delete))
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete all calculated data available for the samples or measurements selected?\n\n" +
                "This will NOT affect any sample data and its available measurements. Recalculation can be done once more at any time.\nHowever current self-shielding results, " +
                "calculated concentrations / FCs and gamma-lines selection/rejection information will be lost.\n\nContinue?", "Delete Analysis...", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No) return;

                toDo = "Delete";
            }

            if (MQ == null)
            {
                MQ = Rsx.Emailer.CreateMQ(DB.Properties.Resources.QMWorks + "." + pathCode, null);
            }
            if (MQ == null)
            {
                Interface.IReport.Msg("Check if MSMQ wether is installed", "Cannot initiate the Message Queue", false);
                return;
            }
            else MQ.Purge();

            if (timerQM == null)
            {
                timerQM = new Timer(this.components);
                timerQM.Interval = 200;
                timerQM.Tick += timerQM_Tick;
            }
            timerQM.Tag = null;
            timerQM.Enabled = true;

            ButtonVisible(false);

            int obj = Index(sender);
            SendQMsg(obj, toDo);
        }

        private void SampleConcentrationsOrFCs_Click(object sender, EventArgs e)
        {
            string Title = "Fc's Report - " + this.Name;

            if (Irepo == null) Irepo = new CReport(Interface.Get() as DataSet);

            Irepo.LoadACrystalReport(Title, CReport.ReporTypes.FcReport);
        }

        private void AnyNodeCMS_Click(object sender, EventArgs e)
        {
            if (this.TV.SelectedNode == null) return;
            object tag = this.TV.SelectedNode.Tag;
            if (tag == null) return;
            DataRow row = tag as DataRow;
            if (Rsx.Dumb.IsNuDelDetch(row)) return;

            Logger log = null;
            object o = null;
            string toprint = string.Empty;
            string title = string.Empty;
            Type tipo = tag.GetType();

            if (tipo.Equals(typeof(LINAA.MeasurementsRow)))
            {
                LINAA.MeasurementsRow m = (LINAA.MeasurementsRow)tag;
                if (sender.Equals(Peaks))
                {
                    o = m.GetPeaksRows();
                    toprint = "Peaks";
                    title = m.Measurement;
                }
            }
            else if (tipo.Equals(typeof(LINAA.SubSamplesRow)))
            {
                LINAA.SubSamplesRow l = (LINAA.SubSamplesRow)tag;
                title = l.SubSampleName;
                if (sender.Equals(this.MeasurementsHyperLab))
                {
                    o = l.GetMeasurementsRows();
                    toprint = "Measurements";
                }
                else if (sender.Equals(this.ViewMatSSF))
                {
                    o = l.GetMatSSFRows();
                    toprint = "MatSSF Results";
                }
            }

            if (o == null) return;
            IEnumerable<DataRow> rows = o as IEnumerable<DataRow>;
            log = new Logger(rows.CopyToDataTable(), title + " - " + toprint);
            log.Show();
        }

        private void watchDog_Click(object sender, EventArgs e)
        {
            LINAA Linaa = (LINAA)Interface.Get();

            IWatchDog wD = null;
            try
            {
                if (sender.Equals(this.watchDogToolStripMenuItem))
                {
                    wD = new ucWatchDog();

                    Program.UserControls.Add(wD);

                    wD.Link(ref Linaa, this.Name);
                    if (!Linaa.IsSpectraPathOk)
                    {
                        Interface.IReport.Msg("Make sure the right Spectra Directory Path is given in the <DB Connections> (right-click on the Notifier)!", "Could not connect to Spectra Directory", false);
                    }
                    else wD.Watch();
                }
                else
                {
                    Form f = new Form();
                    f.Text = " Measurements xTable for " + this.Name;

                    DataView view = Linaa.Measurements.AsDataView();
                    view.RowFilter = "Project LIKE '" + this.Name + "*'";
                    DataGridView dgv = new DataGridView();

                    dgv.Dock = DockStyle.Fill;
                    //  f.AutoSize = true;
                    //    f.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;
                    f.Controls.Add(dgv);

                    string detcol = Linaa.Measurements.DetectorColumn.ColumnName;
                    string[] filt = new string[] { Linaa.Measurements.SampleColumn.ColumnName };
                    string poscol = Linaa.Measurements.PositionColumn.ColumnName;
                    ucWatchDog.ShowXTable(ref view, ref dgv, detcol, filt, poscol);

                    f.Show();
                    f.Size = new System.Drawing.Size(dgv.Width + 10, dgv.Height + 10);
                }
            }
            catch (SystemException ex)
            {
                Interface.IReport.AddException(ex);
            }
        }

        private void OptionsMenu_DropDownOpened(object sender, EventArgs e)
        {
            Preferences(true);
        }

        private void OptionsMenu_DropDownClosed(object sender, EventArgs e)
        {
            Timer t = new Timer(this.components);
            t.Interval = 2000;
            t.Tick += t_Tick;
            t.Enabled = true;
        }

        private void unofficialDb_CheckedChanged(object sender, EventArgs e)
        {
            // bool unoficial = unofficialDb.Checked;

            W.RefreshDB(!unofficialDb.Checked);
        }

        public void ViewLarge_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;

            this.iSS.Daddy = this;
            //visible or not? save if necessary

            this.iSS.ChangeView();

            if (ParentForm != null)
            {
                this.ParentForm.Visible = !this.ParentForm.Visible;

                if (this.ParentForm.Visible)
                {
                    this.BuildTV();
                    this.TV.CollapseAll();

                    this.progress.Value = 0;
                    this.progress.Maximum = samples.Count();
                    foreach (LINAA.SubSamplesRow sample in samples)
                    {
                        LINAA.SubSamplesRow s = sample;
                        CheckNode(ref s);
                        this.progress.PerformStep();
                    }
                    this.TV.TopNode.Expand();
                    //DialogResult res = MessageBox.Show("Would you like to refresh the project?", "Important", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    //if (res == DialogResult.Yes) this.Populate.PerformClick();
                    //else if (res == DialogResult.Cancel) ViewLarge_Click(sender, e);
                }
            }

            System.Windows.Forms.Cursor.Current = Cursors.Default;
        }
    }
}