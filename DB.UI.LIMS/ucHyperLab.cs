using System;
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
      //  private DB.LINAATableAdapters.PeaksHLTableAdapter HLTA;
        private Interface Interface;

        private LINAA.PeaksHLRow peak;
        private LINAA.MeasurementsRow picked;
        private DB.Tools.SolCoin solcoin;

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
                Interface.IBS.EnabledControls = false;
                Interface.IPopulate.ISamples.PopulateMeasurementsGeneric(projectTXT, true);
                Interface.IBS.EnabledControls = true;
             
            };

            IBoc.SetNoBindingSource();
            IBoc.TextContent = string.Empty;
        }
        public void Set (ref VTools.IOptions options)
        {
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


             };
        }
        // private CamFileReader reader; private DetectorX preader; public MainForm MainForm;
        public void Set(ref Interface inter)
        {
            Dumb.FD(ref Linaa);
            Linaa = inter.Get();
            this.Interface = inter;

            // Interface.IDB.PopulateColumnExpresions(); Interface.IAdapter.InitializeAdapters();

           // Interface.IPopulate.IDetSol.PopulateDetectorDimensions();
          //  Interface.IPopulate.IGeometry.PopulateGeometry();
          //  Interface.IPopulate.IIrradiations.PopulateIrradiationRequests();

            Interface.IDB.Measurements.ProjectColumn.Expression = string.Empty;
            Interface.IDB.Measurements.ProjectColumn.ReadOnly = false;
//
          //  this.geoBox.ComboBox.DisplayMember = "GeometryName";

           // this.geoBox.ComboBox.DataSource = this.Linaa.Geometry;

            this.GaTA.Fill(this.Linaa.Gammas);

            Interface.IAdapter.TAM.Connection.ConnectionString = Settings.Default.HLSNMNAAConnectionString;
        //    HLTA = new LINAATableAdapters.PeaksHLTableAdapter();

            CreateHLProjectBox();

            Rsx.Dumb.BS.LinkBS(ref this.measBS, Interface.IDB.Measurements, string.Empty, "MeasurementStart desc");
            Rsx.Dumb.BS.LinkBS(ref this.peaksBS, Interface.IDB.PeaksHL, string.Empty, "Energy desc");
            Rsx.Dumb.BS.LinkBS(ref this.gammasBS, Interface.IDB.Gammas, string.Empty, "Intensity desc");

           // this.CalGeo.Click += new System.EventHandler(this.CalGeo_Click);


            VTools.IGenericBox Ibox = project;

            Ibox.RefreshMethod += delegate
             {
                 if (picked != null)
                 {
                     //measBS.Filter = Interface.IDB.Measurements.ProjectColumn.ColumnName  + " = " + Ibox.TextContent;
                 }
             };


            this.measBS.CurrentChanged += delegate
            {
                if (!Interface.IBS.EnabledControls) return;
                if (measBS.Count != 0)
                {
                    picked = (LINAA.MeasurementsRow)((DataRowView)measBS.Current).Row;
                    int? id = picked?.MeasurementID;

                    Interface.IPopulate.ISamples.PopulatePeaksHL(id);

                    peaksBS.Filter = "MeasurementID = " + picked.MeasurementID;
                }
            };

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


        string geoText = "AU";// geoBox.Text;

        private void CalGeo_Click(object sender, EventArgs e)
        {
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
               
                solcoin.Energies = Hash.HashFrom<double>(this.Linaa.PeaksHL.EnergyColumn).ToArray();

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

        private void Fill_Click(object sender, EventArgs e)
        {
            try
            {
                // this.Linaa.TAM.MeasurementsTableAdapter.ConnectionString = DB.Properties.Settings.Default.HLSNMNAAConnectionString;

               // this.Linaa.TAM.MeasurementsTableAdapter.FillByHLSample(this.Linaa.Measurements, samplebox.Text);

                // if (sender.Equals(this.projectbox))
                {
                 //   UIControl.FillABox(samplebox, Hash.HashFrom<string>(this.Linaa.Measurements.SampleColumn), true, false);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void gammasBS_CurrentItemChanged(object sender, EventArgs e)
        {
            //take this out
            return;
            if (gammasBS.Count != 0)
            {
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
                        DateTime? calibDate = this.Linaa.QTA.GetCertificateDate(peak.MeasurementsRow.MeasurementID);
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
                        double? SolidAngle = (double?)Linaa.QTA.GetSolidAngle(geoText, Convert.ToInt32(peak.MeasurementsRow.Position), peak.MeasurementsRow.Detector, (double)energy);
                        double? SolidAngleRef = (double?)Linaa.QTA.GetSolidAngle("REF", 5, peak.MeasurementsRow.Detector, (double)energy);

                        double SolidFactor = 1;

                        if (SolidAngleRef != null && SolidAngle != null)
                        {
                            SolidFactor = (double)(SolidAngle / SolidAngleRef);
                        }

                        double? Log10Effi = (double?)Linaa.QTA.GetLog10Effi(peak.Energy, peak.MeasurementsRow.Detector, "REF", 5);
                        if (Log10Effi != null)
                        {
                            efficiency = Math.Pow(10.0, (double)Log10Effi) * (SolidFactor);
                        }
                        else if (Log10Effi == null) peak.RowError += "Log10Effi not found. Calculate Geometry first\n";
                        else if (SolidFactor == 1) peak.RowError += "SolidFactor not found. Calculate Geometry first\n";
                        double COI = 1.0;
                        double? coi = this.Linaa.QTA.GetCOI(peak.MeasurementsRow.Detector, geoText, iso, Convert.ToInt16(peak.MeasurementsRow.Position), (double)energy);
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
            Rsx.Dumb.BS.LinkBS(ref this.measBS, this.Linaa.Measurements, filter, "MeasurementStart desc");
        }

        public ucHyperLab()
        {
            InitializeComponent();
        }
    }
}