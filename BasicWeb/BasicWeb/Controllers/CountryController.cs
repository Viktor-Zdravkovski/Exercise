using BasicWeb.Dto.ContactsDto;
using BasicWeb.Dto.CountryDto;
using BasicWeb.Services.Interfaces;
using BasicWeb.Shared.CustomExceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BasicWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryService;

        public CountryController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        [HttpGet("GetAllCountries")]
        public async Task<IActionResult> GetAllCountries()
        {
            try
            {
                var countries = await _countryService.GetAll();

                if (countries == null || !countries.Any())
                {
                    return NotFound("No countries found.");
                }

                return Ok(countries);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("AddCountry")]
        public async Task<IActionResult> AddCountry([FromBody] AddCountryDto addCountryDto)
        {
            try
            {
                if (addCountryDto == null)
                {
                    return BadRequest("Country data is required");
                }

                await _countryService.AddCountry(addCountryDto);
                return Ok("Country added successfuly");
            }
            catch (NoDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("UpdateCountry")]
        public async Task<IActionResult> UpdateCountry([FromBody] UpdateCountryDto updateCountryDto)
        {
            try
            {
                if (updateCountryDto == null)
                {
                    return BadRequest("Invalid contact data");
                }

                await _countryService.UpdateCountry(updateCountryDto);
                return Ok("Country updated successfuly");
            }
            catch (NoDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("DeleteCountryById/{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Please input an ID");
            }

            try
            {
                await _countryService.DeleteCountry(id);
                return Ok($"Country with ID:{id} has been deleted successfuly");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetCompanyStatisticsByCountryId/{id}")]
        public async Task<IActionResult> CompanyStatistics(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Please input a valid ID");
            }

            try
            {
                Dictionary<string, int> country = await _countryService.GetCompanyStatisticsByCountryId(id);
                return Ok(country);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}