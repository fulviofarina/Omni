using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DB;
using Rsx;
using DB.Tools;

//using Rsx.CAM;

namespace k0X
{
    public partial class ucHyperLab : UserControl
    {
        private Interface Interface;

        private LINAA.MeasurementsRow picked;
        private LINAA.PeaksHLRow peak;
        private DB.Tools.SolCoin solcoin;

        //  private CamFileReader reader;
        //  private DetectorX preader;
        public MainForm MainForm;

        private DB.LINAATableAdapters.PeaksHLTableAdapter HLTA;

        public ucHyperLab()
        {
            InitializeComponent();

            this.Linaa.InitializeComponent();
            //  object aux = this.Linaa;
            this.Interface = new Interface(ref this.Linaa);

            Interface.IMain.PopulateColumnExpresions();
            Interface.IAdapter.InitializeAdapters();

            Interface.IPopulate.IDetSol.PopulateDetectorDimensions();
            Interface.IPopulate.IGeometry.PopulateGeometry();
            Interface.IPopulate.IIrradiations.PopulateIrradiationRequests();

            this.Linaa.Measurements.ProjectColumn.Expression = string.Empty;
            this.Linaa.Measurements.ProjectColumn.ReadOnly = false;

            this.geoBox.ComboBox.DataSource = this.Linaa.Geometry;
            this.geoBox.ComboBox.DisplayMember = "GeometryName";

            this.GaTA.Fill(this.Linaa.Gammas);

            Dumb.FillABox(projectbox, this.Linaa.ProjectsList, true, false);

            this.Linaa.TAM.Connection.ConnectionString = DB.Properties.Settings.Default.HLSNMNAAConnectionString;
            HLTA = new DB.LINAATableAdapters.PeaksHLTableAdapter();

            Dumb.LinkBS(ref this.measBS, this.Linaa.Measurements, string.Empty, "MeasurementStart desc");
            Dumb.LinkBS(ref this.peaksBS, this.Linaa.PeaksHL, string.Empty, "Energy desc");
            Dumb.LinkBS(ref this.gammasBS, this.Linaa.Gammas, string.Empty, "Intensity desc");

            AuxiliarForm form = new AuxiliarForm();
            UserControl control = this;
            form.Populate(ref control);
            form.Text = "HyperLab Data";
            form.Show();
        }

        private void Fill_Click(object sender, EventArgs e)
        {
            try
            {
                //   this.Linaa.TAM.MeasurementsTableAdapter.ConnectionString = DB.Properties.Settings.Default.HLSNMNAAConnectionString;

                this.Linaa.TAM.MeasurementsTableAdapter.FillByHLSample(this.Linaa.Measurements, samplebox.Text);

                if (sender.Equals(this.projectbox))
                {
                    Dumb.FillABox(samplebox, Dumb.HashFrom<string>(this.Linaa.Measurements.SampleColumn), true, false);
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private void FillByMeasurement_Click(object sender, EventArgs e)
        {
            error.SetError(measTS, null);

            try
            {
                if (picked != null)
                {
                    double minArea = Convert.ToDouble(minAreabox.Text);
                    double maxUnc = Convert.ToDouble(maxUncbox.Text);

                    this.HLTA.FillByMeasurementID(this.Linaa.PeaksHL, picked.MeasurementID, minArea, maxUnc);
                }
            }
            catch (System.Exception ex)
            {
                error.SetError(measTS, ex.Message);
            }
        }

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
                //  if (IntegrationRefBox.Text.CompareTo("As Non Disk") == 0) solcoin.IntegrationMode = SolCoin.IntegrationModes.AsNonDisk;
                //  else if (IntegrationRefBox.Text.CompareTo("As Disk") == 0) solcoin.IntegrationMode = SolCoin.IntegrationModes.AsDisk;
                solcoin.StoreSolidAngles = true;
                //  detectorNode.Tag = solcoin;

                solcoin.Energies = Dumb.HashFrom<double>(this.Linaa.PeaksHL.EnergyColumn).ToArray();

                solcoin.DoAll(false);
                solcoin.Merge();
            }
            else picked.RowError += "Reference Geometry Data was NOT OK!";

            success = solcoin.Gather(geoBox.Text, Convert.ToInt16(picked.Position), picked.Detector, "REF", 5);

            if (success && solcoin.Exception == null)
            {
                solcoin.CalculateCOIS = false;
                solcoin.StoreCOIS = false;
                solcoin.CleanEffiCOIS = false;

                solcoin.CalculateSolidAngles = true;
                solcoin.CleanSolidAngles = false;

                solcoin.IntegrationMode = DB.Tools.SolCoin.IntegrationModes.AsNonDisk;
                //  if (IntegrationRefBox.Text.CompareTo("As Non Disk") == 0) solcoin.IntegrationMode = SolCoin.IntegrationModes.AsNonDisk;
                //  else if (IntegrationRefBox.Text.CompareTo("As Disk") == 0) solcoin.IntegrationMode = SolCoin.IntegrationModes.AsDisk;
                solcoin.StoreSolidAngles = true;
                //  detectorNode.Tag = solcoin;
                solcoin.DoAll(false);
                solcoin.Merge();
            }
            else picked.RowError += "Geometry Data was NOT OK!";
        }

        #region Binding Sources

        private void measBS_CurrentItemChanged(object sender, EventArgs e)
        {
            if (measBS.Count != 0)
            {
                picked = (LINAA.MeasurementsRow)((DataRowView)measBS.Current).Row;
                measIDbox.Text = picked.MeasurementID.ToString();
            }
        }

        private void peaksBS_CurrentItemChanged(object sender, EventArgs e)
        {
            //take this away
            return;

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
                Dumb.LinkBS(ref this.gammasBS, GaTA.GetPossibleIsotopes(peak.Energy, 1));
            }
        }

        private void gammasBS_CurrentItemChanged(object sender, EventArgs e)
        {
            //take this away
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
                        double? SolidAngle = (double?)Linaa.QTA.GetSolidAngle(geoBox.Text, Convert.ToInt32(peak.MeasurementsRow.Position), peak.MeasurementsRow.Detector, (double)energy);
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
                        double? coi = this.Linaa.QTA.GetCOI(peak.MeasurementsRow.Detector, geoBox.Text, iso, Convert.ToInt16(peak.MeasurementsRow.Position), (double)energy);
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

        #endregion Binding Sources

        private void TV_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            string filter = string.Empty;
            if (e.Node.Text.ToUpper().CompareTo("ALL") != 0)
            {
                filter = "Detector = '" + e.Node.Text + "'";
            }
            Dumb.LinkBS(ref this.measBS, this.Linaa.Measurements, filter, "MeasurementStart desc");
        }
    }
}