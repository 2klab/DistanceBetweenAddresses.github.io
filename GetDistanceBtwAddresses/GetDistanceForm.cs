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

        static public GeocodeAddress GeocodeAddress1 = new GeocodeAddress("57 avenue de l'arche, 92400 Courbevoie, France");
        static public GeocodeAddress GeocodeAddress2 = new GeocodeAddress("14 avenue leonard de vinci, 92400 Courbevoie, France");

        GeocodeAddressSettings settings = new GeocodeAddressSettings();
        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = GeocodeAddress1.Address;
            textBox2.Text = GeocodeAddress2.Address;
            ButtonCheck_Click(null, null);
        }

        private void ButtonCheck_Click(object sender, EventArgs e)
        {
            GeocodeAddress1.Address = textBox1.Text;
            GeocodeAddress2.Address = textBox2.Text;

            double distanceBetween = GeocodeAddress1.GetDistanceInMeter(GeocodeAddress2);

            textBoxResult.Text = "";
            textBoxResult.Text += "From: " + GeocodeAddress1.FixedAdress;
            textBoxResult.Text += " To: " + GeocodeAddress2.FixedAdress;
            textBoxResult.Text += " lat= " + GeocodeAddress1.Latitude.ToString() + "; lon=" + GeocodeAddress1.Longitude.ToString();
            textBoxResult.Text += ". distance= " + distanceBetween + " m";
            textBoxResult.Text += ". Within 500m? " + GeocodeAddress1.IsWithinDistanceInMeter(GeocodeAddress2, settings.MaxDeliveryDistanceInMeter);
            textBoxResult.Text += ". Is the address valid? " + GeocodeAddress1.IsAddressValid();
        }
    }
}
