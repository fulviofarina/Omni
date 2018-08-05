using Rsx.Dumb;
using System;
using System.Data;
using System.Linq;

namespace DB
{
    public partial class LINAA
    {
        public partial class SchAcqsRow
        {
            public TimeSpan GetStopIn()
            {
                CheckedOn = DateTime.Now;
                NotCrashed = true;

                //original counter
                short orgCounter = Counter;
                //total seconds
                double totalSeconds = orgCounter * PresetTime;
                DateTime theoMeasStart = StartOn.AddSeconds(totalSeconds);
                NextStartOn = theoMeasStart.AddSeconds(PresetTime); //counter =0 means start on

                double diff = (CheckedOn - NextStartOn).TotalSeconds;  //positive if checked after next start on!!!
                double maxdiff = 3 * RefreshRate;

                if (PresetTime <= 1800 && PresetTime > 300) maxdiff = 10 * RefreshRate;
                else if (PresetTime <= 300 && PresetTime > 120) maxdiff = 15 * RefreshRate;
                else if (PresetTime <= 120) maxdiff = 20 * RefreshRate;

                while (diff > 0 && diff >= maxdiff)  //but instead of crashing just 1 stupid second afterwards, its necessary to crash after max extra time!!!
                {
                    if (NotCrashed) NotCrashed = false;
                    if (orgCounter < (Repeats - 1))
                    {
                        orgCounter++;
                        totalSeconds = orgCounter * PresetTime;
                        theoMeasStart = StartOn.AddSeconds(totalSeconds);
                        NextStartOn = theoMeasStart.AddSeconds(PresetTime); //counter =0 means start on
                        diff = (CheckedOn - NextStartOn).TotalSeconds;
                    }
                    else break;
                }

                Delay = Convert.ToInt32((theoMeasStart - MeasStart).TotalSeconds); //real delay

                double timeCounted = CT;
                if (Cummulative)
                {
                    double counted = (Counter * PresetTime);

                    if (!NotCrashed)
                    {
                        NotCrashed = true;
                        timeCounted -= (counted);
                    }
                    else
                    {
                        double excess = (orgCounter - Counter) * PresetTime;
                        timeCounted -= (counted + excess);
                    }
                }

                //what's really possible to measure? Available Time to measure...
                Progress = Convert.ToInt16(100 * (timeCounted / PresetTime));
                //substract the time already counted...
                int aux = Convert.ToInt32(PresetTime - timeCounted);
                //finally what's available to measure...
                return new TimeSpan(0, 0, aux);
            }

            public string GetReportString()
            {
                string content = string.Empty;

                SchAcqsDataTable dt = this.tableSchAcqs;
                DataColumn[] arrsToConvert = new DataColumn[] { dt.PresetTimeColumn, dt.CTColumn, dt.RefreshRateColumn };

                foreach (System.Data.DataColumn c in this.tableSchAcqs.Columns)
                {
                    try
                    {
                        object val = this[c];
                        if (arrsToConvert.Contains(c))
                        {
                            val = Dumb.ToReadableString(new TimeSpan(0, 0, 0, Convert.ToInt32(val)));
                        }

                        content += c.ColumnName + ":\t\t";
                        if (val.GetType().Equals(typeof(DateTime))) content += ((DateTime)val).ToString("g", System.Globalization.CultureInfo.InstalledUICulture) + "\n";
                        else content += val + "\n";
                    }
                    catch (SystemException ex)
                    {
                        this.SetColumnError(c, ex.Message);
                    }
                }
                dt = null;
                arrsToConvert = null;
                return content;
            }

            public void SetSchedule(string project, string sample, Int16 pos, string det, Int16 repeats, double preset, DateTime startOn, string useremail, bool cummu)
            {
                DB.LINAA.SchAcqsRow sch = this;

                sch.Project = project;
                sch.Sample = sample;
                sch.Position = pos;
                sch.Detector = det;
                if (repeats == 0) repeats = 1;
                sch.Repeats = repeats;
                sch.StartOn = startOn;
                sch.PresetTime = preset; //in seconds...
                sch.User = useremail;
                sch.Cummulative = cummu;
            }

            public void Reset()
            {
                DB.LINAA.SchAcqsRow sch = this;

                sch.Done = false;
                sch.Counter = 0;
                sch.Delay = 0;
                sch.Saved = 0;
                sch.CT = 0;
                sch.Informed = false;
                sch.Interrupted = false;
                sch.NotCrashed = false;
                sch.LastMeas = string.Empty;
                sch.IsAwake = false;
                if (!sch.IsPresetTimeNull())
                {
                    if (!sch.IsStartOnNull()) sch.NextStartOn = sch.StartOn.AddSeconds(sch.PresetTime);
                    sch.RefreshRate = Convert.ToInt16(0.01 * sch.PresetTime);
                }
                sch.CheckedOn = DateTime.Now;
                sch.Progress = 0;
            }
        }
    }
}