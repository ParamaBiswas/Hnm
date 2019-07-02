using PMSModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMSInterface
{
    public interface ISupplierInvoiceRepository
    {
        string SaveSupplierInvoice(SupplierInvoice objSupplierInvoice);
        List<SupplierInvoice> GetSupplierInvoices(string pDateFrom, string pDateTo, string suppliercode);
    }
}
