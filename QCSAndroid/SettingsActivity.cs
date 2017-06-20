using System;
using Android.App;
using Android.OS;
using Android.Widget;
using QCSCommon;
using System.Xml.Serialization;
using System.IO;

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
        private ArrayAdapter arrRegion;
        private ArrayAdapter arrAgreementTypes;

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
        }
        private void LoadControls()
        {
            editFirstName = FindViewById<EditText>(Resource.Id.editFirstName);
            editLastName = FindViewById<EditText>(Resource.Id.editLastName);
            editEmail = FindViewById<EditText>(Resource.Id.editEmail);
            editAccountNumber = FindViewById<EditText>(Resource.Id.editCustomerAccountNumber);
            spnRegion = FindViewById<Spinner>(Resource.Id.spnRegion);
            spnAgreementType = FindViewById<Spinner>(Resource.Id.spnAgreementType);

            var btnSaveSettings = FindViewById<Button>(Resource.Id.btnSaveSettings);
            btnSaveSettings.Click += delegate
            {
                SaveData();
                base.OnBackPressed();
            };
        }
        private void SetupSpinners()
        {
            spnRegion.ItemSelected += (object sender, AdapterView.ItemSelectedEventArgs e) =>
            {
                var spinner = (Spinner)sender;
                clientInfo.Region = spinner.GetItemAtPosition(e.Position).ToString();
            };
            arrRegion = ArrayAdapter.CreateFromResource(this, Resource.Array.region_array, Android.Resource.Layout.SimpleSpinnerItem);
            arrRegion.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spnRegion.Adapter = arrRegion;


            spnAgreementType.ItemSelected += (object sender, AdapterView.ItemSelectedEventArgs e) =>
            {
                var spinner = (Spinner)sender;
                clientInfo.AgreementType = spinner.GetItemAtPosition(e.Position).ToString();
            };
            arrAgreementTypes = ArrayAdapter.CreateFromResource(this, Resource.Array.agrement_type_array, Android.Resource.Layout.SimpleSpinnerItem);
            arrAgreementTypes.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spnAgreementType.Adapter = arrAgreementTypes;

        }
        private void ReadData()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ClientInfo));
            var externalPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, GetString(Resource.String.file_settings));
            try
            {
                using (var reader = new StreamReader(externalPath, false))
                {
                    clientInfo = serializer.Deserialize(reader) as ClientInfo;

                    editFirstName.Text = clientInfo.Installer.FirstName;
                    editLastName.Text = clientInfo.Installer.LastName;
                    editEmail.Text = clientInfo.Installer.Email;
                    editAccountNumber.Text = clientInfo.AccountNumber;

                    for (int i = 0; i < spnRegion.Adapter.Count; i++)
                    {
                        if (spnRegion.Adapter.GetItem(i).ToString() == clientInfo.Region)
                        {
                            spnRegion.SetSelection(i);
                            break;
                        }
                    }
                    for (int i = 0; i < spnAgreementType.Adapter.Count; i++)
                    {
                        if (spnAgreementType.Adapter.GetItem(i).ToString() == clientInfo.AgreementType)
                        {
                            spnAgreementType.SetSelection(i);
                            break;
                        }
                    }
                }
            }
            catch(Exception)
            {
            }

        }
        private void SaveData()
        {
            clientInfo.Installer.FirstName = editFirstName.Text;
            clientInfo.Installer.LastName  = editLastName.Text;
            clientInfo.Installer.Email     = editEmail.Text;
            clientInfo.AccountNumber       = editAccountNumber.Text;
            clientInfo.AgreementType       = spnAgreementType.SelectedItem.ToString();
            clientInfo.Region              = spnRegion.SelectedItem.ToString();

            XmlSerializer serializer = new XmlSerializer(typeof(ClientInfo));
            var externalPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, GetString(Resource.String.file_settings));
            using (var writer = new StreamWriter(externalPath, false))
            {
                serializer.Serialize(writer, clientInfo);
            }
        }
    }
}