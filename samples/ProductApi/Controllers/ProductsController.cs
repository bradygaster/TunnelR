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

        [HttpGet(Name = nameof(Get))]
        public IEnumerable<Product> Get()
        {
            return _products;
        }

        //[HttpGet("{id}", Name = nameof(GetProduct))]
        //public Product GetProduct([FromRoute] int id)
        //{
        //    return _products.First(x => x.Id == id);
        //}

        //[HttpPost(Name = nameof(CreateProduct))]
        //public Product CreateProduct([FromBody] Product product)
        //{
        //    return product;
        //}
    }
}