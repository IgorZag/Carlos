using Android.App;
using Android.Widget;
using Android.OS;
using ZXing.Mobile;
using Android.Content;
using System;
using System.IO;
using QCSCommon;
using System.Xml.Serialization;

namespace QCSAndroid
{
    [Activity(Label = "QR Code Scanner(QCS)", MainLauncher = true, Icon = "@drawable/finning_16_16")]
    public class MainActivity : Activity
    {
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
                if (arrayAdapter.Count == 0)
                {
                    Toast.MakeText(ApplicationContext, "Nothing to send... ", ToastLength.Long).Show();
                    return;
                }
                var externalPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "qcsdata.csv");

                var sb = new System.Text.StringBuilder();
                for (int i = 0; i < arrayAdapter.Count; i++)
                {
                    sb.Append(arrayAdapter.GetItem(i).ToString());
                }

/*
                using (var streamWriter = new StreamWriter(externalPath, true))
                {
                    streamWriter.WriteLine(sb.ToString());
                }
*/
                using (var streamReader = new StreamReader(externalPath))
                {
                    string content = streamReader.ReadToEnd();
                    System.Diagnostics.Debug.WriteLine(content);
                }



                var email = new Intent(Intent.ActionSend);
                email.PutExtra(Intent.ExtraEmail, new string[]
                {
                    GetString(Resource.String.send_email_to)
                }); 
                email.PutExtra(Android.Content.Intent.ExtraCc, new string[] {
                    GetString(Resource.String.send_email_to_cc)
                });

                email.PutExtra(Intent.ExtraSubject, GetString(Resource.String.send_email_subj));

                var file = new Java.IO.File(externalPath);
                email.PutExtra(Intent.ExtraStream, Android.Net.Uri.Parse("file:///" + externalPath));

                file.SetReadable(true, false);

                email.SetType("plain/text");
                StartActivity(Intent.CreateChooser(email, "Send email..."));

                file.DeleteOnExit();
                arrayAdapter.Clear();

            };

            if (!SettingsSet())
            {
                ShowSettings();
            }
        }
        bool SettingsSet()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ClientInfo));
            var externalPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, GetString(Resource.String.file_settings));
            try
            {
                using (var reader = new StreamReader(externalPath, false))
                {
                    var clientInfo = serializer.Deserialize(reader) as ClientInfo;
                    return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }
        void ShowSettings()
        {
            var activitySettings = new Intent(this, typeof(SettingsActivity));
            StartActivity(activitySettings);
        }
    }
}

