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

        [HttpPost]
        public IActionResult Create(Product product)
        {
            Product newProduct = _context.CreateProduct(product);
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
        
    }
}
