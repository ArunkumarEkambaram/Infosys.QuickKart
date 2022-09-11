using Infosys.QuickKart.DataAccessLayer.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infosys.QuickKart.DataAccessLayer
{
    public class QuickKartRepository : IDisposable
    {
        private readonly QuickKartDBContext _dbContext = null;

        public QuickKartRepository()
        {
            _dbContext = new QuickKartDBContext();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(_dbContext);
            if (_dbContext != null)
            {
                _dbContext.Dispose();
            }
        }

        //Get All the Products
        public List<Products> GetAllProducts()
        {
            return _dbContext.Products.ToList();
        }

        //GetProductBy Id
        public Products GetProductById(string productId)
        {
            return _dbContext.Products.Find(productId);
        }

        //Invoking Scalar Function
        private string GetNewProductId()
        {
            return _dbContext.Products.Select(p => QuickKartDBContext.ufn_GenerateNewProductId()).FirstOrDefault();
        }

        //Add New Product - Model
        public bool AddNewProduct(Products product)
        {
            product.ProductId = GetNewProductId();
            _dbContext.Products.Add(product);
            int res = _dbContext.SaveChanges(); //Returns no of rows affected
            if (res > 0)
            {
                return true;
            }
            return false;
        }

        //Add New Product using USP
        public int AddNewProductUsingUSP(string productName, decimal price, int quantity, byte categoryId)
        {
            SqlParameter PrmProductId = new SqlParameter("@ProductId", GetNewProductId());
            SqlParameter PrmProductName = new SqlParameter("@ProductName", productName);
            SqlParameter PrmPrice = new SqlParameter("@Price", price);
            SqlParameter PrmQuantity = new SqlParameter("@QuantityAvailable", quantity);
            SqlParameter PrmCategoryId = new SqlParameter("@CategoryId", categoryId);
            SqlParameter PrmReturnValue = new SqlParameter("@ReturnValue", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            _dbContext.Database.ExecuteSqlRaw("EXEC @ReturnValue = usp_AddProduct @ProductId, @ProductName, @CategoryId, @Price, @QuantityAvailable"
            , PrmProductId, PrmProductName, PrmCategoryId, PrmPrice, PrmQuantity, PrmReturnValue);

            int result = Convert.ToInt32(PrmReturnValue.Value);

            return result;
        }

        //Add new Categories
        public bool AddCategory(string categoryName)
        {
            _dbContext.Categories.Add(new Categories { CategoryName = categoryName });
            var result = _dbContext.SaveChanges();
            if (result > 0)
            {
                return true;
            }
            return false;
        }


        //Update Product
        public bool UpdateProduct(string productId, decimal price, int quantity)
        {
            var productInDb = _dbContext.Products.Find(productId);
            if (productInDb != null)
            {
                productInDb.Price = price;
                productInDb.QuantityAvailable = quantity;
                _dbContext.Products.Update(productInDb);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }

        //Update Product using Model
        public bool UpdateProductUsingModel(Products product)
        {
            var productInDb = _dbContext.Products.Find(product.ProductId);
            if (productInDb != null)
            {
                productInDb.Price = product.Price;
                //productInDb.QuantityAvailable = product.QuantityAvailable;
                _dbContext.Products.Update(productInDb);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }

        public bool UpdateProductPrice(Products product)
        {
            var productInDb = _dbContext.Products.Find(product.ProductId);
            if (productInDb != null)
            {
                productInDb.Price = product.Price;
                _dbContext.Products.Update(productInDb);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }

        public bool DeleteProduct(string productId)
        {
            var productInDb = _dbContext.Products.Find(productId);
            if (productInDb != null)
            {
                _dbContext.Products.Remove(productInDb);
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }

        //Implementing UDF
        public IEnumerable<Products> GetAllProductsUsingFromSQLRaw(string categoryId)
        {          
            var products = _dbContext.Products.FromSqlRaw("Select * from Products Where CategoryId=" + categoryId);
            return products;
        }

    }
}
