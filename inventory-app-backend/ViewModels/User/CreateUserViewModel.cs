namespace inventory_app_backend.ViewModels.User
{
    public class CreateUserViewModel
    {
        public int IdUser { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int IdStatus { get; set; }
        public int IdUserRole { get; set; }
    }
}
