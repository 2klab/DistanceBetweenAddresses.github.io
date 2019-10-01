using Nominatim.API.Geocoders;
using Nominatim.API.Models;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetDistanceBtwAddresses
{
    public class GeocodeAddress
    {
        public GeocodeAddress() { }
        public GeocodeAddress(string address)
        {
            Address = address;
            gr=GetGeocode(Address);
        }
        public string Address { get; set; }
        public GeocodeResponse gr { get; set; }
        public GeoCoordinate pin1;

        public double GetDistanceInMeter(GeocodeAddress gr2)
        {
            pin1 = new GeoCoordinate(gr.Latitude, gr.Longitude);
            GeoCoordinate pin2 = new GeoCoordinate(gr2.gr.Latitude, gr2.gr.Longitude);

            double distanceBetween = pin1.GetDistanceTo(pin2);
            return Math.Round(distanceBetween, 0);
        }

        public GeocodeResponse GetGeocode(string address)
        {
            Address = address;

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

            if (ar.Result.Length == 0)
                return null;

            gr = ar.Result[0];
            return gr;
        }

        public bool IsAddressValid(string address)
        {
            this.Address = address;
            GeocodeResponse gr = GetGeocode(address);
            if (gr is null)
                return false;
            else
                return true;
        }

        public bool IsWithinDistanceInMeter(GeocodeAddress gr2, double distance = 500)
        {
            double dist = GetDistanceInMeter(gr2);
            if (dist <= distance)
                return true;
            else
                return false;
        }

    }
}
