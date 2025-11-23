using AccentInt.Application.Interfaces;
using AccentInt.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace AccentInt.ApiService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HolidaysController(IHolidayService holidayService) : ControllerBase
{
    private readonly IHolidayService _holidayService = holidayService;

    [HttpPost("three")]
    public async Task<ActionResult> GetLastThreeHolidays([FromBody] HolidaysRequest holidaysRequest)
    {
        if(holidaysRequest.Countries is null || holidaysRequest.Countries.Count == 0 || holidaysRequest.Countries.Count > 1)
        {
            return BadRequest("Provide exact one country code");
        }

        return Ok(await _holidayService.GetLastThreeHolidays(holidaysRequest));
    }

    [HttpPost("count")]
    public async Task<ActionResult> GetNumberOfHolidays([FromBody] HolidaysRequest holidaysRequest)
    {
        if(holidaysRequest.Year is null || holidaysRequest.Countries is null || holidaysRequest.Countries.Count == 0)
        {
            return BadRequest("Provide proper year and country codes");
        }

        return Ok(await _holidayService.GetHolidaysDuringWorkDays(holidaysRequest));
    }

    [HttpPost("common")]
    public async Task<ActionResult> GetCommonHolidays([FromBody] HolidaysRequest holidaysRequest)
    {
        if (holidaysRequest.Year is null || holidaysRequest.Countries is null || holidaysRequest.Countries.Count < 2)
        {
            return BadRequest("Provide proper year and country codes");
        }

        return Ok(await _holidayService.GetDuplicatedHolidays(holidaysRequest));
    }

}
