// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace QCSiOS
{
    [Register ("QCSViewController")]
    partial class QCSViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnScan { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton btnSend { get; set; }

        [Action ("btnScanClick:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void btnScanClick (UIKit.UIButton sender);

        [Action ("btnSendClick:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void btnSendClick (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (btnScan != null) {
                btnScan.Dispose ();
                btnScan = null;
            }

            if (btnSend != null) {
                btnSend.Dispose ();
                btnSend = null;
            }
        }
    }
}