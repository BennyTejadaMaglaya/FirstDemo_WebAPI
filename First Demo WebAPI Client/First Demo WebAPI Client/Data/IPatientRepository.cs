using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using First_Demo_WebAPI_Client.Models;

namespace First_Demo_WebAPI_Client.Data
{
    public interface IPatientRepository
    {
        Task<List<Patient>> GetPatients();
        Task<Patient> GetPatient(int ID);
        Task<List<Patient>> GetPatientsByDoctor(int DoctorID);
    }
}
