using JayData.Test.CommonItems.Entities;
using Microsoft.OData.Edm.Library;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Model
{
    public class NorthwindContext : DbContext
    {
        public NorthwindContext()
            : base("NorthwindContext")
        {
            System.Data.Entity.Database.SetInitializer<NorthwindContext>(new initdb());
            //System.Data.Entity.Database.SetInitializer<NewsReaderContext>(null);
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
    }

    public class initdb : DropCreateDatabaseAlways<NorthwindContext>
    {
        protected override void Seed(NorthwindContext context)
        {
            DbInitializer.BuildInitData(context);
        }
    }


    public class DbInitializer
    {
        public static void BuildInitData(NorthwindContext context)
        {
            var cat1 = new Category { Name = "Beverages", Description = "Soft drinks, coffees, teas, beers, and ales" };
            var cat2 = new Category { Name = "Condiments", Description = "Sweet and savory sauces, relishes, spreads, and seasonings"};
            var cat3 = new Category { Name = "Confections", Description = "Desserts, candies, and sweet breads"};
            var cat4 = new Category { Name = "Dairy Products", Description = "Cheeses" };
            var cat5 = new Category { Name = "Grains/Cereals", Description = "Breads, crackers, pasta, and cereal" };
            var cat6 = new Category { Name = "Meat/Poultry", Description = "Prepared meats" };
            var cat7 = new Category { Name = "Produce", Description = "Dried fruit and bean curd" };
            var cat8 = new Category { Name = "Seafood", Description = "Seaweed and fish" };

            var prod1 = new Product { Name = "Chai", QuantityPerUnit = "10 boxes x 20 bags", UnitPrice = 18, Category = cat1 };
            var prod2 = new Product { Name = "Chang", QuantityPerUnit = "24 - 12 oz bottles", UnitPrice = 19, Category = cat1 };
            var prod3 = new Product { Name = "Aniseed Syrup", QuantityPerUnit = "12 - 550 ml bottles", UnitPrice = 10, Category = cat1 };
            var prod4 = new Product { Name = "Chef Anton's Cajun Seasoning", QuantityPerUnit = "48 - 6 oz jars", UnitPrice = 22, Category = cat1 };
            var prod5 = new Product { Name = "Grandma's Boysenberry Spread", QuantityPerUnit = "12 - 8 oz jars", UnitPrice = new decimal(25.5), Category = cat1 };

            var categories = new List<Category> { cat1, cat2, cat3, cat4, cat5, cat6, cat7, cat8 };
            var products = new List<Product> { prod1, prod2, prod3, prod4, prod5 };
            context.Categories.AddRange(categories);
            context.Products.AddRange(products);
            context.SaveChanges();

        }
    }
}
