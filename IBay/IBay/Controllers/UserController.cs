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
<<<<<<< Updated upstream
        public IActionResult Create(string userPseudo, string userEmail, string userPassword)
=======
        [SwaggerOperation("Create a new user")]
        [SwaggerResponse(200, "User created successfully")]
        public IActionResult Create(string user_pseudo, string user_email, string user_password)
>>>>>>> Stashed changes
        {
            var newUser = context.CreateUser(user_pseudo, user_email, user_password);
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
                return NotFound("User not found : " + id);
            }
            return Ok(user);
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] User user)
        {
<<<<<<< Updated upstream
            var updatedUser = context.UpdateUser(id, user.UserEmail, user.UserPseudo, user.UserPassword);
=======
            if (!IsSelfTargetting(HttpContext))
            {
                return Forbid();
            }

            var updatedUser = context.UpdateUser(id, user.user_email, user.user_pseudo, user.user_password);
>>>>>>> Stashed changes
            if (updatedUser == null)
            {
                return NotFound("User not found : " + id);
            }
            return Ok(updatedUser);
        }

        [HttpPut("{id:int}/money/{money:double}")]
        public IActionResult UpdateMoney(int id, double money)
        {
            var updatedUser = context.UpdateUserMoney(id, money);
            if (updatedUser == null)
            {
                return NotFound("User not found : " + id);
            }
            return Ok(updatedUser);
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var user = context.DeleteUser(id);
            if (user == null)
            {
                return NotFound("User not found : " + id);
            }
            return Ok(user);
        }


    }
}
