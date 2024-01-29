using DAL.Data;
using DAL.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace IBay.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        //Création d'un contexte pour pouvoir utiliser les méthodes du DAL
        private IIbayContext _context;

        public UserController(IIbayContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Create(User user)
        {
            User newUser = _context.CreateUser(user);
            return Ok(newUser);
            
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            User user = _context.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

    }
}
