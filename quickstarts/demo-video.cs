// phase

services.AddSwaggerGen();

// phase

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ProductApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
    }
}

// phase

host.UseApiTestTunnel(app, builder => 
                {
                    builder
                        .UseNGrok()
                        .UseAzureApiMangement();
                });

// phase

public class Product
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        private Product[] _products = new Product[]
        {
            new Product { Id = 1, Name = "Drum Machine" },
            new Product { Id = 2, Name = "MIDI Controller" },
            new Product { Id = 3, Name = "Frequency Modulator" },
            new Product { Id = 4, Name = "Sampler" },
        };

        [HttpGet(Name=nameof(Get))]
        public IEnumerable<Product> Get()
        {
            return _products;
        }

// phase

[HttpGet("{id}", Name=nameof(GetProduct))]
        public IEnumerable<Product> GetProduct([FromRoute] int id)
        {
            return _products;
        }

// phase

public Product GetProduct([FromRoute] int id)
        {
            return _products.First(x => x.Id == id);
        }

// phase 

services.AddSwaggerGen(setup => 
            {
                setup.SwaggerDoc("v2", new OpenApiInfo
                {
                    Title = "Products API (v2)",
                    Version = "v2"
                });
            });

[HttpPost(Name=nameof(CreateProduct))]
        public Product CreateProduct([FromBody] Product product)
        {
            return product;
        }

// phase 

setup.SwaggerDoc("v2", new OpenApiInfo
                {
                    Title = "Products API (v2)",
                    Version = "v2"
                });

// phase

setup.SwaggerDoc("v3", new OpenApiInfo
                {
                    Title = "Products API (v3)",
                    Version = "v3"
                });

[HttpPut("{id}", Name=nameof(UpdateProduct))]
        public Product UpdateProduct([FromRoute] int id, [FromBody] Product product)
        {
            var existing = _products.First(x => x.Id == id);
            existing.Name = product.Name;
            return product;
        }

// phase

[HttpPut("{id}", Name=nameof(UpdateProduct))]
        public ActionResult<Product> UpdateProduct([FromRoute] int id, [FromBody] Product product)
        {
            var existing = _products.FirstOrDefault(x => x.Id == id);
            if(existing == null)
            {
                return NotFound();
            }
            existing.Name = product.Name;
            
            return Ok(existing);
        }