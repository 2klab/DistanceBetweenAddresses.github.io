using Nominatim.API.Geocoders;
using Nominatim.API.Models;
using System;
using System.Device.Location;
using System.Threading.Tasks;

namespace GetDistanceBetweenAddresses
{
#pragma warning disable CA1305 // Specify IFormatProvider
    public class GeocodeAddressSettings
    {
        /// <summary>
        /// Maximum delivery distance in meters around a point
        /// </summary>
        public float MaxDeliveryDistanceInMeter { get; set; } = 500;

    }
    public class GeocodeAddress
    {
        public GeocodeAddress() { }
        public GeocodeAddress(string address)
        {
            Address = address;
            GeocodeResponse = GetGeocode(Address);
        }
        public string Address { get; set; }
        public GeocodeResponse GeocodeResponse { get; set; }
        private GeoCoordinate pin1;

        public override string ToString()
        {
            return GeocodeResponse.Address.HouseNumber.ToString();
        }
        public double GetDistanceInMeter(GeocodeAddress address)
        {
            pin1 = new GeoCoordinate(GeocodeResponse.Latitude, GeocodeResponse.Longitude);
#pragma warning disable CA1062 // Validate arguments of public methods
            GeoCoordinate pin2 = new GeoCoordinate(address.GeocodeResponse.Latitude, address.GeocodeResponse.Longitude);

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

            GeocodeResponse = ar.Result[0];
            return GeocodeResponse;
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
