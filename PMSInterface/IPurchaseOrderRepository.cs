using PMSModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMSInterface
{
    public interface IPurchaseOrderRepository
    {
        string SavePurchaseOrder(PurchaseOrder objPurchaseOrder);
        List<PurchaseOrder> GetPurchaseOrders(string pDateFrom, string pDateTo);
        PurchaseOrder GetPurchaseOrderByCode(string PurchaseOrderCode);
    }
}
