using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models
{
    [Table("books")]
    public class Book
    {
        [Key]
        [Column("book_id")]
        public int BookId { get; set; }

        [Column("title")]
        public string Title { get; set; }

        [Column("author")]
        public string Author { get; set; }

        [Column("category")]
        public string Category { get; set; }

        [Column("total_copies")]
        public int TotalCopies { get; set; }

        [Column("available_copies")]
        public int AvailableCopies { get; set; }
    }
}
