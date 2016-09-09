using System;

namespace DB
{
  public partial class LINAA
  {
    public delegate bool Comparer<T>(T b1, T b2);

    public static class Comparers
    {
      private static bool Alike(ref PeaksRow b1, ref PeaksRow b2)
      {
        if (Math.Abs(b1.ID) == Math.Abs(b2.ID)) return true;
        else return false;
      }

      private static bool Alike(ref IRequestsAveragesRow b1, ref IRequestsAveragesRow b2)
      {
        if (b1.NAAID == b2.NAAID) return true;
        else return false;
      }

      private static bool Alike(ref IPeakAveragesRow b1, ref IPeakAveragesRow b2)
      {
        if (b1.k0NAARow == b2.k0NAARow) return true;
        else return false;
      }

      private static bool NotLike(ref IRequestsAveragesRow b1, ref IRequestsAveragesRow b2)
      {
        if (b1.NAAID != b2.NAAID) return true;
        else return false;
      }

      private static bool NotLike(ref IPeakAveragesRow b1, ref IPeakAveragesRow b2)
      {
        if (b1.k0NAARow.NAARow != b2.k0NAARow.NAARow) return true;
        else return false;
      }

      private static bool NotLike(ref PeaksRow b1, ref PeaksRow b2)
      {
        if (Math.Abs(b1.ID) != Math.Abs(b2.ID)) return true;
        else return false;
      }

      private static bool Alike(ref MeasurementsRow b1, ref MeasurementsRow b2)
      {
        if (b1.Detector.Equals(b2.Detector))
        {
          if (b1.Position == b2.Position) return true;
          else return false;
        }
        else return false;
      }

      private static bool NotLike(ref MeasurementsRow b1, ref MeasurementsRow b2)
      {
        if (!b1.Detector.Equals(b2.Detector))
        {
          if (b1.Position != b2.Position) return true;
          else return false;
        }
        else return false;
      }

      public static bool Alike<T>(T b1, T b2)
      {
        if (b1 == null) throw new ArgumentException("Alike<T> Error: b1 is null");
        if (b2 == null) throw new ArgumentException("Alike<T> Error: b2 is null");

        Type tipo = typeof(T);
        if (tipo.Equals(typeof(PeaksRow)))
        {
          PeaksRow p1 = b1 as PeaksRow;
          PeaksRow p2 = b2 as PeaksRow;
          return Alike(ref p1, ref p2);
        }
        else if (tipo.Equals(typeof(MeasurementsRow)))
        {
          MeasurementsRow p1 = b1 as MeasurementsRow;
          MeasurementsRow p2 = b2 as MeasurementsRow;
          return Alike(ref p1, ref p2);
        }
        else if (tipo.Equals(typeof(IRequestsAveragesRow)))
        {
          IRequestsAveragesRow p1 = b1 as IRequestsAveragesRow;
          IRequestsAveragesRow p2 = b2 as IRequestsAveragesRow;
          return Alike(ref p1, ref p2);
        }
        else if (tipo.Equals(typeof(IPeakAveragesRow)))
        {
          IPeakAveragesRow p1 = b1 as IPeakAveragesRow;
          IPeakAveragesRow p2 = b2 as IPeakAveragesRow;
          return Alike(ref p1, ref p2);
        }
        else throw new ArgumentException("Alike<T> not implemented");
      }

      public static bool NotLike<T>(T b1, T b2)
      {
        if (b1 == null) throw new ArgumentException("NotLike<T> Error: b1 is null");
        if (b2 == null) throw new ArgumentException("NotLike<T> Error: b2 is null");

        Type tipo = typeof(T);
        if (tipo.Equals(typeof(PeaksRow)))
        {
          PeaksRow p1 = b1 as PeaksRow;
          PeaksRow p2 = b2 as PeaksRow;
          return NotLike(ref p1, ref p2);
        }
        else if (tipo.Equals(typeof(MeasurementsRow)))
        {
          MeasurementsRow p1 = b1 as MeasurementsRow;
          MeasurementsRow p2 = b2 as MeasurementsRow;
          return NotLike(ref p1, ref p2);
        }
        else if (tipo.Equals(typeof(IRequestsAveragesRow)))
        {
          IRequestsAveragesRow p1 = b1 as IRequestsAveragesRow;
          IRequestsAveragesRow p2 = b2 as IRequestsAveragesRow;
          return NotLike(ref p1, ref p2);
        }
        else if (tipo.Equals(typeof(IPeakAveragesRow)))
        {
          IPeakAveragesRow p1 = b1 as IPeakAveragesRow;
          IPeakAveragesRow p2 = b2 as IPeakAveragesRow;
          return NotLike(ref p1, ref p2);
        }
        else throw new ArgumentException("NotLike<T> not implemented");
      }
    }
  }
}