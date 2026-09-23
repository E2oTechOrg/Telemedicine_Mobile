using Android.Webkit;
using System.IO;
using System.Text;
using AWebView = Android.Webkit.WebView;

namespace telemedicine.Platforms.Android
{
    public class CustomWebViewClient : WebViewClient
    {
        const string WakeLockStubJs = @"
            'use strict';

            function isWakeLockSupported() {
                return false;
            }

            async function requestWakeLock() {
                return null;
            }

            async function releaseWakeLock() {
                return;
            }

            function applyKeepAwake(enabled) {
                return;
            }
        ";

        const string BlockShareRoomJs = @"
            (function() {
                function tryRemove() {
                    var title = document.getElementById('swal2-title');
                    if (title && title.textContent && title.textContent.indexOf('Share the room') !== -1) {
                        var container = document.querySelector('.swal2-container');
                        if (container) {
                            container.remove();
                            return true;
                        }
                    }
                    return false;
                }

                if (tryRemove()) return;

                var interval = setInterval(function() {
                    if (tryRemove()) {
                        clearInterval(interval);
                    }
                }, 100);

                setTimeout(function() { clearInterval(interval); }, 15000);
            })();
        ";

        public override WebResourceResponse? ShouldInterceptRequest(AWebView? view, IWebResourceRequest? request)
        {
            var url = request?.Url?.ToString() ?? "";

            if (url.Contains("wakeLock.js"))
            {
                var bytes = Encoding.UTF8.GetBytes(WakeLockStubJs);
                var stream = new MemoryStream(bytes);
                return new WebResourceResponse("application/javascript", "UTF-8", stream);
            }

            return base.ShouldInterceptRequest(view, request);
        }

        public override void OnPageFinished(AWebView? view, string? url)
        {
            base.OnPageFinished(view, url);
            view?.EvaluateJavascript(BlockShareRoomJs, null);
        }
    }
}