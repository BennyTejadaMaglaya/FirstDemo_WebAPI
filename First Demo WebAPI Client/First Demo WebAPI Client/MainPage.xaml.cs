﻿using System;
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
            FillDropDown();
        }

        private async void FillDropDown()
        {
            //Show Progress
            progRing.IsActive = true;
            progRing.Visibility = Visibility.Visible;

            try
            {
                List<Doctor> doctors = await doctorRepository.GetDoctors();
                //Add the All Option
                doctors.Insert(0, new Doctor { ID = 0, LastName = " - All Doctors" });
                //Bind to the ComboBox
                DoctorCombo.ItemsSource = doctors.OrderBy(d => d.FormalName);
                btnAdd.IsEnabled = true;
                ShowPatients(null);
            }
            catch (ApiException apiEx)
            {
                string errMsg = "Errors:" + Environment.NewLine;
                foreach (var error in apiEx.Errors)
                {
                    errMsg += Environment.NewLine + "-" + error;
                }
                Jeeves.ShowMessage("Problem filling Doctor Selection:", errMsg);
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

        private async void ShowPatients(int? DoctorID)
        {
            //Show Progress
            progRing.IsActive = true;
            progRing.Visibility = Visibility.Visible;

            try
            {
                List<Patient> patients;
                if (DoctorID.GetValueOrDefault() > 0)
                {
                    patients = await patientRepository.GetPatientsByDoctor(DoctorID.GetValueOrDefault());
                }
                else
                {
                    patients = await patientRepository.GetPatients();
                }
                patientList.ItemsSource = patients.OrderBy(p => p.LastName).ThenBy(p => p.FirstName);

            }
            catch (ApiException apiEx)
            {
                string errMsg = "Errors:" + Environment.NewLine;
                foreach (var error in apiEx.Errors)
                {
                    errMsg += Environment.NewLine + "-" + error;
                }
                Jeeves.ShowMessage("Problem accessing Patients:", errMsg);
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

        private void patientGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the detail page
            Frame.Navigate(typeof(PatientDetailPage), (Patient)e.ClickedItem);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Patient newPat = new Patient();
            newPat.DOB = DateTime.Now;

            // Navigate to the detail page
            Frame.Navigate(typeof(PatientDetailPage), newPat);
        }

        private void DoctorCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Doctor selDoc = (Doctor)DoctorCombo.SelectedItem;
            ShowPatients(selDoc?.ID);
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            FillDropDown();
        }
    }
}
