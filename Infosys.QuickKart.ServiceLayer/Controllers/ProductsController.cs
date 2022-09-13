using Infosys.QuickKart.DataAccessLayer;
using Infosys.QuickKart.DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Infosys.QuickKart.ServiceLayer.Controllers
{
    // [Authorize(Roles = "Admin")]
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
        //[Route("GetProducts")]
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
        //    //return Json("Product Id doesn't exists");
        //}

        //Microsoft recommended code
        //Retrieve All the Products
       // [Authorize]
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

        [HttpGet("GetProductsByCategoryId/{categoryId}")]
        [ProducesResponseType(typeof(Products), StatusCodes.Status200OK)]
        public IEnumerable<Products> GetProductsByCategoryId(string categoryId)
        {
            return repository.GetAllProductsUsingFromSQLRaw(categoryId);
        }


        [HttpGet]
        [Route("GetProductById/{productId}", Name = "GetProductById")]
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

        #region Add Product Using JSON Result

        //[HttpPost]
        //[Route("AddProduct")]
        //public JsonResult AddProduct(Products product)
        //{
        //    var result = repository.AddNewProduct(product);
        //    if (result)
        //    {
        //        return Json("Successfully Created Product");
        //    }
        //    else
        //    {
        //        return Json("Please try later");
        //    }
        //}

        #endregion

        //Add New Product using Model
       // [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("AddProduct")]
        public IActionResult AddNewProduct([FromBody] Products product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(product);
            }
            bool status;
            try
            {
                status = repository.AddNewProduct(product);
                if (status)
                {
                    //return Ok("Product Created Successfully");
                    // return CreatedAtRoute("GetProductById", new { productId = product.ProductId }, product);
                    return Created("AddNewProduct", product);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //return new StatusCodeResult(500);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        //Add New Product using Parameter
        [HttpPost]
        [Route("AddProductUsingParameter")]
        public IActionResult AddNewProductUsingParameter(string productName, decimal price, int quantity, byte categoryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            int status;
            try
            {
                status = repository.AddNewProductUsingUSP(productName, price, quantity, categoryId);
                if (status > 0)
                {
                    return Created("AddProductUsingParameter", productName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }



        [HttpPost("AddCategory")]
        [ProducesResponseType(typeof(Categories), StatusCodes.Status201Created)]
        public IActionResult AddNewCategory(string categoryName)
        {
            bool status;
            try
            {
                status = repository.AddCategory(categoryName);
                if (status)
                {
                    return Created("AddCategory", categoryName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return BadRequest();
        }


        //Update the Product
        [HttpPut]
        [Route("UpdateProduct")]
        public IActionResult UpdateProduct(string productId, decimal price, int quantity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            bool status;
            try
            {
                status = repository.UpdateProduct(productId, price, quantity);
                if (status)
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return NotFound("Product Id does not exists");
        }

        [HttpPut]
        [Route("UpdateProductModel")]
        public IActionResult UpdateProductUsingModel(Products product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(product);
            }
            bool status;
            try
            {
                status = repository.UpdateProductUsingModel(product);
                if (status)
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return NotFound("Product Id does not exists");
        }

        [HttpPut("UpdateProductPrice")]
        public IActionResult UpdateProductPrice(Products product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(product);
            }
            bool status;
            try
            {
                status = repository.UpdateProductUsingModel(product);
                if (status)
                {
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return NotFound("Product Id does not exists");
        }

        [HttpDelete]
        [Route("DeleteProduct/{productId}")]
        //api/Products/DeleteProduct/P158
        public IActionResult DeleteProduct(string productId)
        {
            bool status;
            try
            {
                status = repository.DeleteProduct(productId);
                if (status)
                {
                    //return NoContent();
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return NotFound("Product Id doesn't Exists");
        }
    }
}
