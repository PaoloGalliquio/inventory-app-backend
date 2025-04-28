using inventory_app_backend.Constants;
using inventory_app_backend.Models;
using inventory_app_backend.ViewModels.User;

namespace inventory_app_backend.Mapppers
{
    public static class UserMapper
    {
        public static User MapToUser(LoginUserViewModel viewModel)
        {
            return new User
            {
                Email = viewModel.Email,
                Password = viewModel.Password
            };
        }

        public static User MapCreateUserViewModelToUser(CreateUserViewModel viewModel)
        {
            return new User
            {
                IdUser = viewModel.IdUser,
                Name = viewModel.Name,
                Email = viewModel.Email,
                IdUserRole = viewModel.IdUserRole,
                IdStatus = (int)Status.Active
            };
        }

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
