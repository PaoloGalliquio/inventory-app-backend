using inventory_app_backend.DTO.User;

namespace inventory_app_backend.Validators
{
    public interface ILoginValidator
    {
        ValidatorResult RunValidatorForLogin(LoginDTO login);
    }

    public class LoginValidator : ILoginValidator
    {
        public ValidatorResult RunValidatorForLogin(LoginDTO login)
        {
            var result = ValidatorResult.GetSuccessfulResult();
            result.ClearErrors();
            if (string.IsNullOrEmpty(login.Email))
            {
                result.AddError("Email", "El email es obligatorio");
            }
            if (string.IsNullOrEmpty(login.Password))
            {
                result.AddError("Password", "La contraseña es obligatoria");
            }
            return result;
        }
    }
}
