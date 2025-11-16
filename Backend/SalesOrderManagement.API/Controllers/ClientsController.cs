using Microsoft.AspNetCore.Mvc;
using SalesOrderManagement.Application.Models;
using SalesOrderManagement.Application.Interfaces;

namespace SalesOrderManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientRepository _clientRepository;

        public ClientsController(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientDto>>> GetClients()
        {
            var clients = await _clientRepository.GetAllAsync();
            var clientDtos = clients.Select(c => new ClientDto
            {
                Id = c.Id,
                CustomerName = c.CustomerName,
                Address1 = c.Address1,
                Address2 = c.Address2,
                Address3 = c.Address3,
                Suburb = c.Suburb,
                State = c.State,
                PostCode = c.PostCode
            });

            return Ok(clientDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClientDto>> GetClient(int id)
        {
            var client = await _clientRepository.GetByIdAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            var clientDto = new ClientDto
            {
                Id = client.Id,
                CustomerName = client.CustomerName,
                Address1 = client.Address1,
                Address2 = client.Address2,
                Address3 = client.Address3,
                Suburb = client.Suburb,
                State = client.State,
                PostCode = client.PostCode
            };

            return Ok(clientDto);
        }
    }
}