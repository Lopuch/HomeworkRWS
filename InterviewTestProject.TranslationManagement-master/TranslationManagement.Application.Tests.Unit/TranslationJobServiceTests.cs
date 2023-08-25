using FluentAssertions;
using FluentValidation;
using NSubstitute;
using TranslationManagement.Application.Database;
using TranslationManagement.Application.Models;
using TranslationManagement.Application.Repositories;
using TranslationManagement.Application.Services;

namespace TranslationManagement.Application.Tests.Unit;
public class TranslationJobServiceTests
{
    private readonly TranslationJobService _sut;
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IValidator<TranslationJob> _validator = Substitute.For<IValidator<TranslationJob>>();
    private readonly INotificationService _notificationService = Substitute.For<INotificationService>();
    private readonly ITranslationJobRepo _translationJobRepo = Substitute.For<ITranslationJobRepo>();

    public TranslationJobServiceTests()
    {
        _sut = new TranslationJobService(_validator, _unitOfWork, _notificationService);
    }

    [Fact]
    public async Task GetById_ShouldReturnUser_WhenUserExists()
    {
        // Arrange
        var existingJob = new TranslationJob
        {
            Id = 1,
            CustomerName = "Test",
            OriginalContent = "Test",
            Status = nameof(JobStatuses.New),
            Price = 1,
            TranslatedContent = "Test",
        };
        _translationJobRepo.GetByIdAsync(existingJob.Id).Returns(existingJob);
        _unitOfWork.TranslationJobRepo.Returns(_translationJobRepo);

        // Act
        var result = await _sut.GetByIdAsync(existingJob.Id);

        // Assert
        result.AsT0.Should().BeEquivalentTo(existingJob);
    }
}
