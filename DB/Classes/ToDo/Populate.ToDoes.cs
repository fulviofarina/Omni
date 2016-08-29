using System.Collections.Generic;
using DB.Interfaces;
using Rsx;

namespace DB
{
    public partial class LINAA : IToDoes
    {
        protected internal IList<string> toDoesList;
        public IList<string> ToDoesList
        {
            get
            {
                if (toDoesList != null)
                {
                    toDoesList.Clear();
                    toDoesList = null;
                }
                toDoesList = Dumb.HashFrom<string>(this.tableToDo.labelColumn);
                return toDoesList;
            }
        }

        public void PopulateToDoes()
        {
            //  this.tAM.ToDoTableAdapter.DeleNulls();
            this.tableToDo.Clear();
            this.TAM.ToDoTableAdapter.Fill(this.tableToDo);
            this.tableToDo.AcceptChanges();
        }
    }
}