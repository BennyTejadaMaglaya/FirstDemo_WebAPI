using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using First_Demo_WebAPI_Client.Models;
using First_Demo_WebAPI_Client.Utilities;

namespace First_Demo_WebAPI_Client.Data
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly HttpClient client = new HttpClient();

        public DoctorRepository()
        {
            client.BaseAddress = Jeeves.DBUri;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<Doctor>> GetDoctors()
        {
            HttpResponseMessage response = await client.GetAsync("api/doctor");
            if (response.IsSuccessStatusCode)
            {
                List<Doctor> doctors = await response.Content.ReadAsAsync<List<Doctor>>();
                return doctors;
            }
            else
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }
        public async Task<Doctor> GetDoctor(int DoctorID)
        {
            HttpResponseMessage response = await client.GetAsync($"api/doctor/{DoctorID}");
            if (response.IsSuccessStatusCode)
            {
                Doctor doctor = await response.Content.ReadAsAsync<Doctor>();
                return doctor;
            }
            else
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        public async Task AddDoctor(Doctor doctorToAdd)
        {
            var response = await client.PostAsJsonAsync("/api/doctor", doctorToAdd);
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        public async Task UpdateDoctor(Doctor doctorToUpdate)
        {
            var response = await client.PutAsJsonAsync($"/api/doctor/{doctorToUpdate.ID}", doctorToUpdate);
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        public async Task DeleteDoctor(Doctor doctorToDelete)
        {
            var response = await client.DeleteAsync($"/api/doctor/{doctorToDelete.ID}");
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }
    }
}
