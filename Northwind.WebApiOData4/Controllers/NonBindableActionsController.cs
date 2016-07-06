using Northwind;
using Northwind.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;

namespace Northwind.Controllers
{
    [RoutePrefix("odata")]
    public class NonBindableActionsController : ODataController
    {
        NorthwindContext db = new NorthwindContext();


        [HttpPost]
        [Route("SAction1")]
        public string SAction1(ODataActionParameters param)
        {
            var number = (int)param["number"];
            return "a1_ " + number.ToString();
        }

        [HttpPost]
        [Route("SAction2")]
        [EnableQuery]
        public IQueryable<Product> SAction2(ODataActionParameters param)
        {
            var count = (int)param["count"];
            return db.Products.Take(count);
        }

        [HttpGet]
        [Route("SFunction1")]
        public List<string> SFunction1(int number)
        {
            return new List<string>() { "f1_ ", number.ToString() };
        }

        [HttpGet]
        [Route("SFunction2")]
        public string SFunction2(int number)
        {
            return "f2_ " + number.ToString();
        }




        [HttpPost]
        [Route("Delete")]
        public void Delete()
        {
           
            db.Products.RemoveRange(db.Products);
            db.Categories.RemoveRange(db.Categories);
            db.SaveChanges();
        }


        [HttpPost]
        [Route("InitDb")]
        public string InitDb()
        {
            Delete();
            DbInitializer.BuildInitData(db);
            db.SaveChanges();
            return "ok";
        }



        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
