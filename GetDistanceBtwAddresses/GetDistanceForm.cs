using System;
using System.Windows.Forms;

namespace GetDistanceBetweenAddresses
{
#pragma warning disable CA1305 // Specify IFormatProvider

    public partial class GetDistanceForm : Form
    {
        public GetDistanceForm()
        {
            InitializeComponent();
        }

        static public GeocodeAddress GeocodeAddressFrom = new GeocodeAddress("12 r du capitaine guynemer, 92");
        static public GeocodeAddress GeocodeAddressTo = new GeocodeAddress("5 place des trois freres lebeuf");

        GeocodeAddressSettings settings = new GeocodeAddressSettings();
        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = GeocodeAddressFrom.Address;
            textBox2.Text = GeocodeAddressTo.Address;
            ButtonCheck_Click(null, null);
        }

        private void ButtonCheck_Click(object sender, EventArgs e)
        {
            GeocodeAddressFrom.Address = textBox1.Text;
            GeocodeAddressTo.Address = textBox2.Text;

            double distanceBetween = GeocodeAddressFrom.GetDistanceInMeter(GeocodeAddressTo);

            textBox3.Text = GeocodeAddressFrom.FixedAdress;
            textBox5.Text = GeocodeAddressTo.FixedAdress;
            textBoxResult.Text = "From: " + GeocodeAddressFrom.FixedAdress;
            textBoxResult.Text += Environment.NewLine + "To: " + GeocodeAddressTo.FixedAdress;
            textBoxResult.Text += Environment.NewLine + "Latitude= " + GeocodeAddressFrom.Latitude.ToString() + "; longitude=" + GeocodeAddressFrom.Longitude.ToString();
            textBoxResult.Text += Environment.NewLine + "Distance= " + distanceBetween + " m";
            textBoxResult.Text += Environment.NewLine + "Within 500m? " + GeocodeAddressFrom.IsWithinDistanceInMeter(GeocodeAddressTo, settings.MaxFlyingDeliveryDistanceInMeter);
            textBoxResult.Text += Environment.NewLine + "Is From: address valid? " + GeocodeAddressFrom.IsAddressValid();
            textBoxResult.Text += Environment.NewLine + "Is To: address valid? " + GeocodeAddressTo.IsAddressValid();
        }
    }
}
