using System;
using System.Collections.Generic;
using System.Linq;
using Rsx.Dumb;

namespace DB
{
    public partial class LINAA
    {
        public ToDoType GetToDoType(String ToDoTypeText)
        {
            ToDoTypeText = ToDoTypeText.ToUpper();
            ToDoType tocalculate = 0;

            if (ToDoTypeText.CompareTo("F-ALPHA BARE") == 0) tocalculate = ToDoType.fAlphaBare;
            else if (ToDoTypeText.CompareTo("F-ALPHA CD-RATIO") == 0) tocalculate = ToDoType.fAlphaCdRatio;
            else if (ToDoTypeText.CompareTo("F-ALPHA CD-COVERED") == 0) tocalculate = ToDoType.fAlphaCdCovered;
            else if (ToDoTypeText.CompareTo("GENERAL RATIO") == 0) tocalculate = ToDoType.GeneralRatio;
            else if (ToDoTypeText.CompareTo("U-RATIO") == 0) tocalculate = ToDoType.URatio;
            else if (ToDoTypeText.CompareTo("CD-RATIO") == 0) tocalculate = ToDoType.CdRatio;
            else if (ToDoTypeText.CompareTo("Q0-DETERMINATION") == 0) tocalculate = ToDoType.Q0determination;
            else if (ToDoTypeText.CompareTo("K0-DETERMINATION") == 0) tocalculate = ToDoType.k0determination;
            return tocalculate;
        }

        public enum ToDoType
        {
            fAlphaBare = 1,
            fAlphaCdRatio = 2,
            fAlphaCdCovered = 3,
            GeneralRatio = 4,
            URatio = 5,
            CdRatio = 6,
            Q0determination = 7,
            k0determination = 8,
        }

        public partial class ToDoRow
        {
            public void Checkk0(ref IEnumerable<LINAA.IPeakAveragesRow> refeS)
            {
                LINAA.IRequestsAveragesRow IRef = null;

                //if Referen 1 fails...
                if (!EC.IsNuDelDetch(this.IRAvgRow))
                {
                    //take Reference2!!!
                    IRef = this.IRAvgRow;
                    refeS = IRef.GetIPeakAveragesRows();
                }
                if (!EC.IsNuDelDetch(this.IRAvgRow2))
                {
                    IRef = this.IRAvgRow2;
                    refeS = refeS.Union(IRef.GetIPeakAveragesRows()).ToList();
                }

                if (refeS.Count() == 0) //no peaks, error
                {
                    this.RowError = "Peaks data was not found for this Reference isotope. Please load the irradiation project first";
                    return;
                }

                return;
            }

            public void CheckBare(ref IEnumerable<LINAA.IPeakAveragesRow> refeS)
            {
                LINAA.IRequestsAveragesRow IRef = null;

                //if Referen 1 fails...
                if (EC.IsNuDelDetch(this.IRAvgRow) || EC.IsNuDelDetch(this.IRAvgRow.SubSamplesRow) || this.IRAvgRow.SubSamplesRow.ENAA)
                {
                    //take Reference2!!!
                    if (EC.IsNuDelDetch(this.IRAvgRow2))
                    {
                        this.RowError = "Isotope 1 failed and alternative Isotope 2 has no data loaded. Please load their data first!";
                        return;
                    }
                    IRef = this.IRAvgRow2;
                    if (EC.IsNuDelDetch(IRef.SubSamplesRow))
                    {
                        this.RowError = "Isotope 1 failed and alternative Isotope 2 lacks sample data. Please load their data first!";
                        return;
                    }
                    else if (IRef.SubSamplesRow.ENAA)
                    {
                        this.RowError = "Isotope 1 failed and alternative Isotope 2 was Cd-irradiated.\nNot possible for the f-Alpha Bare method. Please select a non-Cd Reference isotope or load their data!";
                        return;
                    }
                }
                else IRef = this.IRAvgRow;

                refeS = IRef.GetIPeakAveragesRows();

                if (refeS.Count() == 0) //no peaks, error
                {
                    this.RowError = "Peaks data was not found for this Reference isotope. Please load the irradiation project first";
                    return;
                }

                return;
            }

            public void CheckCdCovered(ref IEnumerable<LINAA.IPeakAveragesRow> refeS, ref IEnumerable<LINAA.IPeakAveragesRow> monS)
            {
                LINAA.IRequestsAveragesRow IRef = null;
                //if Referen 1 fails...
                if (EC.IsNuDelDetch(this.IRAvgRow) || EC.IsNuDelDetch(this.IRAvgRow.SubSamplesRow) || !this.IRAvgRow.SubSamplesRow.ENAA)
                {
                    //take Reference2!!!
                    if (EC.IsNuDelDetch(this.IRAvgRow2))
                    {
                        this.RowError = "Isotope 1 failed and alternative Isotope 2 has no data loaded. Please load their data first!";
                        return;
                    }
                    IRef = this.IRAvgRow2;
                    if (EC.IsNuDelDetch(IRef.SubSamplesRow))
                    {
                        this.RowError = "Isotope 1 failed and alternative Isotope 2 lacks sample data. Please load their data first!";
                        return;
                    }
                    else if (!IRef.SubSamplesRow.ENAA)
                    {
                        this.RowError = "Isotope 1 failed and alternative Isotope 2 was non-Cd-irradiated.\nNot possible for the f-Alpha Bare method. Please select a non-Cd Reference isotope or load their data!";
                        return;
                    }
                }
                else IRef = this.IRAvgRow;

                monS = IRef.GetIPeakAveragesRows();
                refeS = monS; //cd-covered does not need 2 projects!!!

                if (monS.Count() == 0) this.RowError = "No data found for this Isotope. Please load the irradiation project first";

                return;
            }

            public void CheckCdRatioOrQ0(ToDoType todoType, ref IEnumerable<LINAA.IPeakAveragesRow> refeS, ref IEnumerable<LINAA.IPeakAveragesRow> monS)
            {
                IRequestsAveragesRow Imon = null;

                if (EC.IsNuDelDetch(this.IRAvgRow) || EC.IsNuDelDetch(this.IRAvgRow.SubSamplesRow)) //no peaks data, Isotope 2 is fucked
                {
                    this.RowError = "Data not found for Isotope 1. Please load their irradiation project first";
                    return;
                }
                else Imon = this.IRAvgRow;
                IRequestsAveragesRow Iref = null;
                if (EC.IsNuDelDetch(this.IRAvgRow2) || EC.IsNuDelDetch(this.IRAvgRow2.SubSamplesRow)) //no peaks data, Isotope 2 is fucked
                {
                    this.RowError = "Data not found for Isotope 2. Please load their irradiation project first";
                    return;
                }
                else Iref = this.IRAvgRow2;

                //in case the Cd-isotopes (tat must go in the denominator) are inverted in the TodoList
                if (!Imon.SubSamplesRow.ENAA && Iref.SubSamplesRow.ENAA)
                {
                    monS = Imon.GetIPeakAveragesRows();
                    refeS = Iref.GetIPeakAveragesRows();
                }
                else if (Imon.SubSamplesRow.ENAA && !Iref.SubSamplesRow.ENAA)
                {
                    monS = Iref.GetIPeakAveragesRows();
                    refeS = Imon.GetIPeakAveragesRows();
                }
                else if (todoType == ToDoType.fAlphaCdRatio)
                {
                    this.RowError = "Could not find the Bare and its Cd counterpart. Please make sure you chose a Bare and Cd counterpart";
                    return;
                }
                else    //general Ratio!!!
                {
                    monS = Imon.GetIPeakAveragesRows();
                    refeS = Iref.GetIPeakAveragesRows();
                }

                if (monS.Count() == 0) this.RowError = "No data found for Isotope 1. Please load the irradiation project first";
                if (refeS.Count() == 0) this.RowError = "No data found for Isotope 2. Please load the irradiation project first";

                return;
            }
        }
    }
}