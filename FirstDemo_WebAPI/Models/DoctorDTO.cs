using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

namespace FirstDemo_WebAPI.Models
{
    [ModelMetadataType(typeof(DoctorMetaData))]
    public class DoctorDTO
    {
        public int ID { get; set; }

        //[JsonIgnore]
        public string Summary
        {
            get
            {
                return "Dr. " + FirstName
                    + (string.IsNullOrEmpty(MiddleName) ? " " :
                        (" " + (char?)MiddleName[0] + ". ").ToUpper())
                    + LastName;
            }
        }

        //[JsonIgnore]
        public string FormalName
        {
            get
            {
                return LastName + ", " + FirstName
                    + (string.IsNullOrEmpty(MiddleName) ? "" :
                        (" " + (char?)MiddleName[0] + ".").ToUpper());
            }
        }

        public string FirstName { get; set; } = "";

        public string? MiddleName { get; set; }

        public string LastName { get; set; } = "";

        public Byte[]? RowVersion { get; set; }

        //[JsonIgnore]
        public ICollection<Patient> Patients { get; set; } = new HashSet<Patient>();
    }
}
