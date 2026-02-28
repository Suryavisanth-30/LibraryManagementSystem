using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Models
{
    [Table("BorrowRecord")]   // exact table name
    public class BorrowRecord
    {
        [Key]
        [Column("borrow_id")]
        public int BorrowId { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("book_id")]
        public int BookId { get; set; }

        [Column("borrow_date")]
        public DateTime BorrowDate { get; set; }

        [Column("return_date")]
        public DateTime? ReturnDate { get; set; }

        [Column("status")]
        public string Status { get; set; }

        // Navigation properties (NOT DB columns)
        public User User { get; set; }
        public Book Book { get; set; }
    }
}
