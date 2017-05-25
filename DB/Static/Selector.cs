using System;
using System.Data;
using Rsx;

namespace DB
{
    public partial class LINAA
    {
        public static Func<T, bool> SelectorAnalyteBy<T>(string sym, string iso, double energy)
        {
            Func<T, bool> any = null;
            if (typeof(T).Equals(typeof(PeaksRow)))
            {
                any = SelectorPeaksBy(sym, iso, energy) as Func<T, bool>;
            }
            else if (typeof(T).Equals(typeof(IPeakAveragesRow)))
            {
                any = SelectorGammasBy(sym, iso, energy) as Func<T, bool>;
            }
            else if (typeof(T).Equals(typeof(IRequestsAveragesRow)))
            {
                any = SelectorIsotopesBy(sym, iso, energy) as Func<T, bool>;
            }
            else if (typeof(T).Equals(typeof(k0NAARow)))
            {
                any = Selectork0NAAIsotopesBy(sym, iso, energy) as Func<T, bool>;
            }
            else if (typeof(T).Equals(typeof(NAARow)))
            {
                any = SelectorNAAIsotopesBy(sym, iso, energy) as Func<T, bool>;
            }
            else throw new SystemException("Not impletemented");
            return any;
        }

        public static Func<T, bool> SelectorByField<T>(object value, string field)
        {
            Func<T, bool> any = null;
            any = x =>
            {
                DataRow r = x as DataRow;
                if (!EC.IsNuDelDetch(r))
                {
                    if (!r.IsNull(field))
                    {
                        if (r.Field<object>(field).Equals(value)) return true;
                    }
                }
                return false;
            };
            return any;
        }

        public static Func<GeometryRow, bool> SelectorGeometryBy(string geometryName)
        {
            Func<GeometryRow, bool> refFinder = o =>
            {
                if (o.GeometryName.ToUpper().CompareTo(geometryName.ToUpper()) == 0) return true;
                return false;
            };
            return refFinder;
        }

        private static Func<IPeakAveragesRow, bool> SelectorGammasBy(string sym, string iso, double energy)
        {
            Func<IPeakAveragesRow, bool> peakfinder2 = x =>
            {
                if (x.Element.ToUpper().CompareTo(sym.ToUpper()) == 0)
                {
                    if (iso.Equals(string.Empty)) return true;
                    if (x.Radioisotope.ToUpper().CompareTo(iso.ToUpper()) == 0)
                    {
                        if (energy == 0) return true;
                        if (x.Energy == energy) return true;
                    }
                }
                return false;
            };
            return peakfinder2;
        }

        private static Func<IRequestsAveragesRow, bool> SelectorIsotopesBy(string sym, string iso, double energy)
        {
            Func<IRequestsAveragesRow, bool> peakfinder2 = x =>
            {
                if (x.Element.ToUpper().CompareTo(sym.ToUpper()) == 0)
                {
                    if (iso.Equals(string.Empty)) return true;
                    if (x.Radioisotope.ToUpper().CompareTo(iso.ToUpper()) == 0) return true;
                }
                return false;
            };
            return peakfinder2;
        }

        private static Func<k0NAARow, bool> Selectork0NAAIsotopesBy(string sym, string iso, double energy)
        {
            Func<k0NAARow, bool> peakfinder2 = x =>
            {
                if (x.Sym.ToUpper().CompareTo(sym.ToUpper()) == 0)
                {
                    if (iso.Equals(string.Empty)) return true;
                    if (x.Iso.ToUpper().CompareTo(iso.ToUpper()) == 0)
                    {
                        if (energy == 0) return true;
                        if (x.Energy == energy) return true;
                    }
                }
                return false;
            };
            return peakfinder2;
        }

        private static Func<NAARow, bool> SelectorNAAIsotopesBy(string sym, string iso, double energy)
        {
            Func<NAARow, bool> peakfinder2 = x =>
            {
                if (x.Sym.ToUpper().CompareTo(sym.ToUpper()) == 0)
                {
                    if (iso.Equals(string.Empty)) return true;
                    if (x.Iso.ToUpper().CompareTo(iso.ToUpper()) == 0) return true;
                }
                return false;
            };
            return peakfinder2;
        }

        private static Func<PeaksRow, bool> SelectorPeaksBy(string sym, string iso, double energy)
        {
            Func<PeaksRow, bool> peakfinder2 = x =>
            {
                if (x.Sym.ToUpper().CompareTo(sym.ToUpper()) == 0)
                {
                    if (iso.Equals(string.Empty)) return true;
                    if (x.Iso.ToUpper().CompareTo(iso.ToUpper()) == 0)
                    {
                        if (energy == 0) return true;
                        if (x.Energy == energy) return true;
                    }
                }
                return false;
            };
            return peakfinder2;
        }
    }
}