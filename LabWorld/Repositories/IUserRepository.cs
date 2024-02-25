using LabWorld.Model;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LabWorld.Repositories
{
    public interface IUserRepository
    {
        Task<IActionResult> Register(UserModel userRegistration);
        Task<LoginResult> Login(UserModel userRegistration);
        Task<IActionResult> LabRegister(RegistrationDTO registrationDTO);

    }
}
