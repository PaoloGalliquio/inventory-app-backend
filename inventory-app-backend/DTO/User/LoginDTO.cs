namespace inventory_app_backend.DTO.User
{
    public class LoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginResultDTO
    {
        public int IdUser { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int IdUserRole { get; set; }
        public string UserRoleName { get; set; }
        public string Token { get; set; }
    }
}
