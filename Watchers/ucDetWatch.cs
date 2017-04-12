using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DB;
using Rsx.CAM;
using DB.Tools;

namespace k0X
{
    public partial class ucDetWatch : UserControl
    {
        protected delegate bool IsNullDelDettached(DataRow row);

        protected IsNullDelDettached IsNDD = null;
        protected internal int backupcounter = 0;
        protected internal bool skipCrash = false;

        protected IDetectorX preader;

        private Interface Interface = null;

        protected LINAA Linaa = null;
        protected LINAA.PeaksDataTable peaksDT;
        protected LINAA.PeaksDataTable clone = null;
        protected WatchersForm main = null;
        protected bool doSolcoi = false;
        protected int deatTimeCount = 0;
        protected int NrofFailures = 0;
        protected LINAA.SubSamplesRow currentSample = null;
        protected LINAA.MeasurementsRow currentMeas = null;
        protected IList<string> elementsList = null;
        protected TreeNode Node;

        private IDetectorX detreader;

        public LINAA.SchAcqsRow LSchAcq;
        public LINAA.SchAcqsRow NxtSchAcq;

        public ucDetWatch(ref Interface inter)
        {
            InitializeComponent();

            this.SuspendLayout();

            Interface = inter;

            main = Application.OpenForms.OfType<WatchersForm>().FirstOrDefault();
            Linaa = inter.Get();
            IsNDD = Rsx.EC.IsNuDelDetch;

            this.detBox.Items.Clear();
            this.detBox.Items.AddRange(main.DetectorsList);
            ROItimer.Enabled = false;
            peaksDT = new LINAA.PeaksDataTable(false);
            clone = new LINAA.PeaksDataTable(false);
            clone.SymColumn.ReadOnly = false;
            clone.IsoColumn.ReadOnly = false;

            this.datePicker.Value = this.datePicker.MinDate;
            Rsx.Dumb.LinkBS(ref this.peakBS, this.peaksDT, string.Empty, "Area desc");

            LINAA.k0NAADataTable k0naa = DB.Tools.WC.Populatek0NAA(true);

            this.Linaa.k0NAA.Merge(k0naa, false, MissingSchemaAction.AddWithKey);
            this.elementsList = Rsx.Dumb.HashFrom<string>(Linaa.k0NAA.SymColumn);
            TVSym.Nodes.Clear();
            foreach (string e in this.elementsList)
            {
                TreeNode node = new TreeNode(e);
                node.Name = e;
                TVSym.Nodes.Add(e);
            }
            this.elementsList.Clear();

            if (Interface.IPreferences.CurrentPref != null)
            {
                abox.Text = Interface.IPreferences.CurrentPref.windowA.ToString();
                bbox.Text = Interface.IPreferences.CurrentPref.windowB.ToString();
                srvBox.Text = Interface.IPreferences.CurrentPref.SpectraSvr;
                this.specPathbox.Text = Interface.IPreferences.CurrentPref.Spectra;
            }

            RefreshProjects();

            this.ResumeLayout(false);

            Auxiliar form = new Auxiliar();
            form.Populate(this);
            form.Show();

            DetectorX.NewLink(ref preader);
        }

        public void SetDetector(string detector)
        {
            timerAux.Interval = 8000;

            detBox.Text = string.Empty;
            detBox.Text = detector;

            Application.DoEvents();

            timerAux.Enabled = true;
        }

        private bool executingCmd = false;

        public string SetCommand(string cmd, string email)
        {
            executingCmd = true;
            string msg = string.Empty;

            if (cmd.Contains(Properties.Cmds.START)) SpectrumStart();
            else if (cmd.Contains(Properties.Cmds.CLEAR)) SpectrumClear();
            else if (cmd.Contains(Properties.Cmds.STOP)) SpectrumStop();
            else if (cmd.Contains(Properties.Cmds.SAVE)) SpectrumSave(null);
            else if (cmd.Contains(Properties.Cmds.CLOSE))
            {
                if (this.ParentForm != null) this.ParentForm.Close();
            }
            else if ((cmd.Contains(Properties.Cmds.SETPOS)))
            {
                this.posbox.Text = cmd[cmd.Length - 1].ToString();
                SpectrumClear();
                SpectrumStart();
                Application.DoEvents();
                SpectrumClear();
                this.usrbox.Text = email;
            }
            else if ((cmd.Contains(Properties.Cmds.SETSAM)))
            {
                string[] projectSam = cmd.Replace(Properties.Cmds.SETSAM, null).Split('.');
                this.probox.Text = projectSam[0].ToUpper();
                Application.DoEvents();
                this.samplebox.Text = this.probox.Text.Substring(1) + projectSam[1].ToUpper();
            }
            else if ((cmd.Contains(Properties.Cmds.SETSCH)))
            {
                this.usrbox.Text = email;
                this.stopafterbox.Text = cmd.Replace(Properties.Cmds.SETSCH, null);
                this.schedule_Click(this.schedule, EventArgs.Empty);
            }

            timerAux_Tick(timerAux, EventArgs.Empty);
            Application.DoEvents();

            msg += "Exceution of command " + cmd + " completed\nUpdated information about the ";
            string extra = string.Empty;
            if (Rsx.EC.IsNuDelDetch(LSchAcq))
            {
                msg += "measurement:\n";
                msg += GetCurrentCrashMeasString();
            }
            else
            {
                msg += "scheduled measurement:\n";
                msg += LSchAcq.GetReportString();
                extra = " (" + LSchAcq.Progress + "%)";
            }
            if (!Rsx.EC.IsNuDelDetch(NxtSchAcq))
            {
                msg += "Next scheduled measurement is:\n";
                msg += NxtSchAcq.GetReportString();
            }

            return msg;
        }

        protected void CrashDataSet()
        {
            try
            {
                if (!IsDetSrvPathOk()) return;
                string path = this.specPathbox.Text + detBox.Text;

                string elements = "None";
                string location = this.ParentForm.Location.X + ";" + this.ParentForm.Location.Y;
                if (this.elementsList.Count != 0)
                {
                    elements = string.Empty;
                    foreach (string s in this.elementsList) elements += s + ";";
                }
                string window = abox.Text + ";" + bbox.Text + ";" + bkgchbox.Text + ";" + highDT.Text;
                string[] array = new string[]
              {
                probox.Text, samplebox.Text,posbox.Text,
                elements,location,stopafterbox.Text,usrbox.Text,repeatsbox.Text,window
              };

                if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                System.IO.File.WriteAllLines(path, array);
            }
            catch (SystemException ex)
            {
                this.Linaa.AddException(ex);
            }
        }

        protected void CrashDataGet()
        {
            if (skipCrash) return;

            if (probox.IsOnDropDown) return;
            if (samplebox.IsOnDropDown) return;
            if (posbox.IsOnDropDown) return;

            if (!IsDetSrvPathOk()) return;

            //Get the previous measurement measured on this detector!!!
            string path = this.specPathbox.Text + detBox.Text;
            bool exists = System.IO.File.Exists(path);
            if (!exists) return;

            string[] array = System.IO.File.ReadAllLines(path);

            try
            {
                probox.Text = array[0];
                samplebox.Text = array[1];
                posbox.Text = array[2];

                string elements = array[3];
                RefreshElementsTV(elements);
            }
            catch (SystemException ex)
            {
                this.Linaa.AddException(ex);
            }

            try
            {
                string[] XY = array[4].Split(';');
                this.ParentForm.Location = new Point(Convert.ToInt32(XY[0]), Convert.ToInt32(XY[1]));
            }
            catch (SystemException ex)
            {
                this.Linaa.AddException(ex);
            }
            try
            {
                this.stopafterbox.Text = array[5];
                this.usrbox.Text = array[6];
                this.repeatsbox.Text = array[7];
                string[] array2 = array[8].Split(';');
                abox.Text = Convert.ToDouble(array2[0]).ToString();
                bbox.Text = Convert.ToDouble(array2[1]).ToString();
                bkgchbox.Text = Convert.ToDouble(array2[2]).ToString();
                highDT.Text = Convert.ToDouble(array2[3]).ToString();
            }
            catch (SystemException ex)
            {
                this.Linaa.AddException(ex);
            }

            skipCrash = true;
        }

        protected bool IsDetSrvPathOk()
        {
            if (string.IsNullOrWhiteSpace(detBox.Text)) return false;
            if (string.IsNullOrWhiteSpace(specPathbox.Text)) return false;
            return System.IO.Directory.Exists(this.specPathbox.Text);
        }

        protected void SetTimer(bool on, bool wasStoping)
        {
            if (this.IsDisposed) return;
            timerAux.Enabled = on;
            timerbtton.Enabled = on;
            if (wasStoping)
            {
                if (on) timerbtton.Text = "Restarting...";
                else timerbtton.Text = "Stoping...";
            }
            else
            {
                if (on) timerbtton.Text = "Timer is On";
                else timerbtton.Text = "Timer is Off";
            }
            if (on)
            {
                timerAux.Interval = Convert.ToInt32(refreshtimebox.Text) * 1000;
                timerbtton.ForeColor = Color.Red;
            }
            else
            {
                timerAux.Interval = 300 * 1000;  //not infinity, but I could...
                timerbtton.ForeColor = Color.Black;
            }
            Application.DoEvents();
        }

        /// <summary>
        /// Gets the Real, Live, DT, and Meas Start Time and returns true when succeded
        /// Called from Aux Timer, RoiCheck (with different DetectorReader) and At the start of application...
        /// </summary>
        /// <param name="reader">detector reader to use</param>
        /// <returns></returns>
        private void timerAux_Tick(object sender, EventArgs e)
        {
            SetTimer(false, false);

            if (detBox.DroppedDown) return;
            if (probox.DroppedDown) return;
            if (samplebox.DroppedDown) return;
            if (posbox.DroppedDown) return;
            if (usrbox.DroppedDown) return;
            if (this.srvBox.DroppedDown) return;
            if (this.measbox.DroppedDown) return;
            this.schProgress.Step = 1;
            this.schProgress.Value = 0;
            this.schProgress.Maximum = 0;
            this.SchedStatuslbl.Text = "Checking the Schedule...";
            this.SchedStatuslbl.ForeColor = Color.Red;

            // Application.DoEvents();

            try
            {
                //if nothing is still assigned...
                //or the system crashed...
                bool firstime = false;
                if (IsNDD(LSchAcq))
                {
                    //get first schedule in list

                    LINAA.SchAcqsRow[] schs = this.Linaa.SchAcqs.FindDetectorLastSchedule(detBox.Text);
                    LSchAcq = schs[0];
                    NxtSchAcq = schs[1];   //get second in list
                                           //if this is not null, a scheduled meas was found.. then start scheduled measurement...
                    if (!IsNDD(LSchAcq))
                    {
                        //this re-generates a new label for the measurement only when checking the schedule...
                        string lastsamPos = GetFromSchedule();
                        firstime = true;
                    }
                }

                //second check
                if (!IsNDD(LSchAcq))
                {
                    bool ok = SpectrumStart();   //if not running start it
                                                 //not started because VDM is messing up?  ok = false
                                                 //forget the schedules and retry to do it again when timer is on
                    if (!this.Start.Enabled) LSchAcq.IsAwake = true;
                    else LSchAcq.IsAwake = false;
                    if (LSchAcq.Interrupted || LSchAcq.Done || !ok)
                    {
                        LSchAcq = null;
                        if (!ok) NxtSchAcq = null;
                    }
                    else if (firstime) main.Speak("Resuming the measurement on... " + LSchAcq.Detector);
                }

                //get times from detector reader
                this.GetStatusDetector();
            }
            catch (SystemException ex)
            {
                this.SchedStatuslbl.Text = ex.Message;
                this.Linaa.AddException(ex);
            }

            SetTimer(true, false);
        }

        protected TimeSpan GetRetryIn()
        {
            LSchAcq.RefreshRate = Convert.ToInt16(0.01 * LSchAcq.PresetTime);
            int currentRate = LSchAcq.RefreshRate;
            if (!refreshtimebox.Focused)
            {
                string rt = refreshtimebox.Text;
                if (currentRate > 0 && rt.CompareTo(currentRate.ToString()) != 0)
                {
                    rt = currentRate.ToString();
                }
                refreshtimebox.Text = rt;
            }
            return new TimeSpan(0, 0, 0, 3 * currentRate);
        }

        protected void SetDatePicker(DateTime datetime)
        {
            try
            {
                this.datePicker.MaxDate = DateTime.Now.AddYears(2);
                this.datePicker.MinDate = datetime;
                this.datePicker.Value = datetime;
            }
            catch (SystemException ex)
            {
                this.Linaa.AddException(ex);
            }
        }

        protected int SetRemainingRepeats()
        {
            int remains = 0;
            remains = (LSchAcq.Repeats - LSchAcq.Counter);
            if (remains <= 0)
            {
                LSchAcq.Done = true;
                this.SchedStatuslbl.Text = "Schedule finished for sample: " + LSchAcq.Sample;
                this.SchedStatuslbl.Text += ". Last Meas: " + LSchAcq.LastMeas;
                this.SchedStatuslbl.ForeColor = Color.Green;
            }

            if (LSchAcq.Repeats > LSchAcq.Counter)
            {
                schProgress.Maximum = LSchAcq.Repeats;
                schProgress.Value = LSchAcq.Counter;
            }

            return remains;
        }

        protected string GetCurrentCrashMeasString()
        {
            string crash = string.Empty;
            crash = "Current measurement is not scheduled and " + this.StatusLbl.Text + ":\n\n";
            crash += "Project:\t" + this.probox.Text + "\n";
            crash += "Sample:\t" + this.samplebox.Text + "\n";
            crash += "Position:\t" + this.posbox.Text + "\n";
            crash += "Measurement:\t" + this.measbox.Text + "\n\n----------- ScreenShot -----------\n\n";

            bool head = true;
            try
            {
                IEnumerable<ToolStrip> tss = this.MainTLP.Controls.OfType<ToolStrip>().Where(o => o.Equals(this.DetMgrTS)).ToList();

                foreach (ToolStrip ts in tss)
                {
                    foreach (ToolStripItem s in ts.Items)
                    {
                        if (head)
                        {
                            crash += s.Text + ":\t";
                            head = false;
                        }
                        else
                        {
                            crash += s.Text + "\n";
                            head = true;
                        }
                    }
                    crash += "\n\n";
                }
            }
            catch (SystemException ex)
            {
                this.Linaa.AddException(ex);
            }
            return crash;
        }

        private void GetSpectrumTimes(ref IDetectorX reader)
        {
            //get the dead-time..
            try
            {
                reader.GetTimes();
                //key importance.. as soon as I read the times...
                //try to determine if a scheduled meas needs to stop...
                //but avoid conflicting with the Isotope Finder module..
                //therefore check if the detector reader is the same object as the Default reader
                //and that this methods was not called from the RoiTicker...
                bool IsDTHigh = RefreshTimeLabels(ref reader);
                if (!Stop.Enabled)
                {
                    SetDatePicker(DateTime.Now);
                    return;
                }
                //GET CURRENT SAMPLE AND MEAS
                string project = string.Empty;
                string sample = samplebox.Text.ToUpper();
                string meas = measbox.Text.ToUpper();
                string pos = posbox.Text;
                string det = detBox.Text;
                if (!sample.Equals(string.Empty) && !project.Equals(string.Empty))
                {
                    //get current sample
                    int? id = this.Linaa.IrradiationRequests.FindIrrReqID(project);
                    this.currentSample = this.Linaa.SubSamples.FindBySample(sample, true, id);
                    if (!IsNDD(this.currentSample))
                    {
                        this.currentSample.SetDetectorPosition(det, project);
                    }
                }
                if (!meas.Equals(string.Empty))
                {
                    //get current measurement
                    this.currentMeas = this.Linaa.Measurements.FindByMeasurementName(meas, true);
                    if (!IsNDD(this.currentMeas)) this.currentMeas.SetCAMData(ref reader);
                }

                //mid way....
                if (reader != preader) return;  //stop the worker detReader

                //check for mismatches in VDM date
                int mismatch = Convert.ToInt32(reader.VDMDelay);
                if (Math.Abs(mismatch) > 60)
                {
                    SetTimer(false, false);
                    if (IsNDD(LSchAcq) && !executingCmd)
                    {
                        string msg = "Mismatch of " + mismatch + " seconds between my estimated Measurement Start (DateTime):\t";
                        msg += reader.LastStart + "\nand Genie's respective value:\t" + reader.StartDate + "\n\n";
                        msg += "Do you want me to clear the spectrum? \n\nIf that does not work, closing genie and afterwards the VDM might help";
                        DialogResult result = MessageBox.Show(msg, "Genie's data mistmatch!", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                        if (result == DialogResult.Yes) this.SpectrumClear();
                        this.refreshtimebox.Text = "1";
                    }
                    else this.SpectrumClear();

                    SetTimer(true, false);
                    return;
                }

                executingCmd = false;

                if (!this.datePicker.Value.Equals(reader.StartDate))
                {
                    TimeSpan sm = (this.datePicker.Value - reader.StartDate);
                    if (sm.TotalSeconds < -1.0) SetDatePicker(reader.StartDate);
                }

                if (DTmode.Checked)
                {
                    SetTimer(false, false);
                    if (IsDTHigh)
                    {
                        main.Speak("Dead time is " + reader.DT + " percent!");
                        deatTimeCount = 0;
                    }
                    else deatTimeCount++;
                    if (deatTimeCount < 4)
                    {
                        this.SpectrumClear();
                    }
                    else
                    {
                        deatTimeCount = 0;
                        DTmode.Checked = false;
                    }
                    refreshtimebox.Text = "10";
                    SetTimer(true, false);

                    return;
                }
                else if (!IsNDD(this.LSchAcq))
                {
                    double Adelay = (LSchAcq.StartOn - reader.StartDate).TotalSeconds;
                    if (LSchAcq.IsMeasStartNull() && Adelay > 1)  // scheduled meas is new  and delay is positive (start on > meas start)
                    {
                        //if start date differs from startOn then clear, because the previous one was running...
                        this.SpectrumClear();
                        GetSpectrumTimes(ref reader);
                        return;
                    }
                    if (LSchAcq.IsLastMeasNull()) LSchAcq.LastMeas = string.Empty;
                    LSchAcq.MeasStart = reader.StartDate;
                    LSchAcq.CT = Convert.ToInt32(reader.CountTime);
                    LSchAcq.Cummulative = cummu.Checked;

                    GetRetryIn();

                    TimeSpan stopinMin = LSchAcq.GetStopIn();

                    int remains = SetRemainingRepeats();
                    InformProgress(remains, stopinMin);
                    //label wich is readeable...
                    if (LSchAcq.Progress < 95) ReportProgress();
                    else
                    {
                        if (LSchAcq.Progress >= 100) SpectrumScheduledSave(null);
                        else if (remains == 1)
                        {
                            if (!LSchAcq.Informed)
                            {
                                double excess = LSchAcq.PresetTime - LSchAcq.CT;
                                if (LSchAcq.PresetTime >= 3600 && excess <= 300)
                                {
                                    ReportLastMeasurement(stopinMin);
                                    LSchAcq.Informed = true;
                                }
                            }
                        }
                    }

                    this.Linaa.Save<LINAA.SchAcqsDataTable>();
                }
                //nothing was really found in the list of schedules...
                //this is a sefe please to put things to do when not scheduling!!!
                else NothingScheduled();
            }
            catch (SystemException ex)
            {
                this.Linaa.AddException(ex);
            }
        }

        private void NothingScheduled()
        {
            backupcounter++;

            if (backupcounter == 360)
            {
                bool backuped = SpectrumSave(this.Backup);
                if (backuped) backupcounter = 0;
                else
                {
                    DetectorX.NewLink(ref this.preader);
                    backupcounter = 359;
                }
            }

            int timebox = 10;
            string lbl = "Waiting for the next scheduled measurement...";
            Color col = Color.Black;
            if (!IsNDD(NxtSchAcq))
            {
                lbl = "Next scheduled measurement starts on: " + NxtSchAcq.StartOn;
                col = Color.Green;
                SetDatePicker(this.NxtSchAcq.StartOn.AddSeconds(NxtSchAcq.Repeats * NxtSchAcq.PresetTime));
                timebox = Convert.ToInt32((NxtSchAcq.StartOn - DateTime.Now).TotalSeconds * 0.1);
            }
            refreshtimebox.Text = timebox.ToString();
            this.SchedStatuslbl.Text = lbl;
            this.SchedStatuslbl.ForeColor = col;
        }

        protected void GetStatusDetector()
        {
            this.progress.Maximum = 5;
            this.progress.Value = 0;
            this.progress.Step = 1;

            try
            {
                string det = detBox.Text;
                string srv = srvBox.Text;

                if (srv.Equals(string.Empty) || det.Equals(string.Empty))
                {
                    refreshtimebox.Text = "10";
                    return;
                }

                //1
                if (srv.CompareTo("PC1533") == 0)
                {
                    if (det.CompareTo("HOBBES") == 0)
                    {
                        srvBox.Text = "PC2134";
                        srv = srvBox.Text;
                    }
                }
                else if (det.CompareTo("HOBBES") != 0)
                {
                    srvBox.Text = "PC1533";
                    srv = srvBox.Text;
                }

                //should I reload the crash data? detector name changed (compare with previous one)?
                if (preader.DetectorCodeName.CompareTo(det) != 0)
                {
                    skipCrash = false;
                    Node = null;
                    preader.Server = srv;
                    preader.DetectorCodeName = det;
                }

                if (Node == null)
                {
                    this.ParentForm.Text = det + " - Detector Watcher";
                    IEnumerable<TreeNode> nodes = main.TV.Nodes.OfType<TreeNode>();
                    Node = nodes.FirstOrDefault(o => o.Text.CompareTo(det) == 0);
                }

                //3
                CrashDataGet();
                progress.PerformStep();
                Application.DoEvents();

                preader.IsAcquiring();
                progress.PerformStep();
                Application.DoEvents();

                bool ok = CheckCmd(false);
                progress.PerformStep();
                Application.DoEvents();

                if (!ok) return;

                //4
                GetSpectrumTimes(ref this.preader);
                progress.PerformStep();
                Application.DoEvents();

                preader.SetSampleData(usrbox.Text, probox.Text, samplebox.Text, measbox.Text);
                progress.PerformStep();
                Application.DoEvents();
            }
            catch (SystemException ex)
            {
                this.Linaa.AddException(ex);
            }
        }

        protected string GetFromSchedule()
        {
            //keep old meas and return it for comparrison with the newly generated measbox.text...
            string lastsamPos = this.samplebox.Text + this.posbox.Text;
            this.probox.Text = LSchAcq.Project;
            this.samplebox.Text = LSchAcq.Sample;
            this.posbox.Text = LSchAcq.Position.ToString();
            this.repeatsbox.Text = LSchAcq.Repeats.ToString();
            this.usrbox.Text = LSchAcq.User;
            this.cummu.Checked = LSchAcq.Cummulative;

            int preset = Convert.ToInt32(LSchAcq.PresetTime);
            TimeSpan sp = new TimeSpan(0, 0, 0, preset);

            this.stopafterbox.Text = Rsx.Dumb.ToReadableString(sp);

            return lastsamPos;
        }

        protected bool ReportMeasSaved(int remains)
        {
            try
            {
                string msg = "The following measurement was saved:\n\n";
                string content = LSchAcq.GetReportString();
                string label = string.Empty;
                if (remains == 0) label = "Finished Acquisitions for ";
                else label = "Continuing Acquisition for ";
                label += LSchAcq.Sample + " (" + remains + ")";

                Interface.IReport.GenerateReport(label, string.Empty, msg + content, detBox.Text, LSchAcq.User);
            }
            catch (SystemException ex)
            {
                this.Linaa.AddException(ex);
                return false;
            }
            return true;
        }

        protected bool ReportScheduleCrashed(DateTime NewStartOn, short missed)
        {
            bool informed = false;

            try
            {
                string label = string.Empty;
                string msg = "Hello,\n\nA schedule did not (or is not) work(ing) as expected.\n";
                msg += "This happens when there is an significant delay in the overall schedule ";
                msg += "(greater or equal to 20% of the preset time).\n\n";
                msg += "In order to keep with the overall schedule, " + missed + " measurements were skipped!\n\n";

                short recoverable = missed;
                bool retry = false;
                if (!IsNDD(this.NxtSchAcq))
                {
                    double timeBeforeNext = (NxtSchAcq.StartOn - NewStartOn).TotalSeconds;
                    recoverable = Convert.ToInt16(Math.Floor((timeBeforeNext - 30) / LSchAcq.PresetTime)); //30 seconds delay
                    if (recoverable != 0) retry = true;
                }
                else retry = true;

                if (!retry)
                {
                    label = "Incompleted schedule for ";
                    msg += "I cannot retry these acquisitions because a forecoming scheduled measurement at: " + NxtSchAcq.StartOn + "\nwill overlap with this one!";
                    LSchAcq.Done = true;
                    LSchAcq.Interrupted = true;
                }

                string content = LSchAcq.GetReportString();

                if (retry)
                {
                    label = "Self-corrected schedule for ";
                    msg += "\nHowever, I found that there is enough time to catch up with " + recoverable + " missing acquisitions\n";
                    msg += "So I fixed a new schedule for this sample and I will start it right now (See below)\n\n";
                    LSchAcq.Interrupted = false;
                    LSchAcq.StartOn = NewStartOn;
                    LSchAcq.NextStartOn = LSchAcq.StartOn.AddSeconds(LSchAcq.PresetTime);
                    LSchAcq.Repeats = recoverable;
                    LSchAcq.Progress = 0;
                    LSchAcq.IsAwake = false;
                    LSchAcq.Counter = 0;
                    LSchAcq.Saved = 0;
                    LSchAcq.CT = 0;
                    LSchAcq.Informed = false;
                    LSchAcq.SetMeasStartNull();

                    msg += LSchAcq.GetReportString(); //write updated content
                    msg += "\n\nThis will override the previous incompleted schedule:\n\n";
                }

                msg += content;
                label += LSchAcq.Sample;

                Interface.IReport.GenerateReport(label, string.Empty, msg, detBox.Text, LSchAcq.User);

                informed = true;
            }
            catch (SystemException ex)
            {
                this.Linaa.AddException(ex);
            }
            return informed;
        }

        protected bool ReportLastMeasurement(TimeSpan stopinMin)
        {
            //time to warn user of changing samples??
            //find next schedule on row to send an email about that too!!!
            bool informed = false;
            try
            {
                string stoprem = Rsx.Dumb.ToReadableString(stopinMin);
                string content = string.Empty;
                string user = LSchAcq.User;
                string msg = "\n\nAcquisition for this detector is about to finish in " + stoprem + "\n\n";

                if (IsNDD(NxtSchAcq))
                {
                    msg += "However no more scheduled measurements were found.\n";
                    msg += "Detector is free after completition ;)";
                    main.Speak("Detector is free after completition");
                }
                else
                {
                    msg += "Next measurement scheduled for this detector starts at:\t" + NxtSchAcq.StartOn + "\n\n";
                    msg += "Information about forecoming acquisition for this detector is:\n";
                    content = NxtSchAcq.GetReportString() + "\n\n";
                    user = NxtSchAcq.User;
                    content += "I'll inform " + user + " on due time";
                    main.Speak("Next measurement for... " + detBox.Text + "... starts at: " + NxtSchAcq.StartOn);
                }

                Interface.IReport.GenerateReport("Acquisition about to finish", string.Empty, msg + content, detBox.Text, user);

                informed = true;

                main.Speak(detBox.Text + "... is about to finish in... " + Decimal.Round(Convert.ToDecimal(stopinMin.TotalMinutes), 1) + "... minutes");
            }
            catch (SystemException ex)
            {
                this.Linaa.AddException(ex);
            }
            return informed;
        }

        protected void InformProgress(int remains, TimeSpan stopinMin)
        {
            string stoprem = Rsx.Dumb.ToReadableString(stopinMin);
            string stlbl = "Progress " + LSchAcq.Progress + "%. ";
            if (remains == 1) stlbl += "Last meas. Saving in ";
            else stlbl += remains + " meas. to do. Saving in ";
            stlbl += stoprem;
            stlbl += ". Finish on " + LSchAcq.StartOn.AddSeconds(LSchAcq.PresetTime * LSchAcq.Repeats).ToString("g", provider);

            this.SchedStatuslbl.Text = stlbl;
            this.SchedStatuslbl.ForeColor = Color.Red;

            Application.DoEvents();
        }

        protected void ReportProgress()
        {
            int step = 20;
            for (int x = 0; x < 100; x = x + step)
            {
                if (x == 0) continue;
                if (LSchAcq.Progress != x) continue;
                if (LSchAcq.PresetTime <= 3600) continue;

                SpectrumScheduledSave(this.Backup);

                if (x == 80 || x == 20 || x == 60)
                {
                    string content = LSchAcq.GetReportString() + "\n\n";
                    string msg = "Checkpoint!\n\nUpdated information about the scheduled measurement:\n\n";
                    Interface.IReport.GenerateReport("Checkpoint (" + LSchAcq.Progress + "%)", string.Empty, msg + content, detBox.Text, "ffarina@sckcen.be");
                }
                break;
            }
        }

        protected void ReportFailure()
        {
            string message = "Cannot take full control of the detector.\nSomewhere a computer has locked this detector (opened through Genie)...\nPlease close the detector on that computer and try again...\n\n";
            string title = " When accessing " + detBox.Text;
            string user = "ffarina@sckcen.be";
            string content = string.Empty;
            if (!IsNDD(LSchAcq))
            {
                LSchAcq.NotCrashed = false;
                LSchAcq.IsAwake = false;
                this.Linaa.Save<LINAA.SchAcqsDataTable>();
                TimeSpan retryIn = GetRetryIn();
                user = LSchAcq.User;
                content = "I will retry to access it in " + retryIn.ToString() + "\nThis corresponds to a 3% progress of\nthe scheduled measurement:\n\n";
                content += LSchAcq.GetReportString();
                //so it comes back at 2 percent + little bit late and forces the opening
                main.SchTimer.Interval = retryIn.Seconds * 1000;
                main.SchTimer.Enabled = true;
            }
            else content = this.GetCurrentCrashMeasString();

            Interface.IReport.GenerateReport("Cannot take control of " + detBox.Text, string.Empty, message + content, string.Empty, user);
        }

        private void ROItimer_Tick(object sender, EventArgs e)
        {
            this.ROItimer.Enabled = false;
            if (this.findbutton.Text.Contains("Find")) return;

            try
            {
                GetSpectrumTimes(ref detreader);  //get times again...

                this.clone.Clear();
                // this.clone.Merge(this.peaksDT);

                RefreshElementsList();
            }
            catch (SystemException ex)
            {
                this.StatusLbl.Text = ex.Message;
                this.StatusLbl.ForeColor = Color.Red;
                this.Linaa.AddException(ex);
            }

            double a = 0.4;
            double b = 0.001;
            double minUnc = 0.7;
            double bkgCh = 5;

            try
            {
                if (!abox.Text.Equals(string.Empty) && !abox.Focused) a = Convert.ToDouble(abox.Text);
                if (!bbox.Text.Equals(string.Empty) && !bbox.Focused) b = Convert.ToDouble(bbox.Text);
                if (!minuncbox.Text.Equals(string.Empty) && !minuncbox.Focused) minUnc = Convert.ToDouble(minuncbox.Text);
                if (!bkgchbox.Text.Equals(string.Empty) && !bkgchbox.Focused) bkgCh = Convert.ToDouble(bkgchbox.Text);
            }
            catch (SystemException ex)
            {
                Interface.IReport.Msg("Inputs are not in the correct format", "Check the ROI Options", false);
                this.Linaa.AddException(ex);
            }

            try
            {
                object[] parms = new object[7];
                parms[0] = detBox.Text;
                parms[1] = elementsList.ToList(); //refreshed!!!
                parms[2] = detreader;
                parms[3] = a;
                parms[4] = b;
                parms[5] = minUnc;
                parms[6] = bkgCh;

                System.ComponentModel.BackgroundWorker worker = new System.ComponentModel.BackgroundWorker();
                worker.DoWork += new System.ComponentModel.DoWorkEventHandler(ROIworker_DoWork);
                worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ROIworker_RunWorkerCompleted);
                worker.RunWorkerAsync(parms);
            }
            catch (SystemException ex)
            {
                this.StatusLbl.Text = ex.Message;
                this.StatusLbl.ForeColor = Color.Red;
                this.Linaa.AddException(ex);
            }
            Application.DoEvents();
        }

        private void ROIworker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result == null) return;
            if (!e.Result.GetType().Equals(typeof(object[]))) return;

            try
            {
                DetectorX ireader = ((object[])e.Result)[2] as DetectorX;
                this.peaksDT.Clear();
                if (this.currentSample != null)
                {
                    this.Linaa.Peaks.Merge(clone, false);
                    LINAA.GeometryRow reference = this.Linaa.DefaultGeometry;
                    if (reference != null)
                    {
                        DB.Tools.WC.SetCOINSolid(this.currentSample, true, true, ref reference);
                        DB.Tools.WC.SetCOINSolid(this.currentSample, false, true, ref reference);
                    }
                    List<LINAA.SubSamplesRow> ls = new List<LINAA.SubSamplesRow>();
                    ls.Add(this.currentSample);
                    IEnumerable<LINAA.SubSamplesRow> samples = ls;
                    DB.Tools.WC.Calculate(ref samples, false, false, false);
                    ls.Clear();
                    ls = null;
                }
                else this.peaksDT.Merge(clone);
            }
            catch (SystemException ex)
            {
                this.Linaa.AddException(ex);
            }

            try
            {
                //recover peaks
                IEnumerable<LINAA.PeaksRow> peaks = this.Linaa.Peaks.Where(p => p.MeasurementsRow == this.currentMeas);
                if (peaks.Count() != 0) this.peaksDT.Load(peaks.CopyToDataTable().CreateDataReader(), LoadOption.OverwriteChanges);
                int time = Convert.ToInt16(this.rOIRefresh.Text) * 1000;
                ROItimer.Interval = time;
            }
            catch (SystemException ex)
            {
                this.Linaa.AddException(ex);
            }

            IDisposable i = sender as IDisposable;
            i.Dispose();
            ROItimer.Enabled = true;
        }

        /// <summary>
        /// for post processing, basically speaking
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ROIworker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            System.ComponentModel.BackgroundWorker worker = sender as System.ComponentModel.BackgroundWorker;

            object[] parms = e.Argument as object[];

            string detector = parms[0].ToString();
            IList<string> elements = parms[1] as IList<string>;
            DetectorX ireader = parms[2] as DetectorX;
            double a = (double)parms[3];
            double b = (double)parms[4];
            double minUnc = (double)parms[5];
            double bkgCh = (double)parms[6];

            HashSet<double> Fullenergylist = new HashSet<double>();

            foreach (string sym in elements)
            {
                IEnumerable<LINAA.k0NAARow> isotopesnaa = Linaa.k0NAA.Where(LINAA.SelectorAnalyteBy<LINAA.k0NAARow>(sym, string.Empty, 0));
                IList<Int32> isotopes = Rsx.Dumb.HashFrom<Int32>(isotopesnaa, Linaa.k0NAA.NAAIDColumn.ColumnName);
                foreach (Int32 iso in isotopes)
                {
                    try
                    {
                        LINAA.IRequestsAveragesRow irs = this.Linaa.IRequestsAverages.FirstOrDefault(o => o.NAAID == iso);
                        if (IsNDD(irs))
                        {
                            this.Linaa.IRequestsAverages.NewIRequestsAveragesRow(iso, ref this.currentSample);
                        }
                    }
                    catch (SystemException ex)
                    {
                        this.Linaa.AddException(ex);
                    }

                    IEnumerable<LINAA.k0NAARow> energies = isotopesnaa.Where(o => o.NAAID == iso);
                    if (energies.Count() == 0) continue;
                    IList<Int32> k0IDList = Rsx.Dumb.HashFrom<Int32>(energies, Linaa.k0NAA.IDColumn.ColumnName);

                    double windoe;
                    double energylow;
                    double energyhigh;

                    foreach (Int32 k0ID in k0IDList)
                    {
                        LINAA.k0NAARow k0na = energies.FirstOrDefault(o => o.ID == k0ID);

                        if (k0na != null)
                        {
                            double energy = k0na.Energy;
                            try
                            {
                                LINAA.IPeakAveragesRow ip = this.Linaa.IPeakAverages.FirstOrDefault(o => o.k0ID == k0ID);
                                if (IsNDD(ip)) ip = this.Linaa.IPeakAverages.NewIPeakAveragesRow(k0ID, ref this.currentSample);
                            }
                            catch (SystemException ex)
                            {
                                this.Linaa.AddException(ex);
                            }
                            try
                            {
                                windoe = a + (b * energy);
                                energylow = energy - windoe;
                                energyhigh = energy + windoe;
                                LINAA.PeaksRow peak = null;
                                peak = this.clone.FirstOrDefault(o => !o.Isk0IDNull() && (o.k0ID == k0ID));
                                if (IsNDD(peak))
                                {
                                    peak = this.clone.NewPeaksRow(k0ID, energy, ref this.currentSample, ref this.currentMeas);
                                }

                                LINAA.SetROIInfo(ref peak, ref ireader, minUnc, bkgCh, energylow, energyhigh);
                                peak.Sym = k0na.Sym;
                                peak.Iso = k0na.Iso;
                            }
                            catch (SystemException ex)
                            {
                                this.Linaa.AddException(ex);
                            }

                            Fullenergylist.Add(energy);
                        }
                    }
                }
            }

            CalculateSolCoin(Fullenergylist);

            e.Result = parms;
        }

        protected void CalculateSolCoin(HashSet<double> Fullenergylist)
        {
            try
            {
                if (!doSolcoi) return;

                if (Rsx.EC.IsNuDelDetch(this.currentMeas)) return;
                if (Rsx.EC.IsNuDelDetch(this.currentSample)) return;

                DB.Tools.SolCoin Solcoin = new DB.Tools.SolCoin(ref this.Linaa);
                string[] posgeodetfillrad = new string[] {
                  this.currentMeas.Position.ToString(),
                  this.currentSample.GeometryName,
                  this.currentMeas.Detector,
                  this.currentSample.FillHeight.ToString(),
                  this.currentSample.Radius.ToString(),
                  this.currentSample.SubSampleName };

                main.Speak("Calculating efficiencies and COI factors...");

                bool success = Solcoin.PrepareSampleUnit(posgeodetfillrad, Fullenergylist.ToArray(), true, false);
                if (success) Solcoin.DoAll(true);
                doSolcoi = false;

                List<LINAA.MeasurementsRow> ls2 = new List<LINAA.MeasurementsRow>();
                ls2.Add(this.currentMeas);
                IEnumerable<LINAA.MeasurementsRow> meas = ls2.AsEnumerable();
                object o = this.Linaa.Solang;
                DB.Tools.WC.PopulateCOINSolang(false, ref meas, ref o);
                o = this.Linaa.COIN;
                DB.Tools.WC.PopulateCOINSolang(true, ref meas, ref o);
            }
            catch (SystemException ex)
            {
                this.Linaa.AddException(ex);
            }
        }

        private void Command_Click(object sender, EventArgs e)
        {
            SetTimer(false, true);  //important to keep so timer does not jump again
            bool ok = false;

            try
            {
                if (sender.Equals(this.Save))
                {
                    ok = SpectrumSave(sender);
                }
                else if (sender.Equals(this.Start))
                {
                    ok = SpectrumStart();
                }
                else if (sender.Equals(this.Stop))
                {
                    ok = SpectrumStop();
                }
                else if (sender.Equals(this.Clear))
                {
                    ok = SpectrumClear();
                }
            }
            catch (SystemException ex)
            {
                this.Linaa.AddException(ex);
                this.StatusLbl.Text = ex.Message;
            }

            if (!ok)
            {
                this.StatusLbl.Text = "Something was not ok when executing that command";
                refreshtimebox.Text = "15";
            }
            else refreshtimebox.Text = "3";

            SetTimer(true, true);
        }

        protected void SpectrumScheduledSave(object sender)
        {
            //reset the measbox.text to the new meas to save...
            //i rather do not alter the boxes...
            try
            {
                timerbtton.Text = "Saving...";
                timerbtton.ForeColor = Color.Black;
                string currentSample = samplebox.Text;
                string currentPosition = posbox.Text;
                //sample,pos and measbox gets reseted to the one from the schedule... ;)
                string whatevameas = this.GetFromSchedule(); //the previous meas is not necessary to keep, it will be regenerated
                                                             //here, because afterwards it would have changed name..
                                                             //a new meas label is assigned after saving!!! this is backup in case meas is NOT saved
                                                             // Application.DoEvents();

                bool saved = SpectrumSave(sender);

                if (sender == null) refreshtimebox.Text = "5";

                samplebox.Text = currentSample; //return the ones the user was playing with...
                posbox.Text = currentPosition;  //return the ones the user was playing with...
            }
            catch (SystemException ex)
            {
                this.Linaa.AddException(ex);
            }
        }

        private bool SpectrumClear()
        {
            preader.Clear();
            return CheckCmd(true);
        }

        private bool SpectrumStart()
        {
            if (!this.Start.Enabled) return true;
            if (!cummu.Checked) SpectrumClear();  //clear first //if VDM problem never appears again this is a safer method then!!!
            preader.Start();  //start it...
            return CheckCmd(false);
        }

        /// <summary>
        /// Checks the status of any CAM exception arosed when a command was sent to the detector
        /// </summary>
        /// <param name="clearing">True if the order was to clear the detector</param>
        /// <returns>Returns true if all is good</returns>
        protected bool CheckCmd(bool clearing)
        {
            string oldStatus = this.StatusLbl.Text;

            if (preader.Exception != null)
            {
                this.StatusLbl.Text = "Cannot control the detector. Check it is on Read-only or restart the VDM";
                this.StatusLbl.ToolTipText = "Error: " + preader.Exception.Message;
                this.StatusLbl.ForeColor = Color.Red;
                this.peaksDataGridView.BackgroundColor = Color.MistyRose;
                this.toolStrip2.BackColor = Color.MistyRose;
                if (Node != null) Node.ForeColor = Color.Orange;
                Application.DoEvents();

                NrofFailures++;
                int a = NrofFailures;

                //notify failure during these intervals...
                if (a == 2 || a == 20 || a == 30 || a == 40 || a == 59 || a == 65)
                {
                    Interface.IReport.Msg(this.StatusLbl.Text, " when accessing " + detBox.Text, false);
                    if (a == 65)
                    {
                        DetectorX.KillVDM(this.preader.Server);
                        NrofFailures = 1;
                    }
                    DetectorX.NewLink(ref this.preader);
                }
                else if (a == 60)  //last warning message, send the email of failure
                {
                    Interface.IReport.Msg(this.StatusLbl.Text, " when accessing " + detBox.Text, false);
                    this.Linaa.AddException(preader.Exception);
                    ReportFailure();
                }
            }
            else //no failure this time
            {
                //was the program coming from previous failures??
                if (NrofFailures != 0)   //important so it does not repet the stupid msg
                {
                    NrofFailures = 0;      //reset the counter...
                    Interface.IReport.Msg("Detector is controlled!", " with " + detBox.Text, true);
                }
                RefreshADCLabels(clearing);
            }

            return (NrofFailures == 0);
        }

        /// <summary>
        /// Never called by the scheduler. Can only be called by UI
        /// </summary>
        private bool SpectrumStop()
        {
            if (!this.Stop.Enabled) return true;

            bool normal = true;
            DialogResult result;

            if (!IsNDD(LSchAcq)) //there was a schedule!! warning
            {
                //ask the user to make sure the scheduled meas must be stoped
                result = MessageBox.Show("The detector is acquiring a scheduled measurement\nAre you sure you want to stop it?", "A conflictive command was detected", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.No) return false;
                else normal = false;
                //ask the user if he wants to save the acquisition...
            }

            preader.Stop();  //stop it
            bool ok = CheckCmd(false);

            if (!normal)    //after user confirmed schedule must be stoped and the stoppped went ok
            {
                result = MessageBox.Show("Do you want to save the spectrum for this scheduled measurement?\nIf you saved it already, click NO", "Just in case...", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes) this.SpectrumScheduledSave(null);
                else LSchAcq.Counter++;

                if (!IsNDD(LSchAcq))
                {
                    int remains = LSchAcq.Repeats - LSchAcq.Counter;
                    bool yes = true;
                    if (remains > 0)
                    {
                        result = MessageBox.Show("There are still " + remains + " repeats scheduled for this measurement.\nIf you want to interrupt the whole schedule press YES\n\nIf you want to keep it and go to the next repeat press NO", "A conflict was detected", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == DialogResult.No) yes = false;
                    }
                    LSchAcq.Interrupted = yes;
                    if (yes && !LSchAcq.Done) LSchAcq.Done = yes;
                    this.Linaa.Save<LINAA.SchAcqsDataTable>();
                    LSchAcq = null;
                }
                if (ok)
                {
                    this.StatusLbl.Text = "Scheduled measurement was stopped before completition. " + this.StatusLbl.Text;
                    this.StatusLbl.ForeColor = Color.Red;
                }
            }

            return ok;
        }

        private bool SpectrumSave(object sender)
        {
            //returns TRUE if it can continue...

            if (measbox.Text.CompareTo(string.Empty) == 0)
            {
                string message = "Please assign a Measurement name first!.It cannot be empty";
                this.StatusLbl.Text = message;
                this.StatusLbl.ForeColor = Color.Red;
                SpectrumSaveBackup(detBox.Text + ".CNF");
                return false;
            }

            if (sender != null && sender.Equals(this.Backup))
            {
                SpectrumSaveBackup(measbox.Text + ".CNF");
                return true;
            }

            string filepath = this.specPathbox.Text + probox.Text + "\\" + measbox.Text + ".CNF";

            if (System.IO.File.Exists(filepath))  //already exists...
            {
                DialogResult result;
                if (sender == null) result = DialogResult.No;
                else
                {
                    string message = "This measurement (" + measbox.Text + ") label already exist\n\nDo you want to overwrite it?";
                    string title = "Found another measurement with the same label...";
                    result = MessageBox.Show(message, title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                }
                if (result == DialogResult.No)
                {
                    measlabel_Click(sender, EventArgs.Empty);
                    SpectrumSave(sender);
                    return false; //keeeps trying to save until measurement label is brand new..
                }
                else if (result == DialogResult.Cancel)
                {
                    this.StatusLbl.Text = "Measurement " + measbox.Text + " saving process was cancelled!";
                    this.StatusLbl.ForeColor = Color.Red;
                    return false;
                }
            }

            bool saved = preader.Save(filepath);
            NrofFailures = 59;
            bool ok = CheckCmd(false);
            if (saved && ok)
            {
                this.lastsvdlbl.ToolTipText = this.preader.LastFileTag;

                string speak = "Saved!... detector: " + detBox.Text[0];
                if (!IsNDD(LSchAcq))
                {
                    LSchAcq.LastMeas = measbox.Text;
                    LSchAcq.Saved++;
                    LSchAcq.Counter++;  //add meas to counter
                    int remains = SetRemainingRepeats();
                    bool clear = ((!cummu.Checked && sender == null && !LSchAcq.Done) || !LSchAcq.NotCrashed);
                    DateTime newStartOn = DateTime.Now;
                    if (clear) this.SpectrumClear();
                    if (!LSchAcq.NotCrashed)
                    {
                        Int16 missed = Convert.ToInt16(LSchAcq.Repeats - LSchAcq.Saved);
                        if (missed != 0) ReportScheduleCrashed(newStartOn, missed);
                    }
                    else
                    {
                        ReportMeasSaved(remains);
                        speak += "... Remaining measurements... " + remains;
                        if (LSchAcq.Done) LSchAcq = null; //now is that you can kill the scheduled task.
                    }

                    this.Linaa.Save<LINAA.SchAcqsDataTable>();
                }
                main.Speak(speak);
                this.StatusLbl.Text = "Saved! Measurement " + measbox.Text;
                this.StatusLbl.ForeColor = Color.Green;
            }

            //generate the new label for next measurement!!!
            measlabel_Click(sender, EventArgs.Empty);

            return saved;
        }

        protected bool SpectrumSaveBackup(string detfile)
        {
            string backupPath = this.Linaa.FolderPath + DB.Properties.Resources.Backups;
            try
            {
                if (System.IO.File.Exists(backupPath + detfile)) System.IO.File.Delete(backupPath + detfile);
                bool exist = preader.Save(backupPath + detfile);
                NrofFailures = 59;
                if (CheckCmd(false) && exist)
                {
                    System.IO.FileInfo f = preader.LastFileInfo;
                    this.StatusLbl.Text = "Backup made on " + f.LastWriteTime;
                    this.StatusLbl.ToolTipText = "Saved on: " + f.LastWriteTime + "\nSize: " + f.Length + "\nPath: " + f.FullName;
                    this.StatusLbl.ForeColor = Color.Green;
                    preader.LastFileInfo = null;
                }
                else
                {
                    this.StatusLbl.Text = "Backup NOT made on " + DateTime.Now;
                    this.StatusLbl.ForeColor = Color.Red;
                }
            }
            catch (SystemException ex)
            {
                this.Linaa.AddException(ex);
            }
            return System.IO.File.Exists(backupPath + detfile);
        }

        private IFormatProvider provider = System.Globalization.CultureInfo.InstalledUICulture;

        private bool RefreshTimeLabels(ref IDetectorX reader)
        {
            //then waste time painting...

            int highDTvalue = 40;
            decimal dt = reader.DT;
            bool IsDTHigh = false;
            if (!this.highDT.TextBox.Focused) highDTvalue = Convert.ToInt16(this.highDT.Text);
            if (dt > highDTvalue)
            {
                IsDTHigh = true;
                DeadTlbl.ForeColor = Color.Red;
                this.StatusLbl.Text += ". Dead Time is high!";
            }
            else DeadTlbl.ForeColor = Color.Black;
            this.DeadTlbl.Text = "DT " + dt + "%";
            this.measStartlbl.Text = "Started on " + reader.StartDate.ToString("g", provider);

            TimeSpan lt = new TimeSpan(0, 0, 0, Convert.ToInt32(reader.LiveTime));
            TimeSpan ct = new TimeSpan(0, 0, 0, Convert.ToInt32(reader.CountTime));
            TimeSpan preset = new TimeSpan(0, 0, 0, Convert.ToInt32(reader.PresetTime));

            this.LiveTlbl.Text = "Live " + Rsx.Dumb.ToReadableString(lt);
            this.RealTlbl.Text = "Real " + Rsx.Dumb.ToReadableString(ct);
            this.prsTlbl.Text = "Preset " + Rsx.Dumb.ToReadableString(preset);

            return IsDTHigh;
        }

        protected void RefreshElementsTV(string elements)
        {
            if (elements != null && elements.Count() != 0 && !elements.Contains("None"))
            {
                foreach (TreeNode n in TVSym.Nodes) n.Checked = false;
                string[] eles = elements.Split(';');
                foreach (string s in eles)
                {
                    if (s.Equals(string.Empty)) continue;
                    TreeNode ns = this.TVSym.Nodes.OfType<TreeNode>().Where(o => o.Text.Equals(s)).FirstOrDefault();
                    if (ns != null) ns.Checked = true;
                }
            }
        }

        protected void RefreshSamples(bool clean)
        {
            try
            {
                if (samplebox.Items.Count != 0) samplebox.Items.Clear();
                IList<string> samples = ucWatchDog.GetSamples(specPathbox.Text, probox.Text);
                samplebox.Items.AddRange(samples.ToArray());
                if (clean)
                {
                    if (!measbox.Text.Equals(string.Empty)) measbox.Text = string.Empty;
                    if (!samplebox.Text.Equals(string.Empty)) samplebox.Text = string.Empty;
                }
            }
            catch (SystemException ex)
            {
                this.Linaa.AddException(ex);
            }
        }

        protected void RefreshElementsList()
        {
            this.elementsList.Clear();
            IList<string> list = this.TVSym.Nodes.OfType<TreeNode>().Where(o => o.Checked).Select(o => o.Text).ToList();
            this.elementsList = new HashSet<string>(list.ToArray()).ToList();
        }

        protected void RefreshProjects()
        {
            try
            {
                probox.Items.Clear();
                probox.Items.AddRange(main.SpectraDirectories.ToArray());
            }
            catch (SystemException ex)
            {
                this.Linaa.AddException(ex);
            }
        }

        private void RefreshADCLabels(bool clearing)
        {
            Start.Enabled = !preader.Acquiring;
            Stop.Enabled = preader.Acquiring;
            Save.Enabled = true;
            Clear.Enabled = true;

            this.toolStrip2.BackColor = Color.Honeydew;

            if (Start.Enabled)
            {
                this.peaksDataGridView.BackgroundColor = Color.Honeydew;
                if (Node != null) Node.ForeColor = Color.Green;
                this.StatusLbl.Text = detBox.Text + " is Stopped";
                this.StatusLbl.ForeColor = Color.Black;
            }
            else
            {
                this.peaksDataGridView.BackgroundColor = Color.MistyRose;

                if (clearing)
                {
                    this.StatusLbl.Text = detBox.Text + " is being cleared!";
                    refreshtimebox.Text = "2";
                }
                else
                {
                    this.StatusLbl.Text = detBox.Text + " is Acquiring...";
                }

                if (Node != null) Node.ForeColor = Color.Red;
                this.StatusLbl.ForeColor = Color.Red;
            }
        }

        private void Hide_Click(object sender, EventArgs e)
        {
            if (Hide.Text.CompareTo("Hide") == 0)
            {
                int orgHeight = 0;
                orgHeight = this.MainTLP.Size.Height;
                this.MainTLP.Controls.Remove(SC2);
                Hide.Text = "View";
                this.AutoSize = true;
                this.ParentForm.Size = new Size(this.ParentForm.Width, (DetMgrTS.Height + MeasTS.Height + 2 * AutoTS.Height));
            }
            else
            {
                this.MainTLP.Controls.Add(SC2);
                this.AutoSize = false;
                Hide.Text = "Hide";
                this.ParentForm.Size = new Size(this.ParentForm.Width, this.ParentForm.Size.Height);
            }
        }

        private void findbutton_Click(object sender, EventArgs e)
        {
            if (this.findbutton.Text.Contains("Find"))
            {
                if (this.elementsList.Count == 0)
                {
                    Interface.IReport.Msg("Select at least 1 element of the list", "Select at least 1 element!", false);
                }
                else
                {
                    string det = detBox.Text;
                    bool started = SpectrumStart();
                    if (started)
                    {
                        this.findbutton.Image = Properties.Resources.Atom;
                        this.findbutton.Text = "Click to Pause...";
                        if (detreader == null)
                        {
                            detreader = new DetectorX();
                            detreader.Server = this.srvBox.Text;
                            detreader.DetectorCodeName = det;
                        }
                        peaksDT.Clear();
                        doSolcoi = true;
                        Interface.IReport.Msg("Watching peaks on " + det + " every " + rOIRefresh.Text + " seconds", "ROI Lookup started", true);
                        this.ROItimer.Interval = 1000;
                        ROItimer.Enabled = true;
                    }
                    else this.findbutton.Text = "Start Detector first";
                }
            }
            else
            {
                ROItimer.Enabled = false;
                this.findbutton.Image = Properties.Resources.Atom;
                this.findbutton.Text = "Find Isotopes";
            }
        }

        private void measlabel_Click(object sender, EventArgs e)
        {
            if (posbox.Text.Equals(string.Empty))
            {
                posbox.Text = posbox.Items.OfType<string>().First();
                return;
            }

            if (measbox.Items.Count != 0) measbox.Items.Clear();

            if (samplebox.Text.Equals(string.Empty) || detBox.Text.Equals(string.Empty))
            {
                if (!measbox.Text.Equals(string.Empty)) measbox.Text = string.Empty;
                return;
            }

            IList<string> meas = ucWatchDog.GetMeasurements(specPathbox.Text, probox.Text, samplebox.Text);

            measbox.Items.AddRange(meas.ToArray());

            string prefix = samplebox.Text + detBox.Text[0] + posbox.Text;
            prefix = prefix.ToUpper();

            measbox.Text = Rsx.Dumb.GetNextName(prefix, meas, true);

            meas = null;

            Application.DoEvents();

            CrashDataSet();

            FindTVElement();
        }

        private void FindTVElement()
        {
            try
            {
                if (findbutton.Text.Contains("Find"))
                {
                    string sample = samplebox.Text.ToUpper();
                    string rex = System.Text.RegularExpressions.Regex.Replace(sample, "[0-9]", "");
                    if (string.IsNullOrWhiteSpace(rex)) return;

                    TreeNode node = this.TVSym.Nodes.OfType<TreeNode>().Where(n => n.Text.ToUpper().CompareTo(rex) == 0).FirstOrDefault();
                    if (node == null) return;
                    if (node.Checked) return;
                    foreach (TreeNode n in this.TVSym.Nodes)
                    {
                        if (n != node) n.Checked = false;
                        else n.Checked = true;
                    }
                }
            }
            catch (SystemException ex)
            {
                this.Linaa.AddException(ex);
            }
        }

        private void probox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender != null) RefreshSamples(true);
            string path = this.specPathbox.Text + probox.Text;
            if (System.IO.Directory.Exists(path))
            {
                this.probox.BackColor = Color.White;
                this.probox.ToolTipText = "This directory was found!";
                this.SchedStatuslbl.Text = "The Project directory exists!";
                this.SchedStatuslbl.ForeColor = Color.Green;
            }
            else
            {
                this.probox.BackColor = Color.MistyRose;
                this.probox.ToolTipText = "I cannot find this directory. Try clicking <Project>";
                this.SchedStatuslbl.Text = "Cannot find the Project directory!";
                this.SchedStatuslbl.ForeColor = Color.Red;
            }
        }

        private void measbox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (measbox.Equals(string.Empty))
                {
                    this.SchedStatuslbl.Text = "Measurement name is empty";
                    this.SchedStatuslbl.ForeColor = Color.Orange;
                    return;
                }
                //  preader.GetSampleData(); not used but could be used to get the data instead of the file.

                bool alreadyexist = System.IO.File.Exists(this.specPathbox.Text + probox.Text + "\\" + measbox.Text + ".CNF");
                if (alreadyexist)
                {
                    this.SchedStatuslbl.Text = "That measurement already exists. You'll be reminded again when saving";
                    this.SchedStatuslbl.ForeColor = Color.Orange;
                }
                else
                {
                    this.SchedStatuslbl.Text = "Measurement name looks OK for: " + measbox.Text;
                    this.SchedStatuslbl.ForeColor = Color.Green;
                }
            }
            catch (SystemException ex)
            {
                this.StatusLbl.Text = "Error when generating measurement name: " + ex.Message;
                this.StatusLbl.ForeColor = Color.Red;
            }

            Application.DoEvents();
        }

        private void Datebox_DoubleClick(object sender, EventArgs e)
        {
            this.datePicker.Value = DateTime.Now;
        }

        private void EnabledSch_Click(object sender, EventArgs e)
        {
            timerAux.Enabled = false;
            SetTimer(true, false);
        }

        private void peaksDataGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DataGridViewRow row = this.peaksDataGridView.Rows[e.RowIndex];

            LINAA.PeaksRow p = ((DataRowView)(row.DataBoundItem)).Row as LINAA.PeaksRow;

            if (p.Ready && p.Area != 0) row.DefaultCellStyle.BackColor = Color.Honeydew;
            else if (p.Ready) row.DefaultCellStyle.BackColor = Color.LightGray;
            else if (p.AreaUncertainty < 3 * Convert.ToDouble(minuncbox.Text))
            {
                row.DefaultCellStyle.BackColor = Color.PapayaWhip;
            }
            else row.DefaultCellStyle.BackColor = Color.MistyRose;
        }

        private void TVSym_AfterCheck(object sender, TreeViewEventArgs e)
        {
            RefreshElementsList();
            CrashDataSet();
        }

        private void schedule_Click(object sender, EventArgs e)
        {
            TimeSpan pre = Rsx.Dumb.ToReadableTimeSpan(stopafterbox.Text);
            double preset = pre.TotalSeconds;
            if (preset == 0)
            {
                MessageBox.Show("Preset time was not in the correct format: ##d##h##m##s \nPlease try again");
                return;
            }

            if (preader.PresetTime < preset)
            {
                MessageBox.Show("Preset time IN the detector MCA is less than the scheduled preset time.\nPlease change that using Genie because I still have not learned how to do it!");
                return;
            }

            SetTimer(false, false);

            try
            {
                Int16 repeats = Convert.ToInt16(repeatsbox.Text);
                Int16 position = Convert.ToInt16(posbox.Text);
                DateTime st = this.datePicker.Value;
                DateTime final = st.AddSeconds(repeats * preset);
                DateTime now = DateTime.Now;
                if (DateTime.Compare(now, final) >= 0)
                {
                    MessageBox.Show("The schedule will be finished at " + final.ToString() + "\n which is earlier (or equal) to just now: " + now.ToString() + "\nPlease make sure you cleared the current spectrum or that your selection is ok and try again");
                    return;
                }

                this.Linaa.AddSchedule(probox.Text, samplebox.Text, position, detBox.Text, repeats, preset, st, usrbox.Text, cummu.Checked, executingCmd);

                refreshtimebox.Text = "1";
                executingCmd = false;
            }
            catch (SystemException ex)
            {
                this.SchedStatuslbl.Text = ex.Message;
                this.Linaa.AddException(ex);
            }
            SetTimer(true, false);
        }

        private void prolbl_Click(object sender, EventArgs e)
        {
            if (probox.Text.Equals(string.Empty)) return;
            RefreshSamples(false);
            if (System.IO.Directory.Exists(this.specPathbox.Text + probox.Text))
            {
                Rsx.Dumb.Process(new System.Diagnostics.Process(), this.specPathbox.Text + probox.Text, "explorer.exe", this.specPathbox.Text + probox.Text, false, false, 0);
            }
            else
            {
                DialogResult result = MessageBox.Show("The Project directory: " + probox.Text + " does not exist. Do you want to create it?", "Project not found!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    System.IO.Directory.CreateDirectory(this.specPathbox.Text + probox.Text);
                    prolbl_Click(null, e);
                }
            }
            RefreshProjects();
        }

        private void Backup_Click(object sender, EventArgs e)
        {
            Rsx.Dumb.Process(new System.Diagnostics.Process(), this.Linaa.FolderPath + DB.Properties.Resources.Backups, "explorer.exe", this.Linaa.FolderPath + DB.Properties.Resources.Backups, false, false, 0);
        }

        private void specPathlbl_Click(object sender, EventArgs e)
        {
            Rsx.Dumb.Process(new System.Diagnostics.Process(), this.specPathbox.Text, "explorer.exe", this.specPathbox.Text, false, false, 0);
        }

        private void ETApply_Click(object sender, EventArgs e)
        {
            object o = this.peakBS.Current;
            if (o == null) return;

            DataRowView rv = o as DataRowView;

            LINAA.PeaksRow p = rv.Row as LINAA.PeaksRow;

            if (!p.IsETANull()) stopafterbox.Text = p.ETA;
        }

        private void probox_DropDownClosed(object sender, EventArgs e)
        {
            SetTimer(false, false);
            refreshtimebox.Text = "2";
            SetTimer(true, false);
        }

        private void ucDetWatch_MouseEnter(object sender, EventArgs e)
        {
            this.Focus();
        }

        private void tstVDM_Click(object sender, EventArgs e)
        {
            string message = "Testing the VDM...";
            this.StatusLbl.Text = message;
            this.StatusLbl.ForeColor = Color.Red;
            SpectrumSaveBackup(detBox.Text + ".CNF");
        }
    }
}