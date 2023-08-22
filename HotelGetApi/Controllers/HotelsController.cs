using HotelGetApi;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class HotelsController : ControllerBase
{
    private readonly IHotelDataService _hotelDataService;

    public HotelsController(IHotelDataService hotelDataService)
    {
        _hotelDataService = hotelDataService;
    }

    [HttpGet("hotels")]
    public async Task<ActionResult> Gethotels()
    {
        // Instead of calling the logic directly in the controller,
        // delegate it to the IHotelDataService service
        await _hotelDataService.ProcessHotelDataAsync();

        // Return response or data as needed
        // ...
        return Ok();
    }

    [HttpGet("countries")]
    public async Task<ActionResult> GetCountry()
    {
        // Instead of calling the logic directly in the controller,
        // delegate it to the IHotelDataService service
        await _hotelDataService.ProcessHotelDataAsync();

        // Return response or data as needed
        // ...
        return Ok();
    }

}
