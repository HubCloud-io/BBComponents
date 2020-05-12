using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerSideForDebug.Data
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsDeleted { get; set; }
    }
}
