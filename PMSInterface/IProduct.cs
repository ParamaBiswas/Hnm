using PMSModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMSInterface
{
    public interface IProduct
    {
        string SaveProduct(Product objproduct);
        List<Product> GetProductsList();
        Product GetProductByCode(string productcode);
        List<Product> GetProductsListByCategory(int productcategory);
    }
}
