using Rsx.Dumb;
using System.Collections.Generic;

namespace DB
{
    public partial class LINAA : IToDoes
    {
        protected IList<string> toDoesList;

        public IList<string> ToDoesList
        {
            get
            {
                if (toDoesList != null)
                {
                    toDoesList.Clear();
                    toDoesList = null;
                }
                toDoesList = Hash.HashFrom<string>(this.tableToDo.labelColumn);
                return toDoesList;
            }
        }

        public void PopulateToDoes()
        {
            // this.tAM.ToDoTableAdapter.DeleNulls();
            this.tableToDo.Clear();
            this.TAM.ToDoTableAdapter.Fill(this.tableToDo);
            this.tableToDo.AcceptChanges();
        }
    }
}