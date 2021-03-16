using System.Threading.Tasks;
using Dadata;

namespace Models
{
    public class IP
    {
        public async static Task<string> City()
        {
            var api = new SuggestClientAsync("ef2db2da426469acd403d525ff8241bcb5487ef6");
            var response = await api.Geolocate(lat: 55.878, lon: 37.653);
            var address = response.suggestions[0].data.city;

            return address;
        }
    }
}