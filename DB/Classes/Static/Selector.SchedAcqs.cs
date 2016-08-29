using System;

namespace DB
{
    public partial class LINAA
    {
        public static Func<SchAcqsRow, bool> SelectorSchAcqsBy(bool done, bool interrupted)
        {
            Func<SchAcqsRow, bool> s = x =>
            {
                if (x.Done == done && x.Interrupted == interrupted) return true;
                return false;
            };
            return s;
        }

        public static Func<SchAcqsRow, bool> SelectorSchAcqsBy(string project, string sample)
        {
            Func<SchAcqsRow, string, bool> projectFilter = (x, proj) =>
            {
                if (proj.Equals(string.Empty)) return true;
                if (x.Project.CompareTo(project) == 0) return true;
                return false;
            };

            Func<SchAcqsRow, bool> s = x =>
            {
                if (projectFilter(x, project))
                {
                    if (sample.Equals(string.Empty)) return true;
                    if (x.Sample.CompareTo(sample) == 0) return true;
                }
                return false;
            };

            return s;
        }

        public static Func<DB.LINAA.SchAcqsRow, bool> SelectorSchAcqsBy(string detector)
        {
            Func<SchAcqsRow, bool> s = x =>
            {
                if (x.Detector.CompareTo(detector) == 0) return true;
                return false;
            };
            return s;
        }

        public static Func<SchAcqsRow, bool> SelectorSchAcqsBy(DateTime startOn, bool lessthan, bool OrEqual)
        {
            if (OrEqual)
            {
                if (lessthan)
                {
                    Func<SchAcqsRow, bool> s = x =>
                    {
                        if (x.StartOn <= startOn) return true;
                        return false;
                    };
                    return s;
                }
                else
                {
                    Func<SchAcqsRow, bool> s = x =>
                    {
                        if (x.StartOn >= startOn) return true;
                        return false;
                    };
                    return s;
                }
            }
            else
            {
                if (lessthan)
                {
                    Func<SchAcqsRow, bool> s = x =>
                    {
                        if (x.StartOn < startOn) return true;
                        return false;
                    };
                    return s;
                }
                else
                {
                    Func<SchAcqsRow, bool> s = x =>
                    {
                        if (x.StartOn > startOn) return true;
                        return false;
                    };
                    return s;
                }
            }
        }
    }
}