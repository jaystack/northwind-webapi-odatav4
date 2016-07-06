using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind
{
    public abstract class MyTClass
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        //public IDictionary<string, object> ttt { get; set; }
    }


    public partial class Category : MyTClass
    {
        //[Timestamp]
        public string Description { get; set; }
        public virtual IList<Product> Products { get; set; }
    }
}
