using Android.App;
using Android.Widget;
using Android.OS;
using ZXing.Mobile;
using Android.Content;
using System;
using System.IO;
using QCSCommon;
using System.Xml.Serialization;
using System.Text;
using System.Collections.Generic;

namespace QCSAndroid
{
    [Activity(Label = "QR Code Scanner(QCS)", MainLauncher = true, Icon = "@drawable/finning_16_16")]
    public class MainActivity : Activity
    {
        private ArrayAdapter m_ListData;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            MobileBarcodeScanner.Initialize(Application);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
/*
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            ActionBar.Title = "My Toolbar";
            */
            var btnScan = FindViewById<Button>(Resource.Id.btnScan);
            var btnSend = FindViewById<Button>(Resource.Id.btnSend);
            var btnSettings = FindViewById<Button>(Resource.Id.btnSettings);

            var lstView = FindViewById<ListView>(Resource.Id.lstView);

            var arrayAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1);
            lstView.Adapter = arrayAdapter;

            lstView.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs e)
            {
                Toast.MakeText(this, e.Position.ToString(), ToastLength.Long).Show();
            };

            btnSettings.Click += delegate
            {
                ShowSettings();
            };

            btnScan.Click += async delegate
            {
                Scan scan = new Scan(this);
                await scan.StartNewScan();
            };
            btnSend.Click += delegate
            {
                SendEmail();
            };

            if (!SettingsSet())
            {
                ShowSettings();
            }
        }
        private bool SettingsSet()
        {
            return GetSettings() != null;
        }
        private void ShowSettings()
        {
            var activitySettings = new Intent(this, typeof(SettingsActivity));
            StartActivity(activitySettings);
        }
        private void SendEmail()
        {
            //read settings xml
            //read data xml
            //convert to CSV
            //save csv
            //create email with attachement
            //

            StringBuilder strData = new StringBuilder();

            ReadSettings(strData);
            strData.AppendLine();
            strData.AppendLine();

            ReadData(strData);


            var externalPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, GetString(Resource.String.file_data_csv));


            
            using (var streamWriter = new StreamWriter(externalPath, true))
            {
                streamWriter.WriteLine(strData.ToString());
            }
            
            SendEmail(externalPath);
        }
        private bool ReadSettings(StringBuilder strData)
        {
            bool bRet = false;
            ClientInfo ci = GetSettings();
            if (null != ci)
            {
                strData.AppendLine(GetString(Resource.String.csv_installer) + " " + ci.Installer.FirstName + " " + ci.Installer.LastName);
                strData.AppendLine(GetString(Resource.String.csv_email_address) + " " + ci.Installer.Email);
                strData.AppendLine(GetString(Resource.String.csv_region) + " " + ci.Region);
                strData.AppendLine(GetString(Resource.String.csv_accountNumber) + " " + ClientInfo.AccountPrefix + ci.AccountNumber);
                strData.AppendLine(GetString(Resource.String.csv_agreementType) + " " + ClientInfo.AccountPrefix + ci.AgreementType);
            }
            return bRet;
        }
        private bool ReadData(StringBuilder strData)
        {
            bool bRet = false;

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

            foreach (var item in data)
            {
                strData.Append(item.InstallationType);
                strData.Append(",");
                strData.Append(item.MachineBrand);
                strData.Append(",");
                strData.Append(item.MachineModel);
                strData.Append(",");
                strData.Append(item.MachineYear);
                strData.Append(",");
                strData.Append(item.ScannedString);
                strData.AppendLine();
            }

            DeleteFile(GetString(Resource.String.file_data));

            return bRet;
        }
        private ClientInfo GetSettings()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ClientInfo));
            var externalPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, GetString(Resource.String.file_settings));
            try
            {
                using (var reader = new StreamReader(externalPath, false))
                {
                    var clientInfo = serializer.Deserialize(reader) as ClientInfo;
                    return clientInfo;
                }
            }
            catch (Exception)
            {
            }
            return null;

        }
        private void SendEmail(string pathToCsvFile)
        {
            var email = new Intent(Intent.ActionSend);
            email.PutExtra(Intent.ExtraEmail, new string[]
            {
                    GetString(Resource.String.send_email_to)
            });
            email.PutExtra(Android.Content.Intent.ExtraCc, new string[] {
                    GetString(Resource.String.send_email_to_cc)
                });

            email.PutExtra(Intent.ExtraSubject, GetString(Resource.String.send_email_subj));

            var file = new Java.IO.File(pathToCsvFile);
            email.PutExtra(Intent.ExtraStream, Android.Net.Uri.Parse("file:///" + pathToCsvFile));

            file.SetReadable(true, false);

            email.SetType("plain/text");
            StartActivity(Intent.CreateChooser(email, "Send email..."));
        }
    }
}

