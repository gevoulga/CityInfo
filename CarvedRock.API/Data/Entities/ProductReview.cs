using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarvedRock.Api.Data.Entities;

public class ProductReview
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [ForeignKey("ProductId")]
    public int ProductId { get; set; }
    public Product Product { get; set; }
    
    [StringLength(200), Required]
    public string Title { get; set; }
    public string Review { get; set; }
}