﻿using AutoMapper;
using ECommerce.Api.Customers.Db;
using ECommerce.Api.Customers.Interfaces;
using ECommerce.Api.Customers.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers.Providers
{
    public class CustomersProvider : ICustomersProvider
    {
        private readonly CustomersDbContext dbContext;
        private readonly ILogger<CustomersProvider> logger;
        private readonly IMapper mapper;
        public CustomersProvider(CustomersDbContext dbContext, ILogger<CustomersProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }
        public async Task<(bool isSuccess, IEnumerable<Models.Customer> Customers, string ErrorMessage)> GetCustomersAsync()
        {
            try
            {
                var Customers = await dbContext.Customers.ToListAsync();
                if(Customers != null && Customers.Any())
                {
                    var result = mapper.Map<IEnumerable<Db.Customer>, IEnumerable<Models.Customer>>(Customers);
                    return (true, result, null);
                }
                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool isSuccess, Models.Customer Customer, string ErrorMessage)> GetCustomersAsync(int id)
        {
            try
            {
                var Customer = await dbContext.Customers.FirstOrDefaultAsync(p => p.Id == id);
                if (Customer != null)
                {
                    var result = mapper.Map<Db.Customer, Models.Customer>(Customer);
                    return (true, result, null);
                }
                return (false, null, "Not found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        private void SeedData()
        {
            if (!dbContext.Customers.Any())
            {
                dbContext.Customers.Add(new Db.Customer() { Id = 1, Name = "Annie", Address = "99 Clover Field Lane" });
                dbContext.Customers.Add(new Db.Customer() { Id = 2, Name = "Bob", Address = "63 Park Avenue" });
                dbContext.Customers.Add(new Db.Customer() { Id = 3, Name = "Derek", Address = "86 Broadway Boulevard" });
                dbContext.Customers.Add(new Db.Customer() { Id = 4, Name = "Steven", Address = "25 Seaside Promenade" });
                dbContext.SaveChanges();
            }
        }
    }
}
