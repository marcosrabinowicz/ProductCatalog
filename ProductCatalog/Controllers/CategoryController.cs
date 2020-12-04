using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Data;
using ProductCatalog.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProductCatalog.Controllers
{
    [Route("v1/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly StoreDataContext _context;

        public CategoryController(StoreDataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<Category> Get()
        {
            return _context.Categories.AsNoTracking().ToList();
        }

        [HttpGet("{id}")]
        public Category Get(int id)
        {
            return _context.Categories.AsNoTracking().Where(x => x.Id == id).FirstOrDefault();
        }

        [HttpGet("{id}/products")]
        public IEnumerable<Product> GetProducts(int id)
        {
            return _context.Products.AsNoTracking().Where(x => x.CategoryId == id).ToList();
        }

        [HttpPost]
        public Category Post([FromBody]Category category)
        {
            _context.Categories.Add(category);
            
            _context.SaveChanges();

            return category;
        }

        [HttpPut]
        public Category Put([FromBody]Category category)
        {
            _context.Entry<Category>(category).State = EntityState.Modified;

            _context.SaveChanges();

            return category;
        }

        [HttpDelete]
        public Category Delete([FromBody]Category category)
        {
            _context.Categories.Remove(category);

            _context.SaveChanges();

            return category;
        }
    }
}
