using AutoMapper;
using CodePulse.API.Models.Domains;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IRepository<BlogPost> blogPostRepository;
        private readonly IMapper mapper;

        public BlogPostsController(IRepository<BlogPost> blogPostRepository, IMapper mapper)
        {
            this.blogPostRepository = blogPostRepository;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlogPost([FromBody] CreateBlogPostRequestDto request)
        {
            var blogPost = mapper.Map<BlogPost>(request);

            await blogPostRepository.CreateAsync(blogPost);

            var response = mapper.Map<BlogPostDto>(blogPost);

            return CreatedAtAction(nameof(GetBlogPostById), new { id = response.Id }, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBlogPosts()
        {
            var blogPosts = await blogPostRepository.GetAllAsync();

            return Ok(mapper.Map<IEnumerable<BlogPostDto>>(blogPosts));
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetBlogPostById([FromRoute] Guid id)
        {
            var blogPost = await blogPostRepository.GetByIdAsync(id);

            if (blogPost is null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<BlogPostDto>(blogPost));
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateBlogPost([FromRoute] Guid id, [FromBody] UpdateBlogPostRequestDto request)
        {
            var blogPost = mapper.Map<BlogPost>(request);
            blogPost.Id = id;

            var updated = await blogPostRepository.UpdateAsync(id, blogPost);

            if (updated is null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<BlogPostDto>(updated));
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteBlogPost([FromRoute] Guid id)
        {
            var deleted = await blogPostRepository.DeleteAsync(id);

            if (deleted is null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<BlogPostDto>(deleted));
        }
    }
}
