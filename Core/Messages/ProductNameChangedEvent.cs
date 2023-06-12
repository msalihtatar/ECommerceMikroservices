using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Messages
{
    public class ProductNameChangedEvent
    {
        public string ProductId { get; set; }
        public string UpdatedProductName { get; set; }
    }
}
