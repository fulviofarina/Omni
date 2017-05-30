//using DB.Interfaces;

namespace DB
{
    public partial class LINAA
    {
        /*
     public bool Read(string file)

     {
         bool read = false;

         try
         {
             this.Clear();

             if (System.IO.File.Exists(file))
             {
                 this.ReadXml(file, XmlReadMode.InferTypedSchema);
             }
             read = true;
         }
         catch (SystemException ex)
         {
             this.AddException(ex);
         }

         return read;
     }
     */

        /*
    public void PopulateSubSamples()
    {
    try
    {
       this.tableSubSamples.Clear();
       this.tableSubSamples.BeginLoadData();
       TAM.SubSamplesTableAdapter.DeleteNulls();
       LINAA.SubSamplesDataTable newsamples = new SubSamplesDataTable(false);
       TAM.SubSamplesTableAdapter.Fill(newsamples);
       this.tableSubSamples.Merge(newsamples, false, MissingSchemaAction.AddWithKey);

       foreach (LINAA.SubSamplesRow s in this.tableSubSamples)
       {
          if (s.IsCapsulesIDNull()) continue;
          if (s.CapsulesRow == null) continue;
          int id = s.CapsulesRow.VialTypeID;
          bool enaa = s.CapsulesRow.ENAA;
         // s.CapsulesID = id; s.ENAA = enaa;

          this.tAM.SubSamplesTableAdapter.UpdateCaps(enaa, id, s.SubSamplesID);
       }

       // LINAA.SetAdded(ref old);
       this.tableSubSamples.EndLoadData();
       this.tableSubSamples.AcceptChanges();
    }
    catch (SystemException ex)
    {
       this.AddException(ex);
    }
    }
    */
    }
}