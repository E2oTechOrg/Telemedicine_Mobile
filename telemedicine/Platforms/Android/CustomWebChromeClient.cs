using Android.Webkit;

namespace telemedicine.Platforms.Android
{
    public class CustomWebChromeClient : WebChromeClient
    {
        public override void OnPermissionRequest(PermissionRequest request)
        {
            request.Grant(request.GetResources());
        }
    }
}