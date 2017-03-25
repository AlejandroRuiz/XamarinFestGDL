using Android.App;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using System.IO;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Timers;

namespace App1
{
    [Activity(Label = "Prueba de concepto", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        System.Timers.Timer timer = new System.Timers.Timer();
        Random rnd = new Random();
        Random rndv = new Random();
        TextView txtRev;
        TextView txtVel;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            timer.Interval = 1000;
            timer.Elapsed += OnTimedEvent;
            timer.Enabled = true;
            timer.Start();

            txtRev = FindViewById<TextView>(Resource.Id.txtDatoUno);
            txtVel = FindViewById<TextView>(Resource.Id.txtDatoDos);
            Button buttonCapture = FindViewById<Button>(Resource.Id.btnCapture);
            buttonCapture.Click += ButtonCapture_Click;
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            string rev = string.Concat(rnd.Next(1001, 5000).ToString(), " rpm");
            string vel = string.Concat(rndv.Next(0, 80).ToString(), " km/h");
            RunOnUiThread(() => 
            {
                txtRev.SetText(rev, TextView.BufferType.Normal);
                txtVel.SetText(vel, TextView.BufferType.Normal);
            });
        }

        private async void ButtonCapture_Click(object sender, System.EventArgs e)
        {
            var view = this.Window.DecorView;
            view.DrawingCacheEnabled = true;

            Bitmap bitmap = view.GetDrawingCache(true);
            byte[] bitmapData;

            using (var stream = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Jpeg, 0, stream);
                bitmapData = stream.ToArray();

                var container = GetContainer();
                string identifier = string.Format("{0}.jpg", Guid.NewGuid().ToString());
                var fileBlob = container.GetBlockBlobReference(identifier);

                await fileBlob.UploadFromByteArrayAsync(bitmapData, 0, bitmapData.Length);
                System.Threading.Thread.Sleep(2000);
                Toast.MakeText(this, "Captura hecha", ToastLength.Short).Show();
            }
        }

        static CloudBlobContainer GetContainer()
        {
            var account = CloudStorageAccount.Parse("<Aqui tu cadena de conexion>");
            var client = account.CreateCloudBlobClient();
            return client.GetContainerReference("logs");
        }
    }
}

