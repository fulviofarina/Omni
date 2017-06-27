using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DB;
using Rsx.Dumb;

namespace k0X
{
  public partial class ucOrder : UserControl
  {
    public System.Collections.Hashtable ucSamplesHashTable;

    protected internal LINAA.OrdersRow orderRow;

    public LINAA.OrdersRow OrderRow
    {
      get { return orderRow; }
      set { orderRow = value; }
    }

    private void Box_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.KeyCode == Keys.Enter)
      {
        if (!EC.IsNuDelDetch(orderRow))
        {
          orderRow.LabOrderRef = Box.Text;
          this.ParentForm.Text = "Order - " + this.Box.Text;
          this.Linaa.TAM.OrdersTableAdapter.Update(this.Linaa.Orders);
          foreach (System.Collections.DictionaryEntry ucSampleHash in ucSamplesHashTable)
          {
            ucSamples ucsample = (ucSamples)ucSampleHash.Value;
            ucsample.Orderbox.Text = orderRow.LabOrderRef;
          }
          Application.OpenForms.OfType<MainForm>().First().Link();
        }
      }
    }

    public void AddOrder(String LabOrderRef)
    {
      try
      {
        orderRow = this.Linaa.Orders.FirstOrDefault(o => o.LabOrderRef.CompareTo(LabOrderRef) == 0);

        if (EC.IsNuDelDetch(orderRow))
        {
          orderRow = this.Linaa.Orders.NewOrdersRow();
          orderRow.LabOrderRef = LabOrderRef;
          orderRow.RegistrationDate = DateTime.Now;
          this.Linaa.Orders.AddOrdersRow(orderRow);
          this.Linaa.Save<LINAA.OrdersDataTable>();
          Application.OpenForms.OfType<MainForm>().First().Link();
        }

        this.Box.Text = LabOrderRef;
      }
      catch (SystemException ex)
      {
        this.Linaa.AddException(ex);
      }
    }

    public void AddProject(ref ucSamples ucsamp)
    {
      try
      {
        int? IrReqId = this.Linaa.FindIrradiationID(ucsamp.Name);

        LINAA.ProjectsRow project = this.Linaa.FindBy(IrReqId, orderRow.OrdersID, true);

        this.Linaa.Save<LINAA.ProjectsDataTable>();

        ucsamp.ProjectsRow = project;
        ucsamp.Orderbox.Visible = true;
        ucsamp.ucOrder = this;
        ucsamp.Orderbox.Text = orderRow.LabOrderRef;
        if (!ucSamplesHashTable.ContainsKey(ucsamp.Name))
        {
          ucSamplesHashTable.Add(ucsamp.Name, ucsamp);
        }
      }
      catch (SystemException ex)
      {
        this.Linaa.AddException(ex);
      }
    }

    public void Save()
    {
      try
      {
        this.Linaa.Save<LINAA.ProjectsDataTable>();
        this.Linaa.Save<LINAA.OrdersDataTable>();
      }
      catch (SystemException ex)
      {
        this.Linaa.AddException(ex);
      }
    }

    public bool Populate()
    {
      bool success = false;
      if (orderRow != null)
      {
        try
        {
          IEnumerable<LINAA.ProjectsRow> projectsRow = orderRow.GetProjectsRows();
          IList<string> ls = Hash.HashFrom<string>(projectsRow, this.Linaa.Projects.ProjectColumn.ColumnName);
          MainForm MBox = Application.OpenForms.OfType<MainForm>().First();
          foreach (string project in ls)
          {
            MBox.Box.Text = project;
            MBox.Box_KeyUp(this, new KeyEventArgs(Keys.Enter));
          }
          ls.Clear();
          ls = null;
          if (ucSamplesHashTable.Count != 0) success = true;
        }
        catch (SystemException ex)
        {
          this.Linaa.AddException(ex);
        }
      }

      return success;
    }

    protected internal LINAA Linaa;

    public ucOrder(ref LINAA set, String LabOrderRef)
    {
      InitializeComponent();
      this.Linaa = set;
      ucSamplesHashTable = new System.Collections.Hashtable();

      this.AddOrder(LabOrderRef);
      AuxiliarForm form2 = new AuxiliarForm();
      form2.Text = "Order - " + LabOrderRef;
      UserControl control = this;
      form2.Populate(ref control);

      form2.Show();
    }
  }
}