using Rsx.Dumb;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DB
{
    public partial class LINAA : IOrders
    {
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

        public int? FindOrderID(string LabOrderRef)
        {
            int? OrderID = null;

            string Orders = this.tableOrders.LabOrderRefColumn.ColumnName;
            Func<OrdersRow, bool> finder = LINAA.SelectorByField<OrdersRow>(LabOrderRef, Orders);
            OrdersRow r = this.tableOrders.FirstOrDefault(finder);
            if (r != null) OrderID = r.OrdersID;

            return OrderID;
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