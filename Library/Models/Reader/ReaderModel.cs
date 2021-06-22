using System;

namespace Library.Models.Reader
{
    public class ReaderModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string SecondName { get; set; }

        public string Surname { get; set; }

        public DateTime? BirthDate { get; set; }

        public string Occupation { get; set; }

        public string WorkPlace { get; set; }

        public string PhoneNumber { get; set; }

        public string Passport { get; set; }

        #region -- Conversions --

        public static implicit operator ReaderBindableModel(ReaderModel reader)
        {
            return new()
            {
                Id = reader.Id,
                Name = reader.Name,
                SecondName = reader.SecondName,
                Surname = reader.Surname,
                BirthDate = reader.BirthDate,
                Occupation = reader.Occupation,
                WorkPlace = reader.WorkPlace,
                PhoneNumber = reader.PhoneNumber,
                Passport = reader.Passport,
            };
        }

        public static implicit operator ReaderModel(ReaderBindableModel reader)
        {
            return new()
            {
                Id = reader.Id,
                Name = reader.Name,
                SecondName = reader.SecondName,
                Surname = reader.Surname,
                BirthDate = reader.BirthDate,
                Occupation = reader.Occupation,
                WorkPlace = reader.WorkPlace,
                PhoneNumber = reader.PhoneNumber,
                Passport = reader.Passport,
            };
        }

        #endregion
    }
}
