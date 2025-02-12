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

        public string FirstName { get; set; } = "";

        public string? MiddleName { get; set; }

        public string LastName { get; set; } = "";

        public Byte[]? RowVersion { get; set; }

        //[JsonIgnore]
        public ICollection<PatientDTO>? Patients { get; set; }
    }
}
