using System;
using System.Collections.Generic;

//using DB.Interfaces;
using Rsx;

namespace DB
{
  public partial class LINAA : IProjects
  {
    protected internal IList<string> projectsList;

    /// <summary>
    /// Gets a non-repeated list of Irradiation Requests in the database
    /// </summary>
    public IList<string> ProjectsList
    {
      get
      {
        if (projectsList != null) projectsList.Clear();
        projectsList = Dumb.HashFrom<string>(this.tableIrradiationRequests.IrradiationCodeColumn);
        return projectsList;
      }
    }

    protected internal ICollection<string> activeProjectsList;

    /// <summary>
    /// Gets a non-repeated list of Lab-Order References in the database
    /// </summary>
    public ICollection<string> ActiveProjectsList
    {
      get
      {
        if (activeProjectsList != null) activeProjectsList.Clear();
        activeProjectsList = Dumb.HashFrom<string>(tableIRequestsAverages.ProjectColumn, DB.Properties.Misc.Cd, string.Empty);

        return activeProjectsList;
      }
    }

    //
    private Action[] PMLIMS()
    {
      Action[] populatorArray = null;

      populatorArray = new Action[]   {
           PopulateChannels,
           PopulateIrradiationRequests,
        PopulateOrders,
        PopulateProjects
         };

      return populatorArray;
    }

    public IList<LINAA.SubSamplesRow> FindByProject(string project)
    {
      return this.tableSubSamples.FindByProject(project);
    }

    public void PopulateProjects()
    {
      try
      {
        //Handlers(this.tableSubSamples, false);

        //	Dumb.CleanColumnExpressions(Measurements);
        this.tableProjects.Clear();

        this.TAM.ProjectsTableAdapter.Fill(this.tableProjects);
        //Handlers(this.tableSubSamples, true);
        this.tableProjects.AcceptChanges();
      }
      catch (SystemException ex)
      {
        this.AddException(ex);
      }
    }
  }
}