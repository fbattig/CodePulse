using AutoMapper;
using CodePulse.API.Models.Domains;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IRepository<Category> categoryRepository;
        private readonly IMapper mapper;

        public CategoriesController(IRepository<Category> categoryRepository, IMapper mapper)
        {
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequestDto request)
        {
            var category = mapper.Map<Category>(request);

            await categoryRepository.CreateAsync(category);

            var response = mapper.Map<CategoryDto>(category);

            return CreatedAtAction(nameof(GetCategoryById), new { id = response.Id }, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await categoryRepository.GetAllAsync();

            return Ok(mapper.Map<IEnumerable<CategoryDto>>(categories));
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
        {
            var category = await categoryRepository.GetByIdAsync(id);

            if (category is null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<CategoryDto>(category));
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid id, [FromBody] UpdateCategoryRequestDto request)
        {
            var category = mapper.Map<Category>(request);
            category.Id = id;

            var updated = await categoryRepository.UpdateAsync(id, category);

            if (updated is null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<CategoryDto>(updated));
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            var deleted = await categoryRepository.DeleteAsync(id);

            if (deleted is null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<CategoryDto>(deleted));
        }
    }
}
