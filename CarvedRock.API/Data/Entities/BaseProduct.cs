using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarvedRock.Api.Data.Entities;

public class BaseProduct : IProduct
{
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Required]
    public int Id { get; set; }
    [StringLength(100)]
    [Required]
    public string Name { get; set; }
}