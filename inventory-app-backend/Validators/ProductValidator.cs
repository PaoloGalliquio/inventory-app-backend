using inventory_app_backend.DTO;

namespace inventory_app_backend.Validators
{
    public interface IProductValidator
    {
        ValidatorResult RunValidatorForCreate(CreateProductDTO product);
        ValidatorResult RunValidatorForUpdate(CreateProductDTO product);
    }

    public class ProductValidator : IProductValidator
    {
        public ValidatorResult RunValidatorForCreate(CreateProductDTO product)
        {
            var result = ValidatorResult.GetSuccessfulResult();
            if (string.IsNullOrEmpty(product.Name))
            {
                result.AddError("Name", "El nombre es obligatorio");
            }
            if (string.IsNullOrEmpty(product.Description))
            {
                result.AddError("Description", "La descripción es obligatoria");
            }
            if (product.Price <= 0)
            {
                result.AddError("Price", "El precio es obligatorio");
            }
            if (product.Quantity <= 0)
            {
                result.AddError("Quantity", "La cantidad es obligatoria");
            }
            if (product.IdCategory <= 0)
            {
                result.AddError("IdCategory", "La categoría es obligatoria");
            }
            return result;
        }

        public ValidatorResult RunValidatorForUpdate(CreateProductDTO product)
        {
            var result = RunValidatorForCreate(product);
            if (product.IdProduct <= 0)
            {
                result.AddError("IdProduct", "El ID de producto es obligatorio");
            }
            return result;
        }
    }
}
