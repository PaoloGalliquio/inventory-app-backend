using inventory_app_backend.Constants;
using inventory_app_backend.DTO;

namespace inventory_app_backend.Services
{
    public interface IRoleService
    {
        List<RoleDTO> GetAllRoles();
    }

    public class RoleService : IRoleService
    {
        public List<RoleDTO> GetAllRoles()
        {
            return Enum.GetValues(typeof(Roles))
                       .Cast<Roles>()
                       .Select(r => new RoleDTO
                       {
                           IdRole = (int)r,
                           Name = r.ToString()
                       })
                       .ToList();
        }
    }
}
