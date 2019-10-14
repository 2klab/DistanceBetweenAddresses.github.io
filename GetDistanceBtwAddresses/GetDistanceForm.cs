using Nominatim.API.Geocoders;
using Nominatim.API.Models;
using System;
using System.ComponentModel;
using System.Device.Location;
using System.Threading.Tasks;
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

        [Bindable(true)]
        string Address1 { get; set; } = "57 avenue de l'arche, 92400 Courbevoie, France";

        [Bindable(true)]
        string Address2 { get; set; } = "14 avenue leonard de vinci, 92400 Courbevoie, France";

        GeocodeAddressSettings settings = new GeocodeAddressSettings();
        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.DataBindings.Add("Text", Address1, "",true, DataSourceUpdateMode.OnPropertyChanged);
            textBox2.DataBindings.Add("Text", Address2, "", true, DataSourceUpdateMode.OnPropertyChanged);

            Button1_Click(null, null);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            GeocodeAddress ga1 = new GeocodeAddress(Address1);
            GeocodeAddress ga2 = new GeocodeAddress(Address2);

            double distanceBetween = ga1.GetDistanceInMeter(ga2);

            string no1 = ga1.GeocodeResponse.Address.Pedestrian + ga1.GeocodeResponse.Address.Road;
            string no2 = ga2.GeocodeResponse.Address.Pedestrian + ga2.GeocodeResponse.Address.Road;
            textBox4.Text = "";
            textBox4.Text += "From:" + ga1.GeocodeResponse.Address.HouseNumber + " " + no1 + " " + ga1.GeocodeResponse.Address.PostCode + " " + ga1.GeocodeResponse.Address.Town;
            textBox4.Text += " To:" + ga2.GeocodeResponse.Address.HouseNumber + " " + no2 + " " + ga2.GeocodeResponse.Address.PostCode + " " + ga2.GeocodeResponse.Address.Town;
            textBox4.Text += " lat=" + ga1.GeocodeResponse.Latitude.ToString() + "; lon=" + ga1.GeocodeResponse.Longitude.ToString();
            textBox4.Text += ". distance=" + distanceBetween + " m";
            textBox4.Text += ". Within 500m? " + ga1.IsWithinDistanceInMeter(ga2, settings.MaxDeliveryDistanceInMeter);
            textBox4.Text += ". Is address valid? " + ga1.IsAddressValid(Address2);
        }
    }
}
