using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using First_Demo_WebAPI_Client.Models;

namespace First_Demo_WebAPI_Client.Data
{
    public interface IDoctorRepository
    {
        Task<List<Doctor>> GetDoctors();
        Task<Doctor> GetDoctor(int ID);
    }
}
