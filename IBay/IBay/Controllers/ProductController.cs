using DAL.Data;
using DAL.Model;
using Microsoft.AspNetCore.Mvc;

namespace IBay.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController(IIbayContext context) : ControllerBase
    {
        [HttpPost("{sellerId:int}")]
        /// <summary> Creates a product </summary>
        /// <param name="sellerId"> The id of the seller </param>
        /// <param name="productName"> The name of the product </param>
        /// <param name="productDescription"> The description of the product </param>
        /// <param name="productType"> The type of the product </param>
        /// <param name="productPrice"> The price of the product </param>
        /// <param name="productStock"> The stock of the product </param>
        /// <returns> The created product </returns>
        /// <response code="200"> Returns the created product </response>
        /// <response code="400"> If the product is not valid </response>
        /// <response code="401"> If the user is not authenticated </response>
        /// <response code="403"> If the user is not authorized </response>
        /// <response code="404"> If the seller is not found </response>
        /// <response code="409"> If the product already exists </response>

        public IActionResult Create(int sellerId, string productName, string productDescription, ProductType productType, double productPrice, int productStock)
        {
            var newProduct = context.CreateProduct(sellerId, productName, productDescription, productType, productPrice, productStock);
            return Ok(newProduct);
        }

        [HttpGet]
        /// <summary> Gets all the products </summary>
        /// <returns> The products </returns>
        /// <response code="200"> Returns the products </response>
        /// <response code="401"> If the user is not authenticated </response>
        /// <response code="403"> If the user is not authorized </response>
        /// <response code="404"> If the products are not found </response>
        public IActionResult Get()
        {
            var products = context.GetProducts();
            return Ok(products);
        }


        [HttpGet("/sort")]
        /// <summary> Gets all the products </summary>
        /// <param name="sortCategory"> The category to sort the products by, 0 = Electronic, 1 = Clothing, 2 = Furniture, 3 = Books, 4 = Other </param>
        /// <param name="limit"> The maximum number of products to return </param>
        /// <returns> The products </returns>
        /// <response code="200"> Returns the products </response>
        /// <response code="400"> If the sort category is not valid </response>
        /// <response code="401"> If the user is not authenticated </response>
        /// <response code="403"> If the user is not authorized </response>
        /// <response code="404"> If the products are not found </response>
        public IActionResult Get(SortCategory sortCategory, int limit)
        {
            var products = context.GetProductSortedBy(sortCategory, limit);
            return Ok(products);
        }

        [HttpGet("{id:int}")]
        /// <summary> Gets a product by its id </summary>
        /// <param name="id"> The id of the product </param>
        /// <returns> The product </returns>
        /// <response code="200"> Returns the product </response>
        /// <response code="404"> If the product is not found </response>
        /// <response code="400"> If the product is not valid </response>
        /// <response code="401"> If the user is not authenticated </response>
        /// <response code="403"> If the user is not authorized </response>
        public IActionResult GetById(int id)
        {
            var product = context.GetProductById(id);
            if (product == null)
            {
                return NotFound("Product not found : " + id);
            }
            return Ok(product);
        }

        [HttpGet("{name}")]
        [SwaggerOperation("Get a product by name")]
        [SwaggerResponse(200, "Product found")]
        [SwaggerResponse(404, "Product not found")]
        public IActionResult GetByName(string name)
        {
            var product = context.GetProductByName(name);
            if (product == null)
            {
                return NotFound("Product not found : " + name);
            }
            return Ok(product);
        }

        [HttpPut("{id:int}")]
        /// <summary> Updates a product </summary>
        /// <param name="id"> The id of the product to update </param>
        /// <param name="product"> The product to update </param>
        /// <returns> The updated product </returns>
        /// <response code="200"> Returns the updated product </response>
        /// <response code="404"> If the product is not found </response>
        /// <response code="400"> If the product is not valid </response>
        public IActionResult Update(int id, [FromBody] Product product)
        {
            var updatedProduct = context.UpdateProduct(id, product.ProductName, product.ProductDescription, product.ProductType, product.ProductPrice, product.ProductStock, product.Available);
            if (updatedProduct == null)
            {
                return NotFound("Product not found : " + id);
            }
            return Ok(updatedProduct);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var product = context.DeleteProduct(id);
            if (product == null)
            {
                return NotFound("Product not found : " + id);
            }
            return Ok(product);
        }
    }
}
