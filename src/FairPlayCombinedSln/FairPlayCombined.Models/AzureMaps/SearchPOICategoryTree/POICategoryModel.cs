using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.AzureMaps.SearchPOICategoryTree
{
    public class POICategoryModel
    {
        public List<POICategoryModel>? Children { get; set; }
        public int Id { get; set; }
        public string? Name { get; set; }
        public string[]? Synonyms { get; set; }
    }
}
