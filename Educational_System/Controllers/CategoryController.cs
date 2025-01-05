using AutoMapper;
using Educational_System.Dto.Category;
using EducationalSystem.BLL.Repositories.Interfaces;
using EducationalSystem.BLL.Specification;
using EducationalSystem.BLL.Specification.Specs;
using EducationalSystem.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EducationalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IGenericRepository<Categories> _repository;
        private readonly IMapper _mapper;

        public CategoryController(IGenericRepository<Categories> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var spec = new CategorySpecification();
            var categories = await _repository.GetAllWithSpec(spec);
            var categoriesInfo = _mapper.Map<List<getCategoryDto>>(categories);
            return Ok(categoriesInfo);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var spec = new CategorySpecification();
            var category = await _repository.GetByIdWithSpecAsync(id,spec);
            var categoryInfo = _mapper.Map<getCategoryDto>(category);
            if (categoryInfo == null)
                return NotFound("category not found.");

            return Ok(categoryInfo);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] postCategoryDto categoryDto)
        {

            var mappedcategory = _mapper.Map<postCategoryDto, Categories>(categoryDto);

            if (mappedcategory == null)
                return BadRequest("Category is null.");

            await _repository.AddAsync(mappedcategory);
            return CreatedAtAction(nameof(GetById), new { id = mappedcategory.ID }, mappedcategory);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] postCategoryDto categoryDto)
        {
            if (categoryDto == null)
                return BadRequest("Category data is null.");

            var existingCategory = await _repository.GetByIdAsync(id);
            if (existingCategory == null)
                return NotFound("Category not found.");

            _mapper.Map(categoryDto, existingCategory);

            await _repository.UpdateAsync(existingCategory);
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _repository.GetByIdAsync(id);
            if (category == null)
                return NotFound("Category not found.");

            await _repository.DeleteAsync(category);
            return NoContent();
        }
    }
}
