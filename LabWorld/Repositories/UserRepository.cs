using LabWorld.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LabWorld.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<UserModel> _passwordHasher;
        private readonly string jwtSecretKey = "n8EqT8q5u9#Ty4Fw@6#kL2S!w3JpHr%G";

        public UserRepository(ApplicationDbContext context, IPasswordHasher<UserModel> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<IActionResult> Register(UserModel userRegistration)
        {
            try
            {

                var existingUser = await _context.User_Details.FirstOrDefaultAsync(x => x.Email == userRegistration.Email);


                if (existingUser != null)
                {
                    return new ConflictResult(); // Email already exists, return 409 Conflict
                }

                UserModel newUser = new UserModel
                {
                    FirstName = userRegistration.FirstName,
                    LastName = userRegistration.LastName,
                    Email = userRegistration.Email,
                    PhoneNumber = userRegistration.PhoneNumber,
                    Password = _passwordHasher.HashPassword(null, userRegistration.Password),
                };

                _context.User_Details.Add(newUser);
                await _context.SaveChangesAsync();

                return new OkResult(); // Registration successful, return 200 OK
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Failed to register user: {ex.Message}");
            }
        }

        public async Task<LoginResult> Login(UserModel userLogin)
        {
            try
            {
               
                var user = await _context.User_Details.FirstOrDefaultAsync(u => u.Email == userLogin.Email);

                if (user == null || !_passwordHasher.VerifyHashedPassword(null, user.Password, userLogin.Password).Equals(PasswordVerificationResult.Success))
                {
                    return  new LoginResult { Success = true,ErrorMessage= "Invalid email or password" };
                }


                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(jwtSecretKey);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.NameIdentifier, user.LaboratoryID.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);
                return new LoginResult { Success = true, Token = tokenString };
            }
            catch (Exception ex)
            {
                return new LoginResult { Success = true, ErrorMessage = $"Error: {ex.Message}" };
            }

        }

        public async Task<IActionResult> LabRegister(RegistrationDTO registrationDTO)
        {
           try
            {
                var existingUser = await _context.LabDetails.FirstOrDefaultAsync(x => x.LaboratoryEmail == registrationDTO.LabDetails.LaboratoryEmail);

                LabDetails lab = new LabDetails
                {
                    LaboratoryName = registrationDTO.LabDetails.LaboratoryName,
                    LaboratoryContactNumber = registrationDTO.LabDetails.LaboratoryContactNumber,
                    LaboratoryEmail = registrationDTO.LabDetails.LaboratoryEmail,
                    LaboratoryAddress = registrationDTO.LabDetails.LaboratoryAddress,
                };
                _context.LabDetails.Add(lab);
                await _context.SaveChangesAsync();

                UserModel user = new UserModel
                {
                    FirstName =registrationDTO.UserModel.FirstName,
                    LastName = registrationDTO.UserModel.LastName,
                    Email = registrationDTO.UserModel.Email,
                    PhoneNumber = registrationDTO.UserModel.PhoneNumber,
                    Password = _passwordHasher.HashPassword(null, registrationDTO.UserModel.Password),
                    LaboratoryID = lab.LaboratoryID,
                    user_Role = registrationDTO.UserModel.user_Role,

                };
                _context.User_Details.Add(user);  
                await _context.SaveChangesAsync();
                return new OkResult();
            }
            catch (Exception ex) {
                return new BadRequestObjectResult($"Failed to register user & Lab: {ex.Message}");
            }
        }

    }
}
