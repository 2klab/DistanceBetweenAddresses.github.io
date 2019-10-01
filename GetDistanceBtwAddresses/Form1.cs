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

        public GeocodeResponse GetGeocode(string address)
        {
            ForwardGeocoder fg = new ForwardGeocoder();
            ForwardGeocodeRequest fgr = new ForwardGeocodeRequest();
            //essayer streetaddress
            Task<GeocodeResponse[]> ar = fg.Geocode(new ForwardGeocodeRequest
            {
                queryString = address,
                DedupeResults = true,
                BreakdownAddressElements = true,
                ShowExtraTags = true,
                ShowAlternativeNames = true,
                ShowGeoJSON = true
            });
            ar.Wait();

            GeocodeResponse gr;
            if (ar.Result.Length == 0)
                return null;

            gr = ar.Result[0];
            return gr;
        }

        public Boolean IsAddressValid(string address)
        {
            GeocodeResponse gr = GetGeocode(address);
            if (gr is null)
                return false;
            else
                return true;
        }

        public double GetDistanceInMeter(GeocodeResponse gr1, GeocodeResponse gr2)
        {
            GeoCoordinate pin1 = new GeoCoordinate(gr1.Latitude, gr1.Longitude);
            GeoCoordinate pin2 = new GeoCoordinate(gr2.Latitude, gr2.Longitude);

            double distanceBetween = pin1.GetDistanceTo(pin2);
            return Math.Round(distanceBetween, 0);
        }

        public Boolean IsWithinDistanceInMeter(GeocodeResponse gr1, GeocodeResponse gr2, double distance = 500)
        {
            double dist = GetDistanceInMeter(gr1, gr2);
            if (dist <= distance)
                return true;
            else
                return false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string address1 = "57 avenue de l'arche, 92400,Courbevoie, France";
            GeocodeResponse gr1 = GetGeocode(address1);


            string address2 = "4 bd montparnasse, 75014 Paris, France";
            GeocodeResponse gr2 = GetGeocode(address2);

            double distanceBetween = GetDistanceInMeter(gr1, gr2);

            string no1 = gr1.Address.Pedestrian + gr1.Address.Road;
            string no2 = gr2.Address.Pedestrian + gr2.Address.Road;
            textBox4.Text += " From:" + gr1.Address.HouseNumber + " " + no1 + " " + gr1.Address.PostCode + " " + gr1.Address.Town;
            textBox4.Text += " To:" + gr2.Address.HouseNumber + " " + no2 + " " + gr2.Address.PostCode + " " + gr2.Address.Town;
            textBox4.Text += " lat=" + gr1.Latitude.ToString() + "; lon=" + gr1.Longitude.ToString();
            textBox4.Text += ". distance=" + distanceBetween + " m";
            textBox4.Text += ". Within 500m? " + IsWithinDistanceInMeter(gr1, gr2, 500);
            textBox4.Text += ". Is valid address ? " + IsAddressValid(address1);

        }

        private void Button1_Click(object sender, EventArgs e)
        {


        }
    }
}
