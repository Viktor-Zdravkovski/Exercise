using BasicWeb.Dto.CompanyDto;
using BasicWeb.Services.Implementations;
using BasicWeb.Services.Interfaces;
using BasicWeb.Shared.CustomExceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BasicWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet("GetAllCompanies")]
        public async Task<IActionResult> GetAllCompanies()
        {
            try
            {
                return Ok(await _companyService.GetAll());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("AddCompany")]
        public async Task<IActionResult> AddCompany([FromBody] AddCompanyDto addCompanyDto)
        {
            try
            {
                if (addCompanyDto == null)
                {
                    return BadRequest("Company data is required");
                }
                await _companyService.AddCompany(addCompanyDto);
                return Ok("Company added successfully");
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

        [HttpPut("UpdateCompany")]
        public async Task<IActionResult> UpdateCompany([FromBody] UpdateCompanyDto updateCompanyDto)
        {
            if (updateCompanyDto == null)
            {
                return BadRequest("Invalid company data");
            }

            try
            {
                await _companyService.UpdateCompany(updateCompanyDto);
                return Ok("Company updated successfuly");
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

        [HttpDelete("DeleteCompanyById/{id}")]
        public async Task<IActionResult> DeleteCompanyId(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Please input a valid ID");
            }

            try
            {
                await _companyService.DeleteCompany(id);
                return Ok($"Company with ID:{id} was successfuly deleted");
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
    }
}