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
        public IActionResult Create(int sellerId, string productName, string productDescription, ProductType productType, double productPrice, int productStock)
        {
            var newProduct = context.CreateProduct(sellerId, productName, productDescription, productType, productPrice, productStock);
            return Ok(newProduct);
        }

        [HttpGet]
        public IActionResult Get(SortCategory sortCategory, int limit)
        {
            var products = context.GetProducts(sortCategory, limit);
            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var product = context.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpGet("user/{userid:int}/products")]
        public IActionResult GetProductsOnSale(int userId)
        {
            var product = context.GetProductsOnSale(userId).ToList();
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] Product product)
        {
            var updatedProduct = context.UpdateProduct(id, product.ProductName, product.ProductDescription, product.ProductType, product.ProductPrice, product.ProductStock, product.Available);
            if (updatedProduct == null)
            {
                return NotFound();
            }
            return Ok(updatedProduct);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var product = context.DeleteProduct(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
    }
}
