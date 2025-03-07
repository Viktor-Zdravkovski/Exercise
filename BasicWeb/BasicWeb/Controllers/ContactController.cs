using BasicWeb.Dto.ContactsDto;
using BasicWeb.Services.Interfaces;
using BasicWeb.Shared.CustomExceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BasicWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet("GetAllContacts")]
        public async Task<ActionResult> GetAllContacts()
        {
            try
            {
                return Ok(await _contactService.GetAll());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("AddContact")]
        public async Task<ActionResult> AddContact([FromBody] AddContactDto addContactDto)
        {
            try
            {
                if (addContactDto == null)
                {
                    return BadRequest("Contact data is required");
                }

                await _contactService.AddContact(addContactDto);
                return Ok("Contact added successfuly");
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

        [HttpPut("UpdateContact")]
        public async Task<ActionResult> UpdateContact([FromBody] UpdateContactDto updateContactDto)
        {
            if (updateContactDto == null)
            {
                return BadRequest("Invalid contact data");
            }

            try
            {
                await _contactService.UpdateContact(updateContactDto);
                return Ok("Contact updated successfuly");
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

        [HttpDelete("DeleteContactById/{id}")]
        public ActionResult DeleteCompanyById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Please input a valid ID");
            }

            try
            {
                _contactService.DeleteContact(id);
                return Ok($"Contact with ID:{id} was successfuly deleted");
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

        [HttpGet("GetContactsWithCompanyAndCountry")]
        public async Task<ActionResult> GetContactsWithCompanyAndCountry()
        {
            try
            {
                var contacts = await _contactService.GetContactsWithCompanyAndCountry();

                if (contacts == null || !contacts.Any())
                {
                    return NotFound("No data found");
                }

                return Ok(contacts);
            }
            catch (NoDataException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }
        }

        [HttpGet("FilterContacts")]
        public async Task<ActionResult> FilterContacts(int countryId, int companyId)
        {
            try
            {
                var filteredContacts = await _contactService.FilterContacts(countryId, companyId);

                if (filteredContacts == null || !filteredContacts.Any())
                {
                    return NotFound("No data found");
                }

                return Ok(filteredContacts);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}