using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using QCSCommon;

namespace QCSAndroid
{
    [Activity(Label = "Settings")]
    public class SettingsActivity : Activity
    {
        private ClientInfo clientInfo = new ClientInfo();
        private EditText editFirstName;
        private EditText editLastName;
        private EditText editEmail;
        private EditText editAccountNumber;
        private Spinner spnRegion;
        private Spinner spnAgreementType;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Settings);
            LoadControls();
            SetupSpinners();

            // Create your application here
            ReadData();
        }
        protected override void OnDestroy()
        {
            //https://developer.xamarin.com/guides/android/application_fundamentals/activity_lifecycle/
            //clicked back
            base.OnDestroy();
            SaveData();

        }
        private void LoadControls()
        {
            editFirstName = FindViewById<EditText>(Resource.Id.editFirstName);
            editLastName = FindViewById<EditText>(Resource.Id.editLastName);
            editEmail = FindViewById<EditText>(Resource.Id.editEmail);
            editAccountNumber = FindViewById<EditText>(Resource.Id.editCustomerAccountNumber);
            spnRegion = FindViewById<Spinner>(Resource.Id.spnRegion);
            spnAgreementType = FindViewById<Spinner>(Resource.Id.spnAgreementType);
        }
        private void SetupSpinners()
        {
            var spn = FindViewById<Spinner>(Resource.Id.spnRegion);
            spn.ItemSelected += (object sender, AdapterView.ItemSelectedEventArgs e) =>
            {
                var spinner = (Spinner)sender;
                clientInfo.Region = spinner.GetItemAtPosition(e.Position).ToString();
            };
            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.region_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spn.Adapter = adapter;


            spn = FindViewById<Spinner>(Resource.Id.spnAgreementType);
            spn.ItemSelected += (object sender, AdapterView.ItemSelectedEventArgs e) =>
            {
                var spinner = (Spinner)sender;
                clientInfo.AgreementType = spinner.GetItemAtPosition(e.Position).ToString();
            };
            adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.agrement_type_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spn.Adapter = adapter;

        }
        private void ReadData()
        {
        }
        private void SaveData()
        {
            clientInfo.Installer.FirstName = editFirstName.Text;
            clientInfo.Installer.LastName  = editLastName.Text;
            clientInfo.Installer.Email     = editEmail.Text;
            clientInfo.AccountNumber = editAccountNumber.Text;
        }
    }
}