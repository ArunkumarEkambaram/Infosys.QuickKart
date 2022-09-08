using Infosys.QuickKart.DataAccessLayer.Models;
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

        //Add New Product - Model
        public bool AddNewProduct(Products product)
        {
            _dbContext.Products.Add(product);
            int res = _dbContext.SaveChanges(); //Returns no of rows affected
            if (res > 0)
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

    }
}
