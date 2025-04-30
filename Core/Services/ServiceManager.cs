using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Models.identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Service.Abstractions;
using Shared;

namespace Services
{
    public class ServiceManager(IUnitOfWork unitOfWork , IMapper mapper,IBasketRepository basketRepository , ICacheRepository cacheRepository , UserManager<AppUser> userManager, IOptions<JwtOptions> options) : IServiceManager
    {
        public IProductService ProductService { get; } = new ProductService(unitOfWork , mapper);

        public IBasketService BasketService { get; } = new BasketService(basketRepository,mapper);

        public IChacheService chacheService { get; } = new CacheService(cacheRepository);

        public IAuthService AuthService { get; } = new AuthService(userManager, options);
    }
}
