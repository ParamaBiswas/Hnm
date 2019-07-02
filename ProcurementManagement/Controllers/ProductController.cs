using System;
using System.Collections.Generic;
using CommonModel;
using ConnectionGateway;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMSInterface;
using PMSModel;

namespace ProcurementManagement.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        IProduct _product;
        ISupplierDbContext _supplierDbContext;
        public ProductController(ISupplierDbContext supplierDbContext, IProduct  product)
        {
            _supplierDbContext = supplierDbContext;
            _product = product;
        }

        [HttpPost]
        [Route("SaveProduct")]
        public IActionResult SaveProduct(Product objProduct)
        {
            if(!String.IsNullOrEmpty(objProduct.ProductCode_PK))
            {
                objProduct.IsNew = false;
            }
            else
            {
                objProduct.IsNew = true;
            }
            objProduct.ActionDate = CommonValidation.FormatDate(DateTime.Today.ToString("dd-MM-yyyy"), "yyyy-mm-dd", "dd-mm-yyyy");
            string Vmsg = _product.SaveProduct(objProduct);
            return Ok(new
            {
                message = Vmsg
            });
        }
        [HttpGet]
        [Route("GetProductsList")]
        public IActionResult GetProductsList()
        {
            List<Product> products= new List<Product>();
            products = _product.GetProductsList();
            return Ok(new
            {
               vList= products
            });
        }
        [HttpGet]
        [Route("GetProductByCode")]
        public IActionResult GetProductByCode(string productcode)
        {
            Product products = new Product();
            products = _product.GetProductByCode(productcode);
            return Ok(new
            {
                products
            });
        }
        [HttpGet]
        [Route("GetProductsListByCategory")]
        public IActionResult GetProductsListByCategory(int productcategory)
        {
            List<Product> products = new List<Product>();
            products = _product.GetProductsListByCategory(productcategory);
            return Ok(new
            {
                products
            });
        }

    }
}