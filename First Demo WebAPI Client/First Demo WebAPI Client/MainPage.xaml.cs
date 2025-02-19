using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using First_Demo_WebAPI_Client.Data;
using First_Demo_WebAPI_Client.Models;
using First_Demo_WebAPI_Client.Utilities;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace First_Demo_WebAPI_Client
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly IDoctorRepository doctorRepository;
        private readonly IPatientRepository patientRepository;

        public MainPage()
        {
            InitializeComponent();
            doctorRepository = new DoctorRepository();
            patientRepository = new PatientRepository();
            ShowPatients();
        }

        private async void ShowPatients()
        {
            //Show Progress
            progRing.IsActive = true;
            progRing.Visibility = Visibility.Visible;

            try
            {
                List<Patient> patients;
                patients = await patientRepository.GetPatients();
                patientList.ItemsSource = patients;

            }
            catch (Exception ex)
            {
                if (ex.GetBaseException().Message.Contains("connection with the server"))
                {
                    Jeeves.ShowMessage("Error", "No connection with the server.");
                }
                else
                {
                    Jeeves.ShowMessage("Error", "Could not complete operation");
                }
            }
            finally
            {
                progRing.IsActive = false;
                progRing.Visibility = Visibility.Collapsed;
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            ShowPatients();
        }
    }
}
