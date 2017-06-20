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
using System.Threading.Tasks;
using ZXing.Mobile;

namespace QCSAndroid
{
    public class Scan
    {
        Context _context;
        MobileBarcodeScanningOptions _options;
        CameraResolution HandleCameraResolutionSelectorDelegate(List<CameraResolution> availableResolutions)
        {
            //Don't know if this will ever be null or empty
            if (availableResolutions == null || availableResolutions.Count < 1)
                return new CameraResolution() { Width = 800, Height = 600 };

            //Debugging revealed that the last element in the list
            //expresses the highest resolution. This could probably be more thorough.
            return availableResolutions[availableResolutions.Count - 1];
        }
        public Scan(Context parent)
        {
            _context = parent;
            _options = new MobileBarcodeScanningOptions
            {
                CameraResolutionSelector = HandleCameraResolutionSelectorDelegate
            };
        }
        public async Task StartNewScan()
        {
            var scanner = new MobileBarcodeScanner();

            scanner.AutoFocus();

            var result = await scanner.Scan(_options);
            string scannedString = string.Empty;

            if (result != null)
            {
                scannedString = result.Text.Replace('\n', ' ').Replace('\r', ',');
                OpenAddActivity(scannedString);
            }
        }
        private void OpenAddActivity(string scannedString)
        {
            if(null != _context)
            {
                var activityAddEdit = new Intent(_context, typeof(AddEditActivity));
                activityAddEdit.PutExtra("scannedString", scannedString);
                _context.StartActivity(activityAddEdit);

            }
        }
    }
}