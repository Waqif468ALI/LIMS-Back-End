using System.ComponentModel.DataAnnotations;

namespace LabWorld.Model
{
    public class UserModel
    {
        [Key]
        public int userID { get; set; } 
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
        public int? LaboratoryID { get; set; }
        public string? user_Role { get; set; }

    }
    public class LoginResult
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class LabDetails
    {
        [Key]
        public int LaboratoryID { get; set; }
        public string LaboratoryName { get; set; }
        public string LaboratoryContactNumber { get; set; }
        public string LaboratoryEmail { get; set; }
        public string LaboratoryAddress { get; set; }
    }
    public class RegistrationDTO
    {
        public UserModel UserModel { get; set; }
        public LabDetails LabDetails { get; set; }
    }
}
