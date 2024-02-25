using Azure;
using LabWorld.Model;
using LabWorld.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LabWorld.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository; // New user repository for authentication

        public AuthController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> register(UserModel user)
        {
            try
            {
              await _userRepository.Register(user);

                return Ok("Data save succesfully");
              
            }
            catch (Exception ex) {
                if (ex.InnerException != null)
                {
                    
                    var innerException = ex.InnerException;
                    Console.WriteLine(innerException.Message);
                }
                return BadRequest("Failed to register user" +ex.Message);
            }
           
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserModel userLogin)
        {
            var loginResult = await _userRepository.Login(userLogin);

            if (loginResult.Success)
            {
                return Ok(new { Token = loginResult.Token }); // Successful login
            }
            else
            {
                if (!string.IsNullOrEmpty(loginResult.ErrorMessage))
                {
                    return BadRequest(loginResult.ErrorMessage); // Error message
                }
                else
                {
                    return Unauthorized(); // Unauthorized
                }
            }
        }

        [HttpPost("Registration")]
        public async Task<IActionResult> Registration( RegistrationDTO registration)
        {
            try
            {

                 await _userRepository.LabRegister(registration);
                return Ok("Data save succesfully"); ;
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

    }
}
