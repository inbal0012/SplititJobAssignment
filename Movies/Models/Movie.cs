using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SplititJobAssignment.Movies
{
    public class Movie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Name { get; set; }
        public required int Year { get; set; }
        public required string Length { get; set; }
        public required string MPA { get; set; }
        public required double Rating {get; set; }
        public DateTime CreatedAt {get; set;}
    }
}