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
    public class CategoriesControllerTests
    {
        private readonly Mock<IRepository<Category>> repository = new();
        private readonly IMapper mapper = MapperHelper.Create();
        private readonly CategoriesController controller;

        public CategoriesControllerTests()
        {
            controller = new CategoriesController(repository.Object, mapper);
        }

        [Fact]
        public async Task CreateCategory_WithValidRequest_ReturnsCreatedWithDto()
        {
            // Arrange
            var request = new CreateCategoryRequestDto { Name = "Tech", UrlHandle = "tech" };
            repository
                .Setup(r => r.CreateAsync(It.IsAny<Category>()))
                .ReturnsAsync((Category c) => { c.Id = Guid.NewGuid(); return c; });

            // Act
            var result = await controller.CreateCategory(request);

            // Assert
            var created = Assert.IsType<CreatedAtActionResult>(result);
            var dto = Assert.IsType<CategoryDto>(created.Value);
            Assert.Equal("Tech", dto.Name);
            Assert.Equal("tech", dto.UrlHandle);
            Assert.NotEqual(Guid.Empty, dto.Id);
        }

        [Fact]
        public async Task GetAllCategories_ReturnsOkWithAllDtos()
        {
            // Arrange
            var categories = new List<Category>
            {
                new() { Id = Guid.NewGuid(), Name = "A", UrlHandle = "a" },
                new() { Id = Guid.NewGuid(), Name = "B", UrlHandle = "b" }
            };
            repository.Setup(r => r.GetAllAsync()).ReturnsAsync(categories);

            // Act
            var result = await controller.GetAllCategories();

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var dtos = Assert.IsAssignableFrom<IEnumerable<CategoryDto>>(ok.Value);
            Assert.Equal(2, dtos.Count());
        }

        [Fact]
        public async Task GetCategoryById_WhenExists_ReturnsOkWithDto()
        {
            // Arrange
            var id = Guid.NewGuid();
            repository.Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(new Category { Id = id, Name = "Tech", UrlHandle = "tech" });

            // Act
            var result = await controller.GetCategoryById(id);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<CategoryDto>(ok.Value);
            Assert.Equal(id, dto.Id);
            Assert.Equal("Tech", dto.Name);
        }

        [Fact]
        public async Task GetCategoryById_WhenMissing_ReturnsNotFound()
        {
            // Arrange
            repository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Category?)null);

            // Act
            var result = await controller.GetCategoryById(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateCategory_WhenExists_ReturnsOkWithUpdatedDto()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new UpdateCategoryRequestDto { Name = "Updated", UrlHandle = "updated" };
            repository
                .Setup(r => r.UpdateAsync(id, It.IsAny<Category>()))
                .ReturnsAsync((Guid _, Category c) => c);

            // Act
            var result = await controller.UpdateCategory(id, request);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<CategoryDto>(ok.Value);
            Assert.Equal(id, dto.Id);
            Assert.Equal("Updated", dto.Name);
        }

        [Fact]
        public async Task UpdateCategory_WhenMissing_ReturnsNotFound()
        {
            // Arrange
            repository
                .Setup(r => r.UpdateAsync(It.IsAny<Guid>(), It.IsAny<Category>()))
                .ReturnsAsync((Category?)null);

            // Act
            var result = await controller.UpdateCategory(Guid.NewGuid(),
                new UpdateCategoryRequestDto { Name = "x", UrlHandle = "x" });

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteCategory_WhenExists_ReturnsOkWithDeletedDto()
        {
            // Arrange
            var id = Guid.NewGuid();
            repository.Setup(r => r.DeleteAsync(id))
                .ReturnsAsync(new Category { Id = id, Name = "Gone", UrlHandle = "gone" });

            // Act
            var result = await controller.DeleteCategory(id);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<CategoryDto>(ok.Value);
            Assert.Equal(id, dto.Id);
        }

        [Fact]
        public async Task DeleteCategory_WhenMissing_ReturnsNotFound()
        {
            // Arrange
            repository.Setup(r => r.DeleteAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Category?)null);

            // Act
            var result = await controller.DeleteCategory(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
