using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.OrderModels;

namespace Service.Abstractions
{
    public interface IOrderService
    {
        //Get Order By Id 

       Task<OrderResultDto> GetOrderByIdAsync(Guid id);
        //Get Order
        Task<IEnumerable<OrderResultDto>> GetOrderByUserEmailAsync(string userEmail);

        //Create Order 

        Task<OrderResultDto> CreateOrderAsync(OrderRequestDto orderRequestDto , string userEmail );
        //Get All Delivery Methods
        //
       Task<IEnumerable<DeliveryMethodDto>> GetAllDeliveryMethods();

    }
}
