using Domain;

namespace Application.Dto
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Doctor Doctor { get; set; }
    }
}