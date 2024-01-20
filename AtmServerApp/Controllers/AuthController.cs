using DataAccessLayer;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer;

namespace AtmServerApp.Controllers
{
    [ApiController]
    [Route("/api/v1/auth/")]
    public class AuthController : Controller
    {
        private UserService userService;
        private JWTAuthManager authManager;
        public AuthController(UserService userService, JWTAuthManager authManager)
        {
            this.userService = userService;
            this.authManager = authManager;
        }
     
        [HttpPost("signIn")]
        public IActionResult SignIn(UserDto user)
        {
            var userDetail = userService.SignIn(user.UserName, user.Password);
            if (userDetail.Message.Contains("successful"))
            {
                // generate token
                var token = authManager.GenerateToken(user.UserName);

                userDetail.Token = token;

                return Ok(userDetail);
            }
            else if (userDetail.Message.Contains("failed"))
            {
                return StatusCode(StatusCodes.Status417ExpectationFailed, userDetail);
            }
            else
            {
                return BadRequest(userDetail);
            }
        }
    }
}
