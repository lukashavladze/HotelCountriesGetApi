using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelGetApi;
using Newtonsoft.Json;

namespace HotelGetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly HotelDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public HotelsController(HotelDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            this._httpClientFactory = httpClientFactory;
        }

        // GET: api/Hotels
        [HttpGet("hotels")]
        public async Task<ActionResult> Gethotels()
        {
            var httpcliet = _httpClientFactory.CreateClient();
            var response = await httpcliet.GetAsync("https://localhost:7033/api/Hotels");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var hotels = JsonConvert.DeserializeObject<List<Hotel>>(content);
            foreach (var hot in hotels)
            {
                var existingHotel = await _context.Hotels
            .FirstOrDefaultAsync(h => h.Name == hot.Name && h.Address == hot.Address && h.CountryId == hot.CountryId);
                if (existingHotel == null)
                {
                    var newHotel = new Hotel
                    {
                        Name = hot.Name,
                        Address = hot.Address,
                        Rating = hot.Rating,
                        CountryId = hot.CountryId,
                        
                    };
                    await _context.AddAsync(newHotel);
                }

            }
            await _context.SaveChangesAsync();
            return Ok(hotels);
        }

        [HttpGet("countries")]
        public async Task<ActionResult> GetCountry()
        {
            var httpcliet = _httpClientFactory.CreateClient();
            var response = await httpcliet.GetAsync("https://localhost:7033/api/v2/countries");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var countries = JsonConvert.DeserializeObject<List<Country>>(content);
            foreach(var country in countries)
            {
                var newcountry = new Country
                {
                    Name = country.Name,
                    ShortName = country.ShortName,

                };
                await _context.AddAsync(newcountry);

            }
            await _context.SaveChangesAsync();
            return Ok(countries);
        }



        private bool CountryExists(int id)
        {
            return (_context.Countries?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
