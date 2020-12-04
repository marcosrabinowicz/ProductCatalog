using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Data;
using ProductCatalog.Models;
using ProductCatalog.ViewModels;
using ProductCatalog.ViewModels.ProductViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductCatalog.Controllers
{
    [Route("v1/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly StoreDataContext _context;

        public ProductController(StoreDataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<ListProductViewModel> Get()
        {
            return _context.Products
                .Include(x => x.Category)
                .Select(x => new ListProductViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Price = x.Price,
                    Category = x.Category.Title,
                    CategoryId = x.Category.Id
                })
                .AsNoTracking()
                .ToList();
        }

        [HttpGet("{id}")]
        public Product Get(int id)
        {
            return _context.Products.AsNoTracking().Where(x => x.Id == id).FirstOrDefault();
        }

        [HttpPost]
        public ResultViewModel Post([FromBody]EditorProductViewModel model)
        {
            var product = new Product();

            product.Title = model.Title;
            product.CategoryId = model.CategoryId;
            product.CreateDate = DateTime.Now;
            product.Description = model.Description;
            product.Image = model.Image;
            product.LastUpdateDate = DateTime.Now;
            product.Price = model.Price;
            product.Quantity = model.Quantity;
            
            _context.Products.Add(product);
            
            _context.SaveChanges();

            return new ResultViewModel
            {
                Success = true,
                Message = "Produto cadastrado com sucesso.",
                Data = product
            };
        }

        [HttpPut]
        public ResultViewModel Put([FromBody]EditorProductViewModel model)
        {
            var product = _context.Products.Find(model.Id);

            product.Title = model.Title;
            product.CategoryId = model.CategoryId;
            product.Description = model.Description;
            product.Image = model.Image;
            product.LastUpdateDate = DateTime.Now;
            product.Price = model.Price;
            product.Quantity = model.Quantity;

            _context.Entry<Product>(product).State = EntityState.Modified;

            _context.SaveChanges();

            return new ResultViewModel
            {
                Success = true,
                Message = "Produto alterado com sucesso.",
                Data = product
            };
        }

        [HttpDelete]
        public ResultViewModel Delete([FromBody]EditorProductViewModel model)
        {
            var product = _context.Products.Find(model.Id);
            
            _context.Products.Remove(product);

            _context.SaveChanges();

            return new ResultViewModel
            {
                Success = true,
                Message = "Produto excluido com sucesso.",
                Data = product
            };
        }
    }
}
