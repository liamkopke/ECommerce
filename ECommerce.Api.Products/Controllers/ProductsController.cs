﻿using ECommerce.Api.Products.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Products.Controllers
{
    [ApiController]
    [Route("api/products")]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsProvider productsProvider;

        public ProductsController(IProductsProvider productsProvider)
        {
            this.productsProvider = productsProvider;
        }

        /// <summary>
        /// Get all products
        /// </summary>
        /// <returns>An IActionResult</returns>
        /// <response code="200">Returns all the products</response>
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductsAsync()
        {
            var result = await productsProvider.GetProductsAsync();
            if (result.isSuccess)
            {
                return Ok(result.Products);
            }
            return NotFound();
        }

        /// <summary>
        /// Get product by the provided id.
        /// </summary>
        /// <param name="id">The product id to get</param>
        /// <returns>An IActionResult</returns>
        /// <response code="200">Returns the requested product</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductsAsync(int id)
        {
            var result = await productsProvider.GetProductsAsync(id);
            if (result.isSuccess)
            {
                return Ok(result.Product);
            }
            return NotFound();
        }
    }
}
