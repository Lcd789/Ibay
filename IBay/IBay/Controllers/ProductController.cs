using DAL.Data;
using DAL.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IBay.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        //Création d'un contexte pour pouvoir utiliser les méthodes du DAL
        private IIbayContext _context;

        public ProductController(IIbayContext context)
        {
            _context = context;
        }

        [HttpPost("{sellerId}")]
        public IActionResult Create(int sellerId, string productname, string productDescription, double productPrice, int productStock)
        {
            Product newProduct = _context.CreateProduct(sellerId, productname, productDescription, productPrice, productStock);
            return Ok(newProduct);
            
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Product product = _context.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpGet("{userid}")]
        public IActionResult GetProductsOnSale(int userId)
        {
            List<Product> product = _context.GetProductsOnSale(userId).ToList();
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Product product)
        {
            Product updatedProduct = _context.UpdateProduct(id, product.ProductName, product.ProductDescription, product.ProductPrice, product.ProductStock) ;
            if (updatedProduct == null)
            {
                return NotFound();
            }
            return Ok(updatedProduct);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Product product = _context.DeleteProduct(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        
    }
}
