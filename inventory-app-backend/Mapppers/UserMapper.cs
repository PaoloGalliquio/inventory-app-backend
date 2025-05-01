using inventory_app_backend.Constants;
using inventory_app_backend.Models;
using inventory_app_backend.ViewModels.User;

namespace inventory_app_backend.Mapppers
{
    public static class UserMapper
    {
        public static UserViewModel MapTopUserViewModel(User user)
        {
            return new UserViewModel
            {
                IdUser = user.IdUser,
                Name = user.Name,
                Email = user.Email,
                IdUserRole = user.IdUserRole,
                UserRoleName = Enum.GetName(typeof(Roles), user.IdUserRole) ?? "-"
            };
        }
    }
}
