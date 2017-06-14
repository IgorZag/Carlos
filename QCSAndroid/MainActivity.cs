using Android.App;
using Android.Widget;
using Android.OS;
using ZXing.Mobile;
using Android.Content;
using System.IO;

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

            var btnScan = FindViewById<Button>(Resource.Id.btnScan);
            var btnSend = FindViewById<Button>(Resource.Id.btnSend);

            var lstView = FindViewById<ListView>(Resource.Id.lstView);

            var arrayAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1);
            lstView.Adapter = arrayAdapter;

            lstView.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs e)
            {
                Toast.MakeText(this, e.Position.ToString(), ToastLength.Long).Show();
            };

            btnScan.Click += async delegate
            {
                var scanner = new MobileBarcodeScanner();
                var result = await scanner.Scan();

                if (result != null)
                {
                    arrayAdapter.Add(result.Text.Replace('\n', ' ').Replace('\r', ','));
                }
            };
            btnSend.Click += delegate
            {
                if (arrayAdapter.Count == 0)
                {
                    Toast.MakeText(ApplicationContext, "Nothing to send... ", ToastLength.Long).Show();
                    return;
                }
                string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                string filename = Path.Combine(path, "qcsdata.csv");

                var sb = new System.Text.StringBuilder();
                for (int i = 0; i < arrayAdapter.Count; i++)
                {
                    sb.Append(arrayAdapter.GetItem(i).ToString());
                }

                using (var streamWriter = new StreamWriter(filename, true))
                {
                    streamWriter.WriteLine(sb.ToString());
                    streamWriter.WriteLine("Hello");
                }

                using (var streamReader = new StreamReader(filename))
                {
                    string content = streamReader.ReadToEnd();
                    System.Diagnostics.Debug.WriteLine(content);
                }



                var email = new Intent(Intent.ActionSend);
                email.PutExtra(Intent.ExtraEmail, new string[]
                {
                   // "carlossantamaria@hotmail.com",
                });
                email.PutExtra(Android.Content.Intent.ExtraCc, new string[] {
                    "igorzag@gmail.com"
                });

                email.PutExtra(Intent.ExtraSubject, "QR Code Scan. " + System.DateTime.Now.ToShortTimeString());

                var file = new Java.IO.File(filename);
                email.PutExtra(Intent.ExtraStream, Android.Net.Uri.FromFile(file));

                email.SetType("message/rfc822");
                StartActivity(email);

                arrayAdapter.Clear();

            };

        }
    }
}

