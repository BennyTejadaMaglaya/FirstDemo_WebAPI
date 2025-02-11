using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

namespace FirstDemo_WebAPI.Models
{
    [ModelMetadataType(typeof(PatientMetaData))]
    public class Patient : IValidatableObject
    {
        public int ID { get; set; }

        public string Summary
        {
            get
            {
                return FirstName
                    + (string.IsNullOrEmpty(MiddleName) ? " " :
                        (" " + (char?)MiddleName[0] + ". ").ToUpper())
                    + LastName;
            }
        }

        public string FirstName { get; set; } = "";

        public string? MiddleName { get; set; }

        public string LastName { get; set; } = "";

        public string OHIP { get; set; } = "";

        public DateTime DOB { get; set; }

        public byte ExpYrVisits { get; set; } = 2;//Most common value is 2.

        public Byte[]? RowVersion { get; set; }

        public int DoctorID { get; set; }
        public Doctor? Doctor { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DOB > DateTime.Today.AddDays(1))
            {
                yield return new ValidationResult("Date of Birth cannot be in the future.", new[] { "DOB" });
            }
        }
    }
}
