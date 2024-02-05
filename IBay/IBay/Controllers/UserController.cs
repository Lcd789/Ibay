using DAL.Data;
using DAL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static IBay.AuthorizationMiddleware;

namespace IBay.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController(IIbayContext context) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Create(string userPseudo, string userEmail, string userPassword)
        {
            var newUser = context.CreateUser(userPseudo, userEmail, userPassword);
            return Ok(newUser);
        }

        [HttpGet]
        public IActionResult Get()
        {
            var users = context.GetUsers();
            return Ok(users);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
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
            if (!IsSelfTargetting(HttpContext))
            {
                return Forbid();
            }

            var updatedUser = context.UpdateUser(id, user.UserEmail, user.UserPseudo, user.UserPassword);
            if (updatedUser == null)
            {
                return NotFound();
            }

            return Ok(updatedUser);
        }

        [HttpPut("{id:int}/money/{money:double}")]
        public IActionResult UpdateMoney(int id, double money)
        {
            var updatedUser = context.UpdateUserMoney(id, money);
            if (updatedUser == null)
            {
                return NotFound();
            }
            return Ok(updatedUser);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            if (!IsSelfTargetting(HttpContext))
            {
                return Forbid();
            }

            var user = context.DeleteUser(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }


    }
}
