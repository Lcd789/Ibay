using DAL.Data;
using DAL.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IBay.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        //Création d'un contexte pour pouvoir utiliser les méthodes du DAL
        private IIbayContext _context;

        public CartController(IIbayContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Create(Cart cart)
        {
            Cart newCart = _context.CreateCart(cart);
            return Ok(newCart);
            
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Cart cart = _context.GetCartById(id);
            if (cart == null)
            {
                return NotFound();
            }
            return Ok(cart);
        }
    }
}
