using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
//using DB.Interfaces;
using Rsx;

namespace DB
{
    public partial class LINAA : IStore
    {
        public void Delete<T>(ref IEnumerable<T> rows2)
        {
            if (rows2 == null || rows2.Count() == 0) return;
            //my first reverse loop... seems to work perfectly
            for (int i = rows2.Count() - 1; i >= 0; i--)
            {
                DataRow op = rows2.ElementAt(i) as DataRow;
                try
                {
                    if (!EC.IsNuDelDetch(op))
                    {
                        op.Delete();
                    }
                }
                catch (SystemException ex)
                {
                    this.AddException(ex);
                }
            }
        }

        public bool DeletePeaks(Int32 measID)
        {
            LINAATableAdapters.PeaksTableAdapter peaksTa = new LINAATableAdapters.PeaksTableAdapter();
            bool success = false;
            try
            {
                peaksTa.DeleteByMeasurementID(measID);
                success = true;
            }
            catch (SystemException ex)
            {
                this.AddException(ex);
            }
            peaksTa.Dispose();
            peaksTa = null;
            return success;
        }
    }
}