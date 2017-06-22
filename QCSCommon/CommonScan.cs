using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using ZXing.Mobile;

namespace QCSCommon
{
    public class CommonScan
    {
        MobileBarcodeScanningOptions _options;
        MobileBarcodeScanner _scanner;
        CameraResolution HandleCameraResolutionSelectorDelegate(List<CameraResolution> availableResolutions)
        {
            //Don't know if this will ever be null or empty
            if (availableResolutions == null || availableResolutions.Count < 1)
                return new CameraResolution() { Width = 800, Height = 600 };

            //Debugging revealed that the last element in the list
            //expresses the highest resolution. This could probably be more thorough.
            return availableResolutions[availableResolutions.Count - 1];
        }
        public CommonScan()
        {
            _options = new MobileBarcodeScanningOptions
            {
                CameraResolutionSelector = HandleCameraResolutionSelectorDelegate
            };
            _scanner = new MobileBarcodeScanner();
            _scanner.AutoFocus();

        }
        public async Task<string> StartNewScanAsync()
        {
            var result = await _scanner.Scan(_options);
            string scannedString = string.Empty;

            if (result != null)
            {
                scannedString = result.Text.Replace('\n', ' ').Replace('\r', ',');
            }
            return scannedString;
        }
        public string StartNewScan()
        {
            string result = string.Empty;
            Task.Factory.StartNew(async () =>
            {
                result = await StartNewScanAsync();
            }).Wait();

            return result;
        }
        private string FixString(ZXing.Result scanResult)
        {
            return scanResult?.Text.Replace('\n', ' ').Replace('\r', ',');
        }
    }
}