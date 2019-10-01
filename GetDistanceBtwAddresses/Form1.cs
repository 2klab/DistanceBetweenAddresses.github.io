using Nominatim.API.Geocoders;
using Nominatim.API.Models;
using System;
using System.Device.Location;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GetDistanceBtwAddresses
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {

            string address1 = "57 avenue de l'arche, 92400 Courbevoie, France";
            GeocodeAddress ga1 = new GeocodeAddress(address1);

            string address2 = "14 avenue leonard de vinci, 92400 Courbevoie, France";
            GeocodeAddress ga2 = new GeocodeAddress(address2);

            double distanceBetween = ga1.GetDistanceInMeter(ga2);

            string no1 = ga1.gr.Address.Pedestrian + ga1.gr.Address.Road;
            string no2 = ga2.gr.Address.Pedestrian + ga2.gr.Address.Road;
            textBox4.Text += "From:" + ga1.gr.Address.HouseNumber + " " + no1 + " " + ga1.gr.Address.PostCode + " " + ga1.gr.Address.Town;
            textBox4.Text += " To:" + ga2.gr.Address.HouseNumber + " " + no2 + " " + ga2.gr.Address.PostCode + " " + ga2.gr.Address.Town;
            textBox4.Text += " lat=" + ga1.gr.Latitude.ToString() + "; lon=" + ga1.gr.Longitude.ToString();
            textBox4.Text += ". distance=" + distanceBetween + " m";
            textBox4.Text += ". Within 500m? " + ga1.IsWithinDistanceInMeter(ga2, 500);
            textBox4.Text += ". Is address valid? " + ga1.IsAddressValid(address2);
        }

        private void Button1_Click(object sender, EventArgs e)
        {


        }
    }
}
