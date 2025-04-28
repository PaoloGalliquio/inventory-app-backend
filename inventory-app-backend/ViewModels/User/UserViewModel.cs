namespace inventory_app_backend.ViewModels.User
{
    public class UserViewModel
    {
        public int IdUser { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int IdUserRole { get; set; }
        public string UserRoleName { get; set; }
        public string Token { get; set; }
    }
}
