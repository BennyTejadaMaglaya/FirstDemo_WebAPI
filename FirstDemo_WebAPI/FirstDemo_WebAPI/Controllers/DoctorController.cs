using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FirstDemo_WebAPI.Data;
using FirstDemo_WebAPI.Models;

namespace FirstDemo_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly MedicalOfficeContext _context;

        public DoctorController(MedicalOfficeContext context)
        {
            _context = context;
        }

        // GET: api/Doctor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoctorDTO>>> GetDoctors()
        {
            var doctorDTOs = await _context.Doctors
                .Select(d => new DoctorDTO
                {
                    ID = d.ID,
                    FirstName = d.FirstName,
                    MiddleName = d.MiddleName,
                    LastName = d.LastName,
                    RowVersion = d.RowVersion
                })
                .ToListAsync();

            if (doctorDTOs.Count() > 0)
            {
                return doctorDTOs;
            }
            else
            {
                return NotFound(new { message = "Error: No Doctor records found in the database." });
            }
        }

        // GET: api/Doctor/inc - Include the Patients Collection
        [HttpGet("inc")]
        public async Task<ActionResult<IEnumerable<DoctorDTO>>> GetDoctorsInc()
        {
            var doctorDTOs = await _context.Doctors
                .Include(d => d.Patients)
                .Select(d => new DoctorDTO
                {
                    ID = d.ID,
                    FirstName = d.FirstName,
                    MiddleName = d.MiddleName,
                    LastName = d.LastName,
                    RowVersion = d.RowVersion,
                    Patients = d.Patients.Select(dPatient => new PatientDTO
                    {
                        ID = dPatient.ID,
                        FirstName = dPatient.FirstName,
                        MiddleName = dPatient.MiddleName,
                        LastName = dPatient.LastName,
                        OHIP = dPatient.OHIP,
                        DOB = dPatient.DOB,
                        ExpYrVisits = dPatient.ExpYrVisits
                    }).ToList()
                })
                .ToListAsync();

            if (doctorDTOs.Count() > 0)
            {
                return doctorDTOs;
            }
            else
            {
                return NotFound(new { message = "Error: No Doctor records found in the database." });
            }
        }


        // GET: api/Doctor/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorDTO>> GetDoctor(int id)
        {
            var doctorDTO = await _context.Doctors
                .Select(d => new DoctorDTO
                {
                    ID = d.ID,
                    FirstName = d.FirstName,
                    MiddleName = d.MiddleName,
                    LastName = d.LastName,
                    RowVersion = d.RowVersion
                })
                .FirstOrDefaultAsync(d => d.ID == id);

            if (doctorDTO == null)
            {
                return NotFound(new { message = "Error: That Doctor was not found in the database." });
            }

            return doctorDTO;
        }

        // GET: api/Doctor/inc/5
        [HttpGet("inc/{id}")]
        public async Task<ActionResult<DoctorDTO>> GetDoctorInc(int id)
        {
            var doctorDTO = await _context.Doctors
                .Select(d => new DoctorDTO
                {
                    ID = d.ID,
                    FirstName = d.FirstName,
                    MiddleName = d.MiddleName,
                    LastName = d.LastName,
                    RowVersion = d.RowVersion,
                    Patients = d.Patients.Select(dPatient => new PatientDTO
                    {
                        ID = dPatient.ID,
                        FirstName = dPatient.FirstName,
                        MiddleName = dPatient.MiddleName,
                        LastName = dPatient.LastName,
                        OHIP = dPatient.OHIP,
                        DOB = dPatient.DOB,
                        ExpYrVisits = dPatient.ExpYrVisits
                    }).ToList()
                })
                .FirstOrDefaultAsync(d => d.ID == id);

            if (doctorDTO == null)
            {
                return NotFound(new { message = "Error: That Doctor was not found in the database." });
            }

            return doctorDTO;
        }

        private bool DoctorExists(int id)
        {
            return _context.Doctors.Any(e => e.ID == id);
        }
    }
}
