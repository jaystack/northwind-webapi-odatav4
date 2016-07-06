using Northwind;
using Northwind.Model;
using Microsoft.Spatial;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;

namespace Northwind.Controllers
{
    public class ProductsController : NewsBaseODataController<Product>
    {

        [EnableQuery(MaxExpansionDepth = 10)]
        public IQueryable<Product> Get()
        {
            return db.Products;
        }

        [EnableQuery]
        public SingleResult<Product> Get([FromODataUri] int key)
        {
            IQueryable<Product> result = db.Products.Where(p => p.Id == key);
            return SingleResult.Create(result);
        }

        public async Task<IHttpActionResult> Post(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Products.Add(product);
            await db.SaveChangesAsync();
            return Created(product);
        }

        public async Task<IHttpActionResult> Patch([FromODataUri] int key, Delta<Product> product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var entity = await db.Products.FindAsync(key);
            if (entity == null)
            {
                return NotFound();
            }
            product.Patch(entity);
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Updated(entity);
        }
        
        public async Task<IHttpActionResult> Put([FromODataUri] int key, Product update)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (key != update.Id)
            {
                return BadRequest();
            }
            db.Entry(update).State = EntityState.Modified;
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Updated(update);
        }
        
        public async Task<IHttpActionResult> Delete([FromODataUri] int key)
        {
            var product = await db.Products.FindAsync(key);
            if (product == null)
            {
                return NotFound();
            }
            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return StatusCode(HttpStatusCode.NoContent);
        }

        [EnableQuery]
        public SingleResult<Category> GetCategory([FromODataUri] int key)
        {
            return SingleResult.Create( db.Products.Where(a => a.Id == key).Select(a=>a.Category));
        }


        public string GetTitleFromCategories([FromODataUri] int key)
        {
            return "category title";
        }

        private bool ProductExists(int key)
        {
            return db.Products.Any(p => p.Id == key);
        }




        [HttpPost]
        public string GetFirstArticleTitle([FromODataUri] int key, ODataActionParameters parameters)
        {
            var contains = (string)parameters["contains"] ?? "";
            return db.Products.Where(a => a.Category.Id == key && a.Name.Contains(contains)).Select(a => a.Name).FirstOrDefault() ?? " - ";
        }

        [HttpPost]
        public string GetFirstArticleTitle(ODataActionParameters parameters)
        {
            var contains = (string)parameters["contains"] ?? "";
            return db.Products.Where(a => a.Name.Contains(contains)).Select(a => a.Name).FirstOrDefault() ?? " - ";
        }

        [HttpPost]
        public GeographyPoint LocationSwipe([FromODataUri] int key, ODataActionParameters parameters)
        {
            var loc = (GeographyPoint)parameters["Loc"] ?? null;
            return GeographyPoint.Create(loc.Longitude, loc.Latitude);
        }



        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
