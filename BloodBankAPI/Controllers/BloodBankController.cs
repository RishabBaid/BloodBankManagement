using BloodBankAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BloodBankAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BloodBankController : ControllerBase
    {
        private static List<BloodBankEntry> BloodBankEntries = new List<BloodBankEntry>();


        [HttpPost]
        public IActionResult Create([FromBody] BloodBankEntry newEntry)
        {
            if (newEntry == null)
            {
                return BadRequest("Invalid input.");
            }

            newEntry.Id = Guid.NewGuid(); // Auto-generate unique ID
            BloodBankEntries.Add(newEntry);
            return CreatedAtAction(nameof(GetById), new { id = newEntry.Id }, newEntry);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(BloodBankEntries);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var entry = BloodBankEntries.FirstOrDefault(e => e.Id == id);
            if (entry == null)
            {
                return BadRequest($"No entry found with id {id}");
            }
            return Ok(entry);
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] BloodBankEntry updatedEntry)
        {
            var existingEntry = BloodBankEntries.FirstOrDefault(e => e.Id == id);
            if (existingEntry == null)
            {
                return NotFound($"No entry found with ID {id}");
            }
            existingEntry.DonorName = updatedEntry.DonorName;
            existingEntry.Age = updatedEntry.Age;
            existingEntry.BloodType = updatedEntry.BloodType;
            existingEntry.ContactInfo = updatedEntry.ContactInfo;
            existingEntry.Quantity = updatedEntry.Quantity;
            existingEntry.CollectionDate = updatedEntry.CollectionDate;
            existingEntry.ExpirationDate = updatedEntry.ExpirationDate;
            existingEntry.Status = updatedEntry.Status;

            return Ok(existingEntry);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var entry = BloodBankEntries.FirstOrDefault(e => e.Id == id);
            if (entry == null)
            {
                return NotFound($"No entry found with ID {id}");
            }
            BloodBankEntries.Remove(entry);
            return Ok($"Entry with ID {id} has been deleted.");
        }

        [HttpGet("pagination")]
        public IActionResult GetPaginated([FromQuery] int page = 1, [FromQuery] int size = 10)
        {
            if (page < 1 || size < 1)
            {
                return BadRequest("Page and size parameters must be greater than 0.");
            }

            var totalEntries = BloodBankEntries.Count;
            var paginatedEntries = BloodBankEntries
                .Skip((page - 1) * size) 
                .Take(size)            
                .ToList();

            var response = new
            {
                TotalEntries = totalEntries,
                Page = page,
                PageSize = size,
                TotalPages = (int)Math.Ceiling((double)totalEntries / size),
                Entries = paginatedEntries
            };

            return Ok(response);
        }
        
        [HttpGet("search")]
        public IActionResult SearchByBloodType([FromQuery] string bloodType = null, [FromQuery] string status = null, [FromQuery] string donorName = null)
        {
            var query = BloodBankEntries.AsQueryable();
            if (!string.IsNullOrWhiteSpace(bloodType))
            {
                query = query.Where(e => e.BloodType.Equals(bloodType, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(e => e.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrWhiteSpace(donorName))
            {
                query = query.Where(e => e.DonorName.Contains(donorName, StringComparison.OrdinalIgnoreCase));
            }
            var results = query.ToList();

            if (!results.Any())
            {
                return NotFound("No matching blood bank entries found.");
            }

            return Ok(results);
        }

    }

}
