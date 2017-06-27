using System;
using System.Collections.Generic;
using System.Linq;

//using DB.Interfaces;
using Rsx.Dumb;

namespace DB
{
    public partial class LINAA : IOrders
    {
        public Int32? FindOrderID(String LabOrderRef)
        {
            Int32? OrderID = null;

            string Orders = this.tableOrders.LabOrderRefColumn.ColumnName;
            Func<OrdersRow, bool> finder = LINAA.SelectorByField<OrdersRow>(LabOrderRef, Orders);
            LINAA.OrdersRow r = this.tableOrders.FirstOrDefault(finder);
            if (r != null) OrderID = r.OrdersID;

            return OrderID;
        }

        protected ICollection<string> ordersList;

        /// <summary>
        /// Gets a non-repeated list of Lab-Order References in the database
        /// </summary>
        public ICollection<string> OrdersList
        {
            get
            {
                if (ordersList != null) ordersList.Clear();
                ordersList = Hash.HashFrom<string>(this.tableOrders.LabOrderRefColumn);

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