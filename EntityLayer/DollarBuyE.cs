using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class DollarBuyE
    {
        [JsonProperty("fecha")]
        public string Date { get; set; }
        [JsonProperty("valor")]
        public double Value { get; set; }
    }
}
