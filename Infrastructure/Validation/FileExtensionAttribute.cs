using System.ComponentModel.DataAnnotations;

namespace ShopQuanAo.Infrastructure.Validation
{
    public class FileExtensionAttribute:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName);
                string[] extensions = { "jpg", "png", "jpeg","JPG","PNG","JPEG" };
                bool result = extensions.Any(x => extension.EndsWith(x));
                if (!result) {
                    return new ValidationResult("Allowed extension are jpg or png or jpeg");
                }
            }
            return ValidationResult.Success;
        }
    }
}
