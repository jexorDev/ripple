using Ripple.DataLayer.Models;
using System.Security.Cryptography;
using System.Text;

namespace Ripple.Utility
{
    public class GoogleMaps
    {
        public static async Task<byte[]?> GetMapByteArray(string googleApiKey, string googleSecret, List<ItineraryPlaceModel> places)
        {
            
            var firstLocation = places.Where(p => p.Place != null).FirstOrDefault();

            if (firstLocation == null) return null;

            string center = firstLocation.Place.Location.Latitude + "," + firstLocation.Place.Location.Longitude;

            var markers = "";

            foreach (var place in places)
            {
                if(place.Place != null)
                    markers += $"&markers=color:red|label:{place.Index}|{place.Place.Location.Latitude},{place.Place.Location.Longitude}";
            }

            const string Url = "https://maps.googleapis.com/maps/api/staticmap?center={0}&zoom=12&size=800x800&maptype=roadmap&key={1}{2}";
            
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(Sign(string.Format(Url, center, googleApiKey, markers), googleSecret));
                var mapByteArray = await response.Content.ReadAsByteArrayAsync();
                return mapByteArray;
            }
        }

        private static string Sign(string url, string keyString)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();

            // converting key to bytes will throw an exception, need to replace '-' and '_' characters first.
            string usablePrivateKey = keyString.Replace("-", "+").Replace("_", "/");
            byte[] privateKeyBytes = Convert.FromBase64String(usablePrivateKey);

            Uri uri = new Uri(url);
            byte[] encodedPathAndQueryBytes = encoding.GetBytes(uri.LocalPath + uri.Query);

            // compute the hash
            HMACSHA1 algorithm = new HMACSHA1(privateKeyBytes);
            byte[] hash = algorithm.ComputeHash(encodedPathAndQueryBytes);

            // convert the bytes to string and make url-safe by replacing '+' and '/' characters
            string signature = Convert.ToBase64String(hash).Replace("+", "-").Replace("/", "_");

            // Add the signature to the existing URI.
            return uri.Scheme + "://" + uri.Host + uri.LocalPath + uri.Query + "&signature=" + signature;
        }


    }
}
