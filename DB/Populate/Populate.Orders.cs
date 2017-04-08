using System;
using System.Collections.Generic;

//using DB.Interfaces;
using Rsx;

namespace DB
{
    public partial class LINAA : IOrders
    {
        public Int32? FindOrderID(string LabOrdRef)
        {
            return this.tableOrders.FindOrderID(LabOrdRef);
        }

        protected internal ICollection<string> ordersList;

        /// <summary>
        /// Gets a non-repeated list of Lab-Order References in the database
        /// </summary>
        public ICollection<string> OrdersList
        {
            get
            {
                if (ordersList != null) ordersList.Clear();
                ordersList = Dumb.HashFrom<string>(this.tableOrders.LabOrderRefColumn);

                return ordersList;
            }
        }

        public void PopulateOrders()
        {
            try
            {
                this.tableOrders.Clear();
                this.TAM.OrdersTableAdapter.Fill(this.tableOrders);

                this.tableOrders.AcceptChanges();
            }
            catch (SystemException ex)
            {
                this.AddException(ex);
            }
        }
    }
}