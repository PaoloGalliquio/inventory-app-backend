using inventory_app_backend.DTO.User;
using System.ComponentModel.DataAnnotations;

namespace inventory_app_backend.Validators
{
    public interface IUserValidator
    {
        ValidatorResult RunValidatorForCreate(UserDTO user);
        ValidatorResult RunValidatorForUpdate(UpdateUserDTO user);
    }

    public class UserValidator : IUserValidator
    {

        public ValidatorResult RunValidatorForCreate(UserDTO user)
        {
            var result = ValidatorResult.GetSuccessfulResult();
            result.ClearErrors();
            if (string.IsNullOrEmpty(user.Name))
            {
                result.AddError("Name", "El nombre es obligatorio");
            }
            if (string.IsNullOrEmpty(user.Email))
            {
                result.AddError("Email", "El correo electrónico es obligatorio");
            }
            else if (!new EmailAddressAttribute().IsValid(user.Email))
            {
                result.AddError("Email", "El correo electrónico no es válido");
            }
            if (string.IsNullOrEmpty(user.Password))
            {
                result.AddError("Password", "La contraseña es obligatoria");
            }
            if (user.IdRole <= 0)
            {
                result.AddError("IdRole", "El rol es obligatorio");
            }
            return result;
        }

        public ValidatorResult RunValidatorForUpdate(UpdateUserDTO user)
        {
            var UserDTO = new UserDTO
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                IdRole = user.IdRole
            };
            var result = RunValidatorForCreate(UserDTO);
            if (user.IdUser <= 0)
            {
                result.AddError("IdUser", "El ID de usuario es obligatorio");
            };
            return result;
        }
    }
}
