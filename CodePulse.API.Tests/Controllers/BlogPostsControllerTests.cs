using AutoMapper;
using CodePulse.API.Controllers;
using CodePulse.API.Models.Domains;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using CodePulse.API.Tests.TestHelpers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CodePulse.API.Tests.Controllers
{
    public class BlogPostsControllerTests
    {
        private readonly Mock<IRepository<BlogPost>> repository = new();
        private readonly IMapper mapper = MapperHelper.Create();
        private readonly BlogPostsController controller;

        public BlogPostsControllerTests()
        {
            controller = new BlogPostsController(repository.Object, mapper);
        }

        private static CreateBlogPostRequestDto SampleCreateRequest() => new()
        {
            Title = "Post",
            ShortDescription = "Short",
            Content = "Content",
            FeaturedImageUrl = "https://example.com/i.jpg",
            UrlHandle = "post",
            PublishedDate = new DateTime(2026, 1, 1),
            Author = "Felix",
            IsVisible = true
        };

        [Fact]
        public async Task CreateBlogPost_WithValidRequest_ReturnsCreatedWithDto()
        {
            // Arrange
            repository
                .Setup(r => r.CreateAsync(It.IsAny<BlogPost>()))
                .ReturnsAsync((BlogPost b) => { b.Id = Guid.NewGuid(); return b; });

            // Act
            var result = await controller.CreateBlogPost(SampleCreateRequest());

            // Assert
            var created = Assert.IsType<CreatedAtActionResult>(result);
            var dto = Assert.IsType<BlogPostDto>(created.Value);
            Assert.Equal("Post", dto.Title);
            Assert.True(dto.IsVisible);
            Assert.NotEqual(Guid.Empty, dto.Id);
        }

        [Fact]
        public async Task GetAllBlogPosts_ReturnsOkWithAllDtos()
        {
            // Arrange
            var posts = new List<BlogPost>
            {
                new() { Id = Guid.NewGuid(), Title = "One", UrlHandle = "one" },
                new() { Id = Guid.NewGuid(), Title = "Two", UrlHandle = "two" }
            };
            repository.Setup(r => r.GetAllAsync()).ReturnsAsync(posts);

            // Act
            var result = await controller.GetAllBlogPosts();

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var dtos = Assert.IsAssignableFrom<IEnumerable<BlogPostDto>>(ok.Value);
            Assert.Equal(2, dtos.Count());
        }

        [Fact]
        public async Task GetBlogPostById_WhenExists_ReturnsOkWithDto()
        {
            // Arrange
            var id = Guid.NewGuid();
            repository.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(new BlogPost { Id = id, Title = "Post", UrlHandle = "post" });

            // Act
            var result = await controller.GetBlogPostById(id);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<BlogPostDto>(ok.Value);
            Assert.Equal(id, dto.Id);
        }

        [Fact]
        public async Task GetBlogPostById_WhenMissing_ReturnsNotFound()
        {
            // Arrange
            repository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((BlogPost?)null);

            // Act
            var result = await controller.GetBlogPostById(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateBlogPost_WhenExists_ReturnsOkWithUpdatedDto()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new UpdateBlogPostRequestDto
            {
                Title = "Updated",
                ShortDescription = "s",
                Content = "c",
                FeaturedImageUrl = "u",
                UrlHandle = "updated",
                PublishedDate = new DateTime(2026, 2, 2),
                Author = "Jane",
                IsVisible = false
            };
            repository
                .Setup(r => r.UpdateAsync(id, It.IsAny<BlogPost>()))
                .ReturnsAsync((Guid _, BlogPost b) => b);

            // Act
            var result = await controller.UpdateBlogPost(id, request);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<BlogPostDto>(ok.Value);
            Assert.Equal(id, dto.Id);
            Assert.Equal("Updated", dto.Title);
            Assert.False(dto.IsVisible);
        }

        [Fact]
        public async Task UpdateBlogPost_WhenMissing_ReturnsNotFound()
        {
            // Arrange
            repository
                .Setup(r => r.UpdateAsync(It.IsAny<Guid>(), It.IsAny<BlogPost>()))
                .ReturnsAsync((BlogPost?)null);

            // Act
            var result = await controller.UpdateBlogPost(Guid.NewGuid(),
                new UpdateBlogPostRequestDto { Title = "x", UrlHandle = "x" });

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteBlogPost_WhenExists_ReturnsOkWithDeletedDto()
        {
            // Arrange
            var id = Guid.NewGuid();
            repository.Setup(r => r.DeleteAsync(id))
                .ReturnsAsync(new BlogPost { Id = id, Title = "Gone", UrlHandle = "gone" });

            // Act
            var result = await controller.DeleteBlogPost(id);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<BlogPostDto>(ok.Value);
            Assert.Equal(id, dto.Id);
        }

        [Fact]
        public async Task DeleteBlogPost_WhenMissing_ReturnsNotFound()
        {
            // Arrange
            repository.Setup(r => r.DeleteAsync(It.IsAny<Guid>()))
                .ReturnsAsync((BlogPost?)null);

            // Act
            var result = await controller.DeleteBlogPost(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
