using EntityLayer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public  class DollarDataE
    {
        [JsonProperty("venta")]
        public DollarSaleE DollarSaleE { get; set; }
        [JsonProperty("compra")]
        public DollarBuyE DollarBuyE { get; set; }
    }
}
