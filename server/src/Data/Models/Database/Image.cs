using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlowerBot.src.Data.Models.Database
{
    public class Image
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ImageId { get; set; }
        public string? Name { get; set; }
        public string Url { get; set; }

        public Guid? ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
