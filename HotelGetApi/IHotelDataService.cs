using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace HotelGetApi
{
    public interface IHotelDataService
    {
        Task ProcessHotelDataAsync();
    }

    public class HotelDataService : IHotelDataService
    {
        private readonly HotelDbContext _dbContext;
        private readonly IHttpClientFactory _httpClientFactory;

        public HotelDataService(HotelDbContext dbContext, IHttpClientFactory httpClientFactory)
        {
            _dbContext = dbContext;
            _httpClientFactory = httpClientFactory;
        }

        public async Task ProcessHotelDataAsync()
        {
            await FetchAndProcessHotelsAsync();
            await FetchAndProcessCountriesAsync();
        }

        private async Task FetchAndProcessHotelsAsync()
        {
            // Fetch hotels from external API
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync("https://localhost:7033/api/Hotels");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var hotels = JsonConvert.DeserializeObject<List<Hotel>>(content);

            foreach (var hotel in hotels)
            {
                var existingHotel = await _dbContext.Hotels.FirstOrDefaultAsync(h =>
                    h.Name == hotel.Name && h.Address == hotel.Address && h.CountryId == hotel.CountryId);

                if (existingHotel == null)
                {
                    var newHotel = new Hotel
                    {
                        Name = hotel.Name,
                        Address = hotel.Address,
                        Rating = hotel.Rating,
                        CountryId = hotel.CountryId,
                    };
                    await _dbContext.AddAsync(newHotel);
                }
            }

            await _dbContext.SaveChangesAsync();
        }

        private async Task FetchAndProcessCountriesAsync()
        {
            // Fetch countries from external API
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync("https://localhost:7033/api/v2/countries");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var countries = JsonConvert.DeserializeObject<List<Country>>(content);

            foreach (var country in countries)
            {
                var existingCountry = await _dbContext.Countries.FirstOrDefaultAsync(n =>
                    n.Name == country.Name && n.ShortName == country.ShortName);

                if (existingCountry == null)
                {
                    var newCountry = new Country
                    {
                        Name = country.Name,
                        ShortName = country.ShortName,
                    };
                    await _dbContext.AddAsync(newCountry);
                }
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}