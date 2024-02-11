using DAL.Data;
using DAL.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static IBay.AuthorizationMiddleware;
using Swashbuckle.AspNetCore.Annotations;

namespace IBay.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [SwaggerTag("User Endpoints")]
    public class UserController(IIbayContext context) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        [SwaggerOperation("Create a new user")]
        [SwaggerResponse(200, "User created successfully")]
        public IActionResult Create(string userPseudo, string userEmail, string userPassword)
        {
            try { 
                var newUser = context.CreateUser(userPseudo, userEmail, userPassword);
                return Ok(newUser);
            }catch(IbayContext.BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [SwaggerOperation("Get all users")]
        [SwaggerResponse(200, "List of all users")]
        public IActionResult Get()
        {
            var users = context.GetUsers();
            return Ok(users);
        }

        [HttpGet("{id:int}")]
        [SwaggerOperation("Get a user by id")]
        [SwaggerResponse(200, "User found")]
        [SwaggerResponse(404, "User not found")]
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
        [SwaggerOperation("Update a user")]
        [SwaggerResponse(200, "User updated successfully")]
        [SwaggerResponse(404, "User not found")]
        public IActionResult Update(int id, [FromBody] User user)
        {
            if (!IsSelfTargetting(HttpContext))
            {
                return Forbid();
            }
            try
            {
                var updatedUser = context.UpdateUser(id, user.user_email, user.user_pseudo, user.user_password);
                if (updatedUser == null)
                {
                    return NotFound();
                }

                return Ok(updatedUser);
            }catch(IbayContext.BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
                
            
        }

        [HttpPut("{id:int}/money/{money:double}")]
        [SwaggerOperation("Update user money")]
        [SwaggerResponse(200, "User's money updated successfully")]
        [SwaggerResponse(404, "User not found")]
        public IActionResult UpdateMoney(int id, double money)
        {
            try
            {
                var updatedUser = context.UpdateUserMoney(id, money);
                if (updatedUser == null)
                {
                    return NotFound();
                }
                return Ok(updatedUser);
            }catch(IbayContext.NotFoundException ex)
            {
                return NotFound(ex.Message);
            }catch(IbayContext.BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpDelete("{id:int}")]
        [SwaggerOperation("Delete a user")]
        [SwaggerResponse(200, "User deleted successfully")]
        [SwaggerResponse(404, "User not found")]
        public IActionResult Delete(int id)
        {
            if (!IsSelfTargetting(HttpContext))
            {
                return Forbid();
            }
            try
            {
                var user = context.DeleteUser(id);
                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }catch(IbayContext.NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            
        }
    }
}

