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
    public class PatientRepository : IPatientRepository
    {
        private readonly HttpClient client = new HttpClient();
        public PatientRepository()
        {
            client.BaseAddress = Jeeves.DBUri;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<Patient>> GetPatients()
        {
            HttpResponseMessage response = await client.GetAsync("api/patient");
            if (response.IsSuccessStatusCode)
            {
                List<Patient> Patients = await response.Content.ReadAsAsync<List<Patient>>();
                return Patients;
            }
            else
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        public async Task<List<Patient>> GetPatientsByDoctor(int DoctorID)
        {
            HttpResponseMessage response = await client.GetAsync($"api/patient/byDoctor/{DoctorID}");
            if (response.IsSuccessStatusCode)
            {
                List<Patient> Patients = await response.Content.ReadAsAsync<List<Patient>>();
                return Patients;
            }
            else
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new Exception("Cannot find any Patients for that Doctor.");
                }
                else
                {
                    var ex = Jeeves.CreateApiException(response);
                    throw ex;
                }
            }
        }

        public async Task<Patient> GetPatient(int ID)
        {
            HttpResponseMessage response = await client.GetAsync($"api/patient/{ID}");
            if (response.IsSuccessStatusCode)
            {
                Patient Patient = await response.Content.ReadAsAsync<Patient>();
                return Patient;
            }
            else
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        public async Task AddPatient(Patient patientToAdd)
        {
            var response = await client.PostAsJsonAsync("/api/patient", patientToAdd);
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        public async Task UpdatePatient(Patient patientToUpdate)
        {
            var response = await client.PutAsJsonAsync($"/api/patient/{patientToUpdate.ID}", patientToUpdate);
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        public async Task DeletePatient(Patient patientToDelete)
        {
            var response = await client.DeleteAsync($"/api/patient/{patientToDelete.ID}");
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }
    }
}
