using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Models;
using Domain.Models.identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.identity;

namespace Persistence
{
    public class DbInitializer : IDbInitializer
    {
        private readonly StoreDbContext _context;
        private readonly StoreIdentityDbContext _identityDbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(StoreDbContext context,StoreIdentityDbContext identityDbContext
            ,UserManager<AppUser> userManager , RoleManager<IdentityRole> roleManager
            )
        {
            _context = context;
            _identityDbContext = identityDbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task InitializeAsync()
        {
            try
            {
                //Create DataBase If Doesn't Exists && Apply To Any Pending Migrations

                if (_context.Database.GetPendingMigrations().Any()) 
                
                { 
                   await _identityDbContext.Database.MigrateAsync();
                }
                //Data Seeding

                //Seeding ProductTypes From Json Files


                if (!_context.ProductTypes.Any())
                {
                    //1.Read All Data From Types Json Files
                    var typesData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\types.json");

                    //2.Transform String To C# Objects [List<ProductTypes>]
                    var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);

                    //3.Add List<ProductTypes> To DataBase
                    if (types is not null && types.Any())
                    {
                        await _context.ProductTypes.AddRangeAsync(types);
                        await _context.SaveChangesAsync();
                    }


                }




                //Seeding ProductBrands From Json Files

                if (!_context.ProductBrands.Any())
                {
                    //1.Read All brands From Types Json Files
                    var brandsData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\brands.json");

                    //2.Transform String To C# Objects [List<ProductTypes>]
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                    //3.Add List<ProductBrands> To DataBase
                    if (brands is not null && brands.Any())
                    {
                        await _context.ProductBrands.AddRangeAsync(brands);
                        await _context.SaveChangesAsync();
                    }


                }

                //Seeding Products From Json Files


                if (!_context.Products.Any())
                {
                    //1.Read All Data From products Json Files
                    var productsData = await File.ReadAllTextAsync(@"..\Infrastructure\Persistence\Data\Seeding\products.json");

                    //2.Transform String To C# Objects [List<Products>]
                    var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                    //3.Add List<Product> To DataBase
                    if (products is not null && products.Any())
                    {
                        await _context.Products.AddRangeAsync(products);
                        await _context.SaveChangesAsync();
                    }


                }
            }
            catch (Exception) 
            { 

            }

        }

        public async Task InitializeIdentityAsync()
        {
            //Create DataBase If Doesn't Exists && Apply To Any Pending Migrations
            if (_context.Database.GetPendingMigrations().Any())
            {
                await _identityDbContext.Database.MigrateAsync();
            }


            if(!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole()
                {
                    Name="Admin"
                });

                await _roleManager.CreateAsync(new IdentityRole()
                {
                    Name = "SuperAdmin"
                });
            }

            //Seeding
            if (!_userManager.Users.Any())
            {
                var SuperAdminUser = new AppUser()
                {
                    DisplayName="Super Admin",
                    Email="SuperAdmin@gmail.com",
                    UserName="SuperAdmin",
                    PhoneNumber="0123456789"
                };

                var adminUser = new AppUser()
                {
                    DisplayName = "Admin",
                    Email = "Admin@gmail.com",
                    UserName = "Admin",
                    PhoneNumber = "0123456789"
                };

               await _userManager.CreateAsync(SuperAdminUser, "P@ssW0rd");
                await _userManager.CreateAsync(adminUser, "P@ssW0rd");

               await _userManager.AddToRoleAsync(SuperAdminUser, "SuperAdmin");
                await _userManager.AddToRoleAsync(adminUser, "Admin");
            }

        }
    }
}
//..\Infrastructure\Persistence\Data\Seeding\types.json