using Foundation;
using MessageUI;
using QCSCommon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UIKit;
using ZXing.Mobile;

namespace QCSiOS
{
    public partial class QCSViewController
    {
        public QCSViewController() 
        {
        }




        partial void btnScanDown(UIButton sender)
        {
            var scan = new CommonScan();
            string scannedString = scan.StartNewScan();
        }
        partial void btnSendDown(UIButton sender)
        {
            SendEmail();
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

            strData.AppendLine("hello");

            ReadSettings(strData);
            strData.AppendLine();
            strData.AppendLine();

            ReadData(strData);


            var externalPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "file_data_csv.csv");



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
                //strData.AppendLine(GetString(Resource.String.csv_installer) + " " + ci.Installer.FirstName + " " + ci.Installer.LastName);
                //strData.AppendLine(GetString(Resource.String.csv_email_address) + " " + ci.Installer.Email);
                //strData.AppendLine(GetString(Resource.String.csv_region) + " " + ci.Region);
                //strData.AppendLine(GetString(Resource.String.csv_accountNumber) + " " + ClientInfo.AccountPrefix + ci.AccountNumber);
                //strData.AppendLine(GetString(Resource.String.csv_agreementType) + " " + ClientInfo.AccountPrefix + ci.AgreementType);
            }
            return bRet;
        }
        private bool ReadData(StringBuilder strData)
        {
            bool bRet = false;

            List<InstallattionData> data = new List<InstallattionData>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<InstallattionData>));
            var externalPath = string.Empty;// Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, GetString(Resource.String.file_data));
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

           // DeleteFile(GetString(Resource.String.file_data));

            return bRet;
        }
        private ClientInfo GetSettings()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ClientInfo));
            var externalPath = string.Empty;// Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, GetString(Resource.String.file_settings));
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
            if (MFMailComposeViewController.CanSendMail)
            {
                MFMailComposeViewController mailController = new MFMailComposeViewController();
                mailController.SetToRecipients(new[] { "igorzag@gmail.com" });
                mailController.SetCcRecipients(new []{ "igorzag@gmail.com" });
                mailController.SetSubject("hfghfghfghfg");
                mailController.SetMessageBody("not html ody", false);
                NSData fileData = NSData.FromFile(pathToCsvFile);
                mailController.AddAttachmentData(fileData, "text/plain", "filedata.csv");


                mailController.Finished += (object s, MFComposeResultEventArgs args) =>
                {
                    Console.WriteLine(args.Result.ToString());
                    args.Controller.DismissViewController(true, null);
                };

                this.PresentViewController(mailController, true, null);
            }
            



            //var email = new Intent(Intent.ActionSend);
            //email.PutExtra(Intent.ExtraEmail, new string[]
            //{
            //        ResourceManager.GetString("send_email_to");
            //});
            //email.PutExtra(Android.Content.Intent.ExtraCc, new string[] {
            //        GetString(Resource.String.send_email_to_cc)
            //    });

            //email.PutExtra(Intent.ExtraSubject, GetString(Resource.String.send_email_subj));

            //var file = new Java.IO.File(pathToCsvFile);
            //email.PutExtra(Intent.ExtraStream, Android.Net.Uri.Parse("file:///" + pathToCsvFile));

            //file.SetReadable(true, false);

            //email.SetType("plain/text");
            //StartActivity(Intent.CreateChooser(email, "Send email..."));
        }
    }
}

