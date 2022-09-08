using Infosys.QuickKart.DataAccessLayer;
using Infosys.QuickKart.DataAccessLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infosys.QuickKart.ServiceLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]    
    public class ProductsController : ControllerBase
    {
        private readonly QuickKartRepository repository;

        public ProductsController()
        {
            repository = new QuickKartRepository();
        }

        //Infosys
        //public JsonResult GetAllProducts()
        //{
        //    List<Products> products;
        //    try
        //    {
        //        products = repository.GetAllProducts();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return Json(products);
        //}

        //Microsoft recommended code
        //Retrieve All the Products
        [HttpGet]
        [Route("GetProducts")]
        //api/Products/getproducts
        public IActionResult GetAllProducts()
        {
            List<Products> products;
            try
            {
                products = repository.GetAllProducts();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Ok(products); //Retunrs HTTP Status Code : 200
        }

        [HttpGet]
        [Route("GetProductById/{productId}")]
        /* /api/Products/GetProductById/p101 */
        public IActionResult GetProductByProductId(string productId)
        {
            Products product;
            try
            {
                product = repository.GetProductById(productId);
                if (product != null)
                {
                    return Ok(product);
                }                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return NotFound("Product Id doesn't exists");
        }
       
    }
}
