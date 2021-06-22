using System;

namespace Library.Models.Book
{
    public class BookModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Author { get; set; }

        public string Category { get; set; }

        public int? ReaderId { get; set; }

        public DateTime? FreeDate { get; set; }

        public DateTime? EliminationDate { get; set; }

        #region -- Conversions --

        public static implicit operator BookBindableModel(BookModel book)
        {
            return new()
            {
                Id = book.Id,
                Name = book.Name,
                Author = book.Author,
                Category = book.Category,
                ReaderId = book.ReaderId,
                FreeDate = book.FreeDate,
                EliminationDate = book.EliminationDate,
            };
        }

        public static implicit operator BookModel(BookBindableModel book)
        {
            return new()
            {
                Id = book.Id,
                Name = book.Name,
                Author = book.Author,
                Category = book.Category,
                ReaderId = book.ReaderId,
                FreeDate = book.FreeDate,
                EliminationDate = book.EliminationDate,
            };
        }

        #endregion
    }
}
