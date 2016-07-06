using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind
{
    public partial class Product : MyTClass
    {
        public string QuantityPerUnit { get; set; }
        public virtual Category Category { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
