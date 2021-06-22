using System;
using Newtonsoft.Json;

namespace Library.Models.Reader
{
    public class ReaderModelDTO
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("secondname")]
        public string SecondName { get; set; }

        [JsonProperty("surname")]
        public string Surname { get; set; }

        [JsonProperty("birthdate")]
        public string BirthDate { get; set; }

        [JsonProperty("occupation")]
        public string Occupation { get; set; }

        [JsonProperty("workplace")]
        public string WorkPlace { get; set; }

        [JsonProperty("phonenumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("passport")]
        public string Passport { get; set; }

        #region -- Conversions --

        public static implicit operator ReaderModelDTO(ReaderModel reader)
        {
            return new()
            {
                Id = reader.Id,
                Name = reader.Name,
                SecondName = reader.SecondName,
                Surname = reader.Surname,
                BirthDate = reader.BirthDate is null ? null : Convert.ToString(reader.BirthDate),
                Occupation = reader.Occupation,
                WorkPlace = reader.WorkPlace,
                PhoneNumber = reader.PhoneNumber,
                Passport = reader.Passport,
            };
        }

        public static implicit operator ReaderModel(ReaderModelDTO reader)
        {
            DateTime? birthDate = null;

            if (reader.BirthDate is not null)
            {
                if (DateTime.TryParse(reader.BirthDate, out var dt))
                {
                    birthDate = dt;
                }
            }

            return new()
            {
                Id = reader.Id,
                Name = reader.Name,
                SecondName = reader.SecondName,
                Surname = reader.Surname,
                BirthDate = birthDate,
                Occupation = reader.Occupation,
                WorkPlace = reader.WorkPlace,
                PhoneNumber = reader.PhoneNumber,
                Passport = reader.Passport,
            };
        }

        #endregion
    }
}
