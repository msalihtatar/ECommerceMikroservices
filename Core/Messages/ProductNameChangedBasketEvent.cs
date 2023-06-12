using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Messages
{
    public class ProductNameChangedBasketEvent
    {
        public string UserId { get; set; }
        public string UpdatedProductName { get; set; }
    }
}
