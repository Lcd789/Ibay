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
        public IActionResult Create(string UserPseudo, string UserEmail, string UserPassword)
        {
            User newUser = _context.CreateUser(UserPseudo, UserEmail, UserPassword);
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

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] User user)
        {
            User updatedUser = _context.UpdateUser(id, user.UserEmail, user.UserPseudo, user.UserPassword);
            if (updatedUser == null)
            {
                return NotFound();
            }
            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            User user = _context.DeleteUser(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

    }
}
