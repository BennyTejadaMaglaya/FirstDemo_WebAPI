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
    public class PatientController : ControllerBase
    {
        private readonly MedicalOfficeContext _context;

        public PatientController(MedicalOfficeContext context)
        {
            _context = context;
        }

        // GET: api/Patient
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientDTO>>> GetPatients()
        {
            var patientDTOs = await _context.Patients
                .Include(p => p.Doctor)
                .Select(p => new PatientDTO
                {
                    ID = p.ID,
                    FirstName = p.FirstName,
                    MiddleName = p.MiddleName,
                    LastName = p.LastName,
                    OHIP = p.OHIP,
                    DOB = p.DOB,
                    ExpYrVisits = p.ExpYrVisits,
                    RowVersion = p.RowVersion,
                    DoctorID = p.DoctorID,
                    Doctor = p.Doctor != null ? new DoctorDTO
                    {
                        ID = p.Doctor.ID,
                        FirstName = p.Doctor.FirstName,
                        MiddleName = p.Doctor.MiddleName,
                        LastName = p.Doctor.LastName,
                        RowVersion = p.Doctor.RowVersion,
                        Patients = p.Doctor.Patients.Select(pDoctorPatient => new PatientDTO
                        {
                            ID = pDoctorPatient.ID,
                            FirstName = pDoctorPatient.FirstName,
                            MiddleName = pDoctorPatient.MiddleName,
                            LastName = pDoctorPatient.LastName,
                            OHIP = pDoctorPatient.OHIP,
                            DOB = pDoctorPatient.DOB,
                            ExpYrVisits = pDoctorPatient.ExpYrVisits,
                            RowVersion = pDoctorPatient.RowVersion,
                            DoctorID = pDoctorPatient.DoctorID,
                            Doctor = pDoctorPatient.Doctor /* Stop recursive mapping */
                        }).ToList()
                    } : null
                })
                .ToListAsync();

            if (patientDTOs.Count() > 0)
            {
                return patientDTOs;
            }
            else
            {
                return NotFound(new { message = "Error: No Patient records found in the database." });
            }
        }

        // GET: api/Patient/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatient(int id)
        {
            var patient = await _context.Patients
                .Include(p => p.Doctor)
                .FirstOrDefaultAsync(p => p.ID == id);

            if (patient == null)
            {
                return NotFound();
            }

            return patient;
        }

        // GET: api/PatientsByDoctor
        [HttpGet("ByDoctor/{id}")]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatientsByDoctor(int id)
        {
            return await _context.Patients.Include(e => e.Doctor)
                .Where(e => e.DoctorID == id).ToListAsync();
        }

        // PUT: api/Patient/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPatient(int id, Patient patient)
        {
            if (id != patient.ID)
            {
                return BadRequest();
            }

            _context.Entry(patient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Patient
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Patient>> PostPatient(Patient patient)
        {
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPatient", new { id = patient.ID }, patient);
        }

        // DELETE: api/Patient/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.ID == id);
        }
    }
}
