using System;

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

        partial void btnScanClick(UIButton sender)
        {
            throw new NotImplementedException();
        }

        partial void btnSendClick(UIButton sender)
        {
            var scanner = new MobileBarcodeScanner();
            var result = scanner.Scan().Result;

            if (result != null)
            {
               // arrayAdapter.Add(result.Text.Replace('\n', ' ').Replace('\r', ','));
            }
        }

        partial void BtnScan_TouchUpInside(UIButton sender)
        {
            throw new NotImplementedException();
        }
    }
}

