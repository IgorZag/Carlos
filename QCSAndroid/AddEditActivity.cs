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
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

namespace QCSAndroid
{
    [Activity(Label = "Add")]
    public class AddEditActivity : Activity
    {
        private string _scannedData = string.Empty;
        private InstallattionData installationData = new InstallattionData();


        private EditText editMachineBrand;
        private EditText editMachineModel;
        private EditText editMachineHours;
        private Spinner spnMachineYear;
        private Spinner spnInstallationType;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _scannedData = Intent.GetStringExtra("scannedString");

            SetContentView(Resource.Layout.AddEdit);
            LoadControls();
            SetupSpinners();
            SetButtonAction();
        }
        protected override void OnDestroy()
        {
            //https://developer.xamarin.com/guides/android/application_fundamentals/activity_lifecycle/
            //clicked back
            base.OnDestroy();
        }
        private void LoadControls()
        {
            editMachineBrand = FindViewById<EditText>(Resource.Id.editMachineBrand);
            editMachineModel = FindViewById<EditText>(Resource.Id.editMachineModel);
            editMachineHours = FindViewById<EditText>(Resource.Id.editMachineHours);
            spnMachineYear = FindViewById<Spinner>(Resource.Id.spnMachineYear);
            spnInstallationType = FindViewById<Spinner>(Resource.Id.spnInstallationType);
        }
        private void SetButtonAction()
        {
            var btnSaveAndScan = FindViewById<Button>(Resource.Id.btnSaveAndScan);
            var btnSaveAndClose = FindViewById<Button>(Resource.Id.btnSaveAndClose);

            if (null != btnSaveAndScan)
            {
                btnSaveAndScan.Click += async delegate
                {
                    SaveData();
                    await ScanAgain();
                };
            }
            if (null != btnSaveAndClose)
            {
                btnSaveAndClose.Click += delegate
                {
                    SaveData();
                    base.OnBackPressed();
                };
            }
        }
        private void SetupSpinners()
        {
            var spn = FindViewById<Spinner>(Resource.Id.spnInstallationType);
            spn.ItemSelected += (object sender, AdapterView.ItemSelectedEventArgs e) =>
            {
                var spinner = (Spinner)sender;
            };
            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.installation_type_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spn.Adapter = adapter;

            spn = FindViewById<Spinner>(Resource.Id.spnMachineBrand);
            spn.ItemSelected += (object sender, AdapterView.ItemSelectedEventArgs e) =>
            {
                var spinner = (Spinner)sender;
                //populate field with text
                editMachineBrand.Text = spinner.GetItemAtPosition(e.Position).ToString();
            };
            adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.machine_brand_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spn.Adapter = adapter;

            spn = FindViewById<Spinner>(Resource.Id.spnMachineYear);

            var yearList = new List<string>();
            for (int i = 1950; i <= DateTime.UtcNow.Year; i++)
            {
                yearList.Add(i.ToString());
            }

            adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleSpinnerItem, yearList);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spn.Adapter = adapter;
        }
        private void ReadData()
        {
        }
        private void SaveData()
        {
            List<InstallattionData> data = new List<InstallattionData>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<InstallattionData>));
            var externalPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, GetString(Resource.String.file_data));
            try
            {
                using (var reader = new StreamReader(externalPath, false))
                {
                    data = serializer.Deserialize(reader) as List<InstallattionData>;
                }
            }
            catch (Exception)
            {
            }

            InstallattionData newData = new InstallattionData()
            {
                MachineBrand = editMachineBrand.Text,
                MachineModel = editMachineModel.Text,
                ScannedString = _scannedData,
                MachineHours = editMachineHours.Text,
                MachineYear = spnMachineYear.SelectedItem.ToString(),
                InstallationType = spnInstallationType.SelectedItem.ToString()
            };

            data.Add(newData);

            using (var writer = new StreamWriter(externalPath, false))
            {
                serializer.Serialize(writer, data);
            }
        }
        private async Task  ScanAgain()
        {
            AndroidScan scan = new AndroidScan(base.Parent);
            await scan.StartNewScanAsync();
        }
    }
}