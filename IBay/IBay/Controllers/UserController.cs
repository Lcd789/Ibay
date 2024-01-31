using DAL.Data;
using DAL.Model;
using Microsoft.AspNetCore.Mvc;

namespace IBay.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IIbayContext context) : ControllerBase
    {
        [HttpPost]
        public IActionResult Create(string userPseudo, string userEmail, string userPassword)
        {
            var newUser = context.CreateUser(userPseudo, userEmail, userPassword);
            return Ok(newUser);
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var user = context.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] User user)
        {
            var updatedUser = context.UpdateUser(id, user.UserEmail, user.UserPseudo, user.UserPassword);
            if (updatedUser == null)
            {
                return NotFound();
            }
            return Ok(updatedUser);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var user = context.DeleteUser(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        
        [HttpPut("{id:int}/money/{money:double}")]
        public IActionResult UpdateUserMoney(int id, double money)
        {
            var updatedUser = context.UpdateUserMoney(id, money);
            if (updatedUser == null)
            {
                return NotFound();
            }
            return Ok(updatedUser);
        }
    }
}
