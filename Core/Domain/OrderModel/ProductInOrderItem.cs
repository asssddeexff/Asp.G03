using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.OrderModel
{
    public class ProductInOrderItem
    {
        public ProductInOrderItem()
        {
            
        }
        public ProductInOrderItem(int prouductId, string productName, string pictureUrl)
        {
            ProuductId = prouductId;
            ProductName = productName;
            PictureUrl = pictureUrl;
        }

        public int ProuductId { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
    }
}
