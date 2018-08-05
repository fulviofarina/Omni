﻿using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DB.Properties;
using DB.Tools;
using Rsx.Dumb;

//using Rsx.CAM;

namespace DB.UI
{
    public partial class ucHyperLab : UserControl
    {
        private Interface Interface;
        private Uri helpFile = new Uri("https://www.researchgate.net/publication/317904672");


        public void CreateHLProjectBox()
        {
            VTools.IGenericBox IBoc = this.project;
           
            IBoc.PopulateListMethod += delegate
            {
                IBoc.InputProjects = Interface.IPopulate.IProjects.ListOfHLProjects().ToArray();
            };

            IBoc.RefreshMethod += delegate
            {
                string projectTXT = IBoc.TextContent;

                if (IBoc.InputProjects.Contains(projectTXT))
                {
                    Interface.IPopulate.LoadProjectHL(projectTXT);

                    Interface.IPreferences.CurrentPref.LastIrradiationProject = projectTXT;
                }
            };
            //interesting if this workk without binding
            IBoc.SetNoBindingSource();
            IBoc.TextContent = Interface.IPreferences.CurrentPref.LastIrradiationProject;
        }
        public void Set (ref VTools.IOptions options)
        {
            if (options == null) return;

            EventHandler help = delegate
            {
                IO.Process("explorer.exe ", string.Empty, helpFile.ToString(),true,false);
            };

            options.HelpClick += help;


            (options as Control).Dock = DockStyle.Fill;
            TLP2.Controls.Add(options as Control);
        }

        public void Set(ref ISpecPreferences pref)
        {
            pref.FilterChangedEvent += delegate
             {



             };

            pref.IndexChangedEvent += delegate

             {
                // Interface.IPreferences.CurrentSpecPref.SetIdxLength();

             };
        }

        private void destroy()
        {
            measDGV.DataSource = null;
            peaksDGV.DataSource = null;
            gammasDGV.DataSource = null; 
         //   this.mea.BindingSource = null;
         //   Dumb.FD(ref this.measBS);
            Dumb.FD(ref this.measBS);
            Dumb.FD(ref this.peaksBS);
            Dumb.FD(ref this.gammasBS);
            Dumb.FD(ref Linaa);
        }
        // private CamFileReader reader; private DetectorX preader; public MainForm MainForm;
        public void Set(ref Interface inter)
        {


            destroy();
         
            this.Interface = inter;
         //   Linaa = inter.Get();
            // Interface.IDB.PopulateColumnExpresions(); Interface.IAdapter.InitializeAdapters();

            // Interface.IPopulate.IDetSol.PopulateDetectorDimensions();
            //  Interface.IPopulate.IGeometry.PopulateGeometry();
            //  Interface.IPopulate.IIrradiations.PopulateIrradiationRequests();
            Interface.IDB.Measurements.ProjectColumn.Expression = string.Empty;
            Interface.IDB.Measurements.ProjectColumn.ReadOnly = false;

            this.measDGV.DataSource = Interface.IBS.Measurements;
            this.peaksDGV.DataSource = Interface.IBS.PeaksHL;
            this.gammasDGV.DataSource = Interface.IBS.Gammas;

            this.measDGV.ColumnHeaderMouseClick += Interface.IReport.ReportToolTip;
            this.peaksDGV.ColumnHeaderMouseClick += Interface.IReport.ReportToolTip;
            this.gammasDGV.ColumnHeaderMouseClick += Interface.IReport.ReportToolTip;
            //
            //  this.geoBox.ComboBox.DisplayMember = "GeometryName";

            // this.geoBox.ComboBox.DataSource = this.Linaa.Geometry;

            //     this.GaTA.Fill(this.Linaa.Gammas);
       
        //    HLTA = new LINAATableAdapters.PeaksHLTableAdapter();

            CreateHLProjectBox();

        //    Rsx.Dumb.BS.LinkBS(ref this.measBS, Interface.IDB.Measurements, string.Empty, "MeasurementStart desc");
      //      Rsx.Dumb.BS.LinkBS(ref this.peaksBS, Interface.IDB.PeaksHL, string.Empty, "Energy desc");
     //       Rsx.Dumb.BS.LinkBS(ref this.gammasBS, Interface.IDB.Gammas, string.Empty, "Intensity desc");

           // this.CalGeo.Click += new System.EventHandler(this.CalGeo_Click);



            /*
            this.peaksBS.CurrentChanged += delegate
            {
                if (peaksBS.Count != 0)
                {
                    peak = (LINAA.PeaksHLRow)((DataRowView)peaksBS.Current).Row;
                    if (this.gammasBS.DataSource != null)
                    {
                        if (this.gammasBS.DataSource.Equals(typeof(DataTable)))
                        {
                            DataTable table = this.gammasBS.DataSource as DataTable;
                            table.Clear();
                            table.Dispose();
                        }
                    }
                    Rsx.Dumb.BS.LinkBS(ref this.gammasBS, GaTA.GetPossibleIsotopes(peak.Energy, 1));
                }
            };
            this.gammasBS.CurrentChanged += (this.gammasBS_CurrentItemChanged);
            
        //    this.samplebox.TextChanged += (this.Fill_Click);
            */

        }


       private string geoText = "AU";// geoBox.Text;

        private void CalGeo_Click(object sender, EventArgs e)
        {
             DB.Tools.SolCoin solcoin = null;
              LINAA.MeasurementsRow picked = null;//AREGLAR

            if (solcoin == null) solcoin = new DB.Tools.SolCoin();

            bool success = solcoin.Gather("REF", Convert.ToInt16(picked.Position), picked.Detector, "REF", 5);

            if (success && solcoin.Exception == null)
            {
                solcoin.CalculateCOIS = false;
                solcoin.StoreCOIS = false;
                solcoin.CleanEffiCOIS = false;

                solcoin.CalculateSolidAngles = true;
                solcoin.CleanSolidAngles = false;

                solcoin.IntegrationMode = DB.Tools.SolCoin.IntegrationModes.AsPointSource;
                 solcoin.StoreSolidAngles = true;
               
                solcoin.Energies = Hash.HashFrom<double>(Interface.IDB.PeaksHL.EnergyColumn).ToArray();

                solcoin.DoAll(false);
                solcoin.Merge();
            }
            else picked.RowError += "Reference Geometry Data was NOT OK!";

         
       
            success = solcoin.Gather(geoText, Convert.ToInt16(picked.Position), picked.Detector, "REF", 5);

            if (success && solcoin.Exception == null)
            {
                solcoin.CalculateCOIS = false;
                solcoin.StoreCOIS = false;
                solcoin.CleanEffiCOIS = false;

                solcoin.CalculateSolidAngles = true;
                solcoin.CleanSolidAngles = false;

                solcoin.IntegrationMode = DB.Tools.SolCoin.IntegrationModes.AsNonDisk;
                solcoin.StoreSolidAngles = true;
             
                solcoin.DoAll(false);
                solcoin.Merge();
            }
            else picked.RowError += "Geometry Data was NOT OK!";
        }

      
        private void gammasBS_CurrentItemChanged(object sender, EventArgs e)
        {
          //  //take this out
         //   return;


            if (gammasBS.Count != 0)
            {
                 LINAA.PeaksHLRow peak =null; //AREGLAR
                LINAA.GammasRow gamma = (LINAA.GammasRow)((DataRowView)gammasBS.Current).Row;

                if (peak != null && peak.MeasurementsRow != null)
                {
                    try
                    {
                        peak.ClearErrors();

                        peak.RadiationID = string.Empty;
                        peak.Isotope = string.Empty;
                        peak.GAct = 0.0;
                        peak.RadiationID = gamma.RADIATION_ID;
                        peak.Isotope = gamma.SYMBOL;

                        double lamda = (0.693 / peak.GammasRow.HALFLIFE);
                        double C = 1.0 - Math.Exp(-1 * lamda * peak.MeasurementsRow.CountTime);
                        DateTime? calibDate = Interface.IAdapter.QTA.GetCertificateDate(peak.MeasurementsRow.MeasurementID);
                        double D = 1;
                        if (calibDate != null)
                        {
                            D = Math.Exp(-1 * lamda * (peak.MeasurementsRow.MeasurementStart - (DateTime)calibDate).TotalSeconds);
                        }
                        else peak.RowError += "Calibration Date not found. Certificate not found!\n";

                        int position = Convert.ToInt16(peak.MeasurementsRow.Position);

                        string isoNr = System.Text.RegularExpressions.Regex.Replace(peak.Isotope, "[a-z]", string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                        if (isoNr.Contains('_')) isoNr = isoNr.Split('_')[0];
                        string iso = System.Text.RegularExpressions.Regex.Replace(peak.Isotope, "[0-9]", string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                        if (iso.Contains("_m")) iso = iso.Replace("_m", isoNr + "M");
                        else iso += isoNr;
                        iso.ToUpper();
                        peak.Isotope = iso;

                        double efficiency = 1.0;
                        decimal energy = 0;
                        energy = decimal.Round(Convert.ToDecimal(peak.Energy), 1);
                        double? SolidAngle = (double?)Interface.IAdapter.QTA.GetSolidAngle(geoText, Convert.ToInt32(peak.MeasurementsRow.Position), peak.MeasurementsRow.Detector, (double)energy);
                        double? SolidAngleRef = (double?)Interface.IAdapter.QTA.GetSolidAngle("REF", 5, peak.MeasurementsRow.Detector, (double)energy);

                        double SolidFactor = 1;

                        if (SolidAngleRef != null && SolidAngle != null)
                        {
                            SolidFactor = (double)(SolidAngle / SolidAngleRef);
                        }

                        double? Log10Effi = (double?)Interface.IAdapter.QTA.GetLog10Effi(peak.Energy, peak.MeasurementsRow.Detector, "REF", 5);
                        if (Log10Effi != null)
                        {
                            efficiency = Math.Pow(10.0, (double)Log10Effi) * (SolidFactor);
                        }
                        else if (Log10Effi == null) peak.RowError += "Log10Effi not found. Calculate Geometry first\n";
                        else if (SolidFactor == 1) peak.RowError += "SolidFactor not found. Calculate Geometry first\n";
                        double COI = 1.0;
                        double? coi = Interface.IAdapter.QTA.GetCOI(peak.MeasurementsRow.Detector, geoText, iso, Convert.ToInt16(peak.MeasurementsRow.Position), (double)energy);
                        if (coi != null) COI = (double)coi;

                        peak.GAct = (peak.Area * lamda) / (peak.GammasRow.INTENSITY * 0.01 * COI * efficiency * C * D * gamma.DECAYBRANCHING);
                    }
                    catch (SystemException ex)
                    {
                        peak.RowError = ex.Message;
                    }
                }
            }
        }

        private void TV_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            string filter = string.Empty;
            if (e.Node.Text.ToUpper().CompareTo("ALL") != 0)
            {
                filter = "Detector = '" + e.Node.Text + "'";
            }
            Rsx.Dumb.BS.LinkBS(ref this.measBS, Interface.IDB.Measurements, filter, "MeasurementStart desc");
        }

        public ucHyperLab()
        {
            InitializeComponent();
        }
    }
}