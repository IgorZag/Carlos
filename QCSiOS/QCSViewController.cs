using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UIKit;
using ZXing.Mobile;

namespace QCSiOS
{
    public partial class QCSViewController : UIViewController
    {
        public QCSViewController() : base("QCSViewController", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
        CameraResolution HandleCameraResolutionSelectorDelegate(List<CameraResolution> availableResolutions)
        {
            //Don't know if this will ever be null or empty
            if (availableResolutions == null || availableResolutions.Count < 1)
                return new CameraResolution() { Width = 800, Height = 600 };

            //Debugging revealed that the last element in the list
            //expresses the highest resolution. This could probably be more thorough.
            return availableResolutions[availableResolutions.Count - 1];
        }

        partial void btnScanClick(UIButton sender)
        {
            var options = new ZXing.Mobile.MobileBarcodeScanningOptions
            {
                CameraResolutionSelector = HandleCameraResolutionSelectorDelegate
            };
            string text;
            Task.Factory.StartNew(async () => 
            {
                var scanner = new MobileBarcodeScanner(this);
                scanner.AutoFocus();

                var result = await scanner.Scan(options, true);
                text = result.Text;

            }).Wait();


            //string s = result.Text;

        }

        partial void btnSendClick(UIButton sender)
        {

        }


    }
}

