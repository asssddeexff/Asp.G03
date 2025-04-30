using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Models.identity;
using Stripe;

namespace Domain.OrderModel
{
    public class Order : BaseEntity<Guid>
    {
        public Order()
        {
             
        }
        public Order(string userEmail, Address shippingAddress, ICollection<OrderItem> orderitem, DeliveryMethod deliveryMethod, decimal subTotal, string paymentIntentId)
        {
            Id = Guid.NewGuid();    
            UserEmail = userEmail;
            ShippingAddress = shippingAddress;
            this.orderitem = orderitem;
            DeliveryMethod = deliveryMethod;
            SubTotal = subTotal;
            PaymentIntentId = paymentIntentId;
        }


        //Id
        //User Email

        public string UserEmail { get; set; }

        //Shipping Address

        public Address ShippingAddress { get; set; }
        //Order Items
        public ICollection<OrderItem> orderitem { get; set; } = new List<OrderItem>();//Navigational Property

        //Delivery Method
        public DeliveryMethod DeliveryMethod { get; set; }//Navigational Property

        public int? DeliveryMethodId { get; set; }//FK

        //Payment Status
        public OrderPaymentStatus PaymentStatus {  get; set; }= OrderPaymentStatus.Pending;
        //Sub Total 

        public decimal SubTotal { get; set; }
        //Order Date 
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;

        //Payment
        public string PaymentIntentId { get; set; }
    }
}
