﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Abstractions
{
    public interface IServiceManager
    {
        IProductService ProductService { get; }
         IBasketService BasketService { get; }

        IChacheService chacheService { get; }
        IAuthService AuthService { get; }   
    }
}
