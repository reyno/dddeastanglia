using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DDDEastAnglia.Api.Data.Entities {
    public class Category {

        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        public string Description { get; set; }

        public ICollection<Product> Products { get; set; }

    }
}
