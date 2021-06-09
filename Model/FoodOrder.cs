using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riverbed.Test.FreshBrew.Models
{
    public class FoodOrder
    {
        
        //name of food like 'Blueberry muffin'
        [JsonProperty("name")]
        public string Name { get; set; }

        //category could be 'bakery', 'sandwich','icecream'
        [JsonProperty("category")]
        public string Category { get; set; }
    }
}
