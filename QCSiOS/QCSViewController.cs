using System;

using UIKit;

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
            throw new NotImplementedException();
        }
    }
}

