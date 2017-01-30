using Android.App;
using Android.Widget;
using Android.OS;
using System;

namespace AndroidClientIndirect
{
    [Activity(Label = "AndroidClientIndirect", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);
            var txtAlpha = FindViewById<TextView>(Resource.Id.txtAlpha);
            var txtNumeric = FindViewById<TextView>(Resource.Id.txtNumeric);

            var svc = new NetStandardClientLib.DemoServiceProxy();

            try { txtAlpha.Text = svc.Reverse("ABCDEF"); }
            catch (Exception ex)
            {
                txtAlpha.Text = ex.ToString();
                Android.Util.Log.Error("Remote call", ex.ToString());
            }

            try { txtNumeric.Text = svc.ReverseAsync("ABCDEF").Result; }
            catch (Exception ex)
            {
                txtNumeric.Text = ex.ToString();
                Android.Util.Log.Error("Remote call", ex.ToString());
            }
        }
    }
}
