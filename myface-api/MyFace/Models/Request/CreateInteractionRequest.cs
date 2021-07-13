using MyFace.Models.Database;
using System.ComponentModel.DataAnnotations;

namespace MyFace.Models.Request
{
    public class CreateInteractionRequest
    {
        [Required]
        public InteractionType InteractionType { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int PostId { get; set; }
    }
}