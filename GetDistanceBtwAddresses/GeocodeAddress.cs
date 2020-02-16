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
        /// Maximum flying delivery distance in meters around a point
        /// </summary>
        public float MaxFlyingDeliveryDistanceInMeter { get; set; } = 500;

    }
    public class GeocodeAddress
    {
        public GeocodeAddress() { }
        public GeocodeAddress(string address)
        {
            Address = address;
        }


        private string _Address;
        /// <summary>
        /// As soon as the address is set, calculate its GeoCoordinates
        /// Could be postponed using a "dirty" flag
        /// </summary>
        public string Address
        {
            get
            {
                return _Address;
            }
            set
            {
                _Address = value;
                CalcGeocodeResponse();
            }
        }

        public string FixedAdress
        {
            get
            {
                if (GeocodeResponse == null || GeocodeResponse.Address == null)
                    return UndefinedString;
                string no1 = GeocodeResponse.Address.Pedestrian + GeocodeResponse.Address.Road;
                return GeocodeResponse.Address.HouseNumber + " " + no1 + " " + GeocodeResponse.Address.PostCode
                    + " " + GeocodeResponse.Address.Town
                + " " + GeocodeResponse.Address.Country;
            }
        }

        public double Latitude
        {
            get
            {
                if (GeocodeResponse == null || GeocodeResponse.Address == null)
                    return Double.NaN;
                else
                    return GeocodeResponse.Latitude;
            }
        }

        public double Longitude
        {
            get
            {
                if (GeocodeResponse == null || GeocodeResponse.Address == null)
                    return Double.NaN;
                else
                    return GeocodeResponse.Longitude;
            }
        }

        private GeocodeResponse GeocodeResponse { get; set; } = null;
        private GeoCoordinate pinFrom;
        public string UndefinedString = "<Undefined>";
        public double GetDistanceInMeter(GeocodeAddress gAddressTo)
        {
            if (GeocodeResponse == null || gAddressTo == null || gAddressTo.GeocodeResponse == null)
                return Double.NaN;
            pinFrom = new GeoCoordinate(GeocodeResponse.Latitude, GeocodeResponse.Longitude);
#pragma warning disable CA1062 // Validate arguments of public methods
            GeoCoordinate pinTo = new GeoCoordinate(gAddressTo.GeocodeResponse.Latitude, gAddressTo.GeocodeResponse.Longitude);

            double distanceBetween = pinFrom.GetDistanceTo(pinTo);
            return Math.Round(distanceBetween, 0);
        }

        public void CalcGeocodeResponse()
        {
            //if (address == _address)
            //     return GeocodeResponse;

            //SetAddress(address);

            ForwardGeocoder fg = new ForwardGeocoder();
            //ForwardGeocodeRequest fgr = new ForwardGeocodeRequest();
            Task<GeocodeResponse[]> ar = fg.Geocode(new ForwardGeocodeRequest
            {
                queryString = Address,
                DedupeResults = true,
                BreakdownAddressElements = true,
                ShowExtraTags = true,
                ShowAlternativeNames = true,
                ShowGeoJSON = true
            });
            ar.Wait();

            if (ar.Result.Length == 0)
                GeocodeResponse = null;
            else
                GeocodeResponse = ar.Result[0];
        }

        public bool IsAddressValid()
        {
            if (GeocodeResponse == null)
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
