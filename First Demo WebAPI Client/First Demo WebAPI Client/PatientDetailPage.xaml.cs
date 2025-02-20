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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace First_Demo_WebAPI_Client
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PatientDetailPage : Page
    {
        Patient view;
        IDoctorRepository doctorRepository;
        IPatientRepository patientRepository;
        bool InsertMode;

        public PatientDetailPage()
        {
            this.InitializeComponent();
            doctorRepository = new DoctorRepository();
            patientRepository = new PatientRepository();
            fillDropDown();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            view = (Patient)e.Parameter;

            if (view.ID == 0) //Inserting
            {
                //Disable the delete button if adding
                btnDelete.IsEnabled = false;
                InsertMode = true;
            }
            else
            {
                InsertMode = false;
            }
        }
        private async void fillDropDown()
        {
            try
            {
                List<Doctor> doctors = await doctorRepository.GetDoctors();
                //Bind to the ComboBox
                DoctorCombo.ItemsSource = doctors.OrderBy(d => d.FormalName);
                //Now you can assign the DataContext for the page
                this.DataContext = view;
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
        }
        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (view.DoctorID == 0)
                {
                    Jeeves.ShowMessage("Error", "You must select the Primary Care Physician");
                }
                else
                {
                    if (InsertMode)
                    {
                        await patientRepository.AddPatient(view);
                    }
                    else
                    {
                        await patientRepository.UpdatePatient(view);
                    }
                    Frame.GoBack();
                }
            }
            catch (AggregateException aex)
            {
                string errMsg = "Errors:" + Environment.NewLine;
                foreach (var exception in aex.InnerExceptions)
                {
                    errMsg += Environment.NewLine + exception.Message;
                }
                Jeeves.ShowMessage("One or more exceptions has occurred:", errMsg);
            }
            catch (ApiException apiEx)
            {
                string errMsg = "Errors:" + Environment.NewLine;
                foreach (var error in apiEx.Errors)
                {
                    errMsg += Environment.NewLine + "-" + error;
                }
                Jeeves.ShowMessage("Problem Saving the Record:", errMsg);
            }
            catch (Exception ex)
            {
                if (ex.GetBaseException().Message.Contains("connection with the server"))
                {
                    Jeeves.ShowMessage("Error", "No connection with the server.");
                }
                else
                {
                    Jeeves.ShowMessage("Error", "Could not complete operation.");
                }
            }
        }
        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            string strTitle = "Confirm Delete";
            string strMsg = "Are you certain that you want to delete " + view.Summary + "?";
            ContentDialogResult result = await Jeeves.ConfirmDialog(strTitle, strMsg);
            if (result == ContentDialogResult.Secondary)
            {
                try
                {
                    await patientRepository.DeletePatient(view);
                    Frame.GoBack();
                }
                catch (AggregateException aex)
                {
                    string errMsg = "Errors:" + Environment.NewLine;
                    foreach (var exception in aex.InnerExceptions)
                    {
                        errMsg += Environment.NewLine + exception.Message;
                    }
                    Jeeves.ShowMessage("One or more exceptions has occurred:", errMsg);
                }
                catch (ApiException apiEx)
                {
                    string errMsg = "Errors:" + Environment.NewLine;
                    foreach (var error in apiEx.Errors)
                    {
                        errMsg += Environment.NewLine + "-" + error;
                    }
                    Jeeves.ShowMessage("Problem Deleting the Record:", errMsg);
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
            }
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
