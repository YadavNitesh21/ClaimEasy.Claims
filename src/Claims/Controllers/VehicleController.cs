using Claims.ClaimDbContext;
using Claims.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Claims.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly ClaimsDbContext _context;
        private readonly ILogger<VehicleController> _logger;

        public VehicleController(ClaimsDbContext context, ILogger<VehicleController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAllVehicles()
        {
            _logger.LogInformation("Fetching all vehicles.");

            var result = _context.Vehicles.ToList();
            if (result.Count == 0)
            {
                _logger.LogWarning("No vehicles found.");
                return NotFound("No Vehicle found");
            }

            _logger.LogInformation("Successfully fetched {Count} vehicles.", result.Count);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetVehicleById([FromRoute] Guid id)
        {
            _logger.LogInformation("Fetching vehicle with ID: {VehicleId}", id);

            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(x => x.Id == id);
            if (vehicle == null)
            {
                _logger.LogWarning("Vehicle with ID {VehicleId} not found.", id);
                return NotFound("Id not found");
            }

            _logger.LogInformation("Successfully fetched vehicle with ID: {VehicleId}", id);
            return Ok(vehicle);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterVehicle(VehicleRequest vehicle)
        {
            _logger.LogInformation("Registering a new vehicle.");

            var vehicleRequest = new Vehicle()
            {
                VehicleNumber = vehicle.VehicleNumber,
                Make = vehicle.Make,
                Model = vehicle.Model,
                Year = vehicle.Year,
                Color = vehicle.Color,
                Country = vehicle.Country,
                State = vehicle.State,
            };
            vehicleRequest.CreatedAt = DateTime.Now;
            vehicleRequest.UpdatedAt = DateTime.Now;

            await _context.Vehicles.AddAsync(vehicleRequest);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Successfully registered a new vehicle with ID: {VehicleId}", vehicleRequest.Id);
            return Ok(vehicle);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateVehicle([FromRoute] Guid id, [FromBody] VehicleRequest request)
        {
            _logger.LogInformation("Updating vehicle with ID: {VehicleId}", id);

            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(x => x.Id == id);
            if (vehicle == null)
            {
                _logger.LogWarning("Vehicle with ID {VehicleId} not found for update.", id);
                return NotFound("Id not found, Can't be updated");
            }

            vehicle.Make = request.Make;
            vehicle.Model = request.Model;
            vehicle.Year = request.Year;
            vehicle.Color = request.Color;
            vehicle.Country = request.Country;
            vehicle.State = request.State;
            vehicle.VehicleNumber = request.VehicleNumber;
            vehicle.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Successfully updated vehicle with ID: {VehicleId}", id);
            return Ok(vehicle);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteVehicle([FromRoute] Guid id)
        {
            _logger.LogInformation("Deleting vehicle with ID: {VehicleId}", id);

            var vehicle = await _context.Vehicles.FirstOrDefaultAsync(x => x.Id == id);

            if (vehicle == null)
            {
                _logger.LogWarning("Vehicle with ID {VehicleId} not found for deletion.", id);
                return NotFound("Id not found");
            }

            _context.Remove(vehicle);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Successfully deleted vehicle with ID: {VehicleId}", id);
            return Ok(vehicle);
        }
    }
}