# Demonstration Process

This will walk you through the process of trying the test tunnel extension. You'll need to have an API Management instance in your Azure subscription. 

1. Open the API Management service you created and go to the **APIs** tool in the Azure portal.

1. In Visual Studio or Visual Studio Code, copy the `Get` method already in the controller. Paste in the copied code. Then, and change the name of the method (and the `nameof`) to be `GetProduct`. 

1. Debug the project.

1. Put a breakpoint on the `GetProduct` method. Then call the `GetProduct` method from within the API Management portal blade using 1 as the parameter. Notice how it returns all the products instead of the single one you requested. 

1. Stop the debugger and change the `GetProduct` code to return a single product.

    ```csharp
    [HttpGet("{id}", Name = nameof(GetProduct))]
    public Product GetProduct([FromRoute] int id)
    {
        return _products.First(x => x.Id == id);
    }
    ```

1. Debug the project. Note how this time the API structure has changed in the portal to reflect that it returns a single product instead of a list of products. 

1. Add this code as a parameter to the `services.AddSwaggerGen()` method call in `Startup.cs`. 
    ```csharp
    setup => 
    {
        setup.SwaggerDoc("v2", new OpenApiInfo
        {
            Title = "Products API (v2)",
            Version = "v2"
        });
    }
    ```

1. Debug the project. 

1. Note in the API Management portal that you now have 2 versions of the API. 

    ```csharp
    [HttpPost(Name = nameof(CreateProduct))]
    public Product CreateProduct([FromBody] Product product)
    {
        return product;
    }
    ```

1. Change the version of the API to be v3 in `Startup.cs`.

    ```csharp
    setup => 
    {
        setup.SwaggerDoc("v3", new OpenApiInfo
        {
            Title = "Products API (v2)",
            Version = "v3"
        });
    }
    ```

1. Add the `UpdateProduct` method.

    ```csharp
    [HttpPut("{id}", Name = nameof(UpdateProduct))]
    public ActionResult<Product> UpdateProduct([FromRoute] int id, [FromBody] Product product)
    {
        var existing = _products.FirstOrDefault(x => x.Id == id);
        if (existing == null)
        {
            return NotFound();
        }
        existing.Name = product.Name;

        return Ok(existing);
    }
    ```

1. Debug the project and note how the `UpdateProduct` method is now in `v3` of the API, but not in the other versions.