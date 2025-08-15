using AutoMapper;
using Educational_System.Dto;
using EducationalSystem.BLL.Repositories.Interfaces;
using EducationalSystem.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Educational_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DiscountController> _logger;

        public DiscountController(
            IDiscountRepository discountRepository,
            IMapper mapper,
            ILogger<DiscountController> logger)
        {
            _discountRepository = discountRepository;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/discount
        [HttpGet]
        public async Task<IActionResult> GetAllDiscounts()
        {
            var discounts = await _discountRepository.GetAll();
            var discountDtos = _mapper.Map<IEnumerable<GetDiscountDto>>(discounts);
            return Ok(discountDtos);
        }

        // GET: api/discount/course/5
        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> GetDiscountsByCourseId(int courseId)
        {
            var discounts = await _discountRepository.GetDiscountsByCourseIdAsync(courseId);

            if (!discounts.Any())
                return NotFound($"No discounts found for course {courseId}.");

            var discountDtos = _mapper.Map<IEnumerable<GetDiscountDto>>(discounts);
            return Ok(discountDtos);
        }

        // GET: api/discount/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDiscountById(int id)
        {
            var discount = await _discountRepository.GetByIdAsync(id);
            if (discount == null)
                return NotFound($"Discount {id} not found.");

            var discountDto = _mapper.Map<GetDiscountDto>(discount);
            return Ok(discountDto);
        }

        // POST: api/discount
        [HttpPost]
        public async Task<IActionResult> AddDiscount([FromBody] PostDiscountDto discountDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var discount = _mapper.Map<Discounts>(discountDto);

            await _discountRepository.AddAsync(discount);
            _logger.LogInformation($"Discount {discount.ID} created for course {discount.CourseID}");

            var createdDto = _mapper.Map<GetDiscountDto>(discount);
            return CreatedAtAction(nameof(GetDiscountById), new { id = discount.ID }, createdDto);
        }

        // PUT: api/discount/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDiscount(int id, [FromBody] PostDiscountDto discountDto)
        {
            var existingDiscount = await _discountRepository.GetByIdAsync(id);
            if (existingDiscount == null)
                return NotFound($"Discount {id} not found.");

            _mapper.Map(discountDto, existingDiscount);
            await _discountRepository.UpdateAsync(existingDiscount);

            return NoContent();
        }

        // DELETE: api/discount/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiscount(int id)
        {
            var discount = await _discountRepository.GetByIdAsync(id);
            if (discount == null)
                return NotFound($"Discount {id} not found.");

            await _discountRepository.DeleteAsync(discount);
            return NoContent();
        }
    }
}
