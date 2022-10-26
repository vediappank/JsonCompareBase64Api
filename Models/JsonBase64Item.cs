using JSONCompareApi.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace JSONCompareApi.Models
{
    public class JsonBase64Item
    {
        [Required]
        public string Id { get; set; }

        public string Position { get; set; }

        [Required]
        public string Data { get; set; }
    }
}
