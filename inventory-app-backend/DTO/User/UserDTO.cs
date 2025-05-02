namespace inventory_app_backend.DTO.User
{
    public class UserDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int IdRole { get; set; }
    }

    public class UpdateUserDTO : UserDTO
    {
        public int IdUser { get; set; }
    }

    public class GetUserDTO
    {
        public int IdUser { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int IdRole { get; set; }
        public string RoleName { get; set; }
    }
}
