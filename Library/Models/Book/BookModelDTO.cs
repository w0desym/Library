using System;
using Newtonsoft.Json;

namespace Library.Models.Book
{
    public class BookModelDTO
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("readerid")]
        public int? ReaderId { get; set; }

        [JsonProperty("freedate")]
        public string FreeDate { get; set; }

        [JsonProperty("eliminationdate")]
        public string EliminationDate { get; set; }

        #region -- Conversions --

        public static implicit operator BookModelDTO(BookModel book)
        {
            return new()
            {
                Id = book.Id,
                Name = book.Name,
                Author = book.Author,
                Category = book.Category,
                ReaderId = book.ReaderId,
                FreeDate = book.FreeDate is null ? null : Convert.ToString(book.FreeDate),
                EliminationDate = book.EliminationDate is null ? null : Convert.ToString(book.FreeDate)
            };
        }

        public static implicit operator BookModel(BookModelDTO book)
        {
            DateTime? freeDate = null;

            if (book.FreeDate is not null)
            {
                if (DateTime.TryParse(book.FreeDate, out var dt))
                {
                    freeDate = dt;
                }
            }

            DateTime? eliminationDate = null;

            if (book.EliminationDate is not null)
            {
                if (DateTime.TryParse(book.EliminationDate, out var dt))
                {
                    eliminationDate = dt;
                }
            }

            return new()
            {
                Id = book.Id,
                Name = book.Name,
                Author = book.Author,
                Category = book.Category,
                ReaderId = book.ReaderId,
                FreeDate = freeDate,
                EliminationDate = eliminationDate,
            };
        }

        #endregion
    }
}
