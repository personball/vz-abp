using System.ComponentModel.DataAnnotations;

namespace CompanyName.ProjectName.Identity.ExternalAuth.Dto
{
    public class ExternalAuthByCodeInput
    {
        [Required]
        public string? ProviderName { get; set; }
        
        [Required]
        public string? Code { get; set; }
    }
}