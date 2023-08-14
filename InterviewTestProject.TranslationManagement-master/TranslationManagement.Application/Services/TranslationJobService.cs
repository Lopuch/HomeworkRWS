using External.ThirdParty.Services;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using OneOf;
using OneOf.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TranslationManagement.Application.Database;
using TranslationManagement.Application.Models;
using TranslationManagement.Application.Validators;

namespace TranslationManagement.Application.Services;

public interface ITranslationJobService
{
    Task<IEnumerable<TranslationJob>> GetAllJobsAsync();
    Task<OneOf<TranslationJob, NotFound>> GetByIdAsync(int jobId);
    Task<OneOf<Success, ValidationFailed>> Create(TranslationJob job);
    Task<OneOf<Success, ValidationFailed>> CreateWithFile(IFormFile file, string customer);
    Task<OneOf<Success, NotFound, ValidationFailed>> UpdateJobStatus(int jobId, string newStatus);
    void SetPrice(TranslationJob job);
}

public class TranslationJobService : ITranslationJobService
{
    private readonly IValidator<TranslationJob> _jobValidator;
    private readonly IUnitOfWork _unitOfWork;

    private readonly INotificationService _notificationService;

    const double PricePerCharacter = 0.01;

    public TranslationJobService(IValidator<TranslationJob> jobValidator, IUnitOfWork unitOfWork, INotificationService notificationService)
    {
        _jobValidator = jobValidator;
        _unitOfWork = unitOfWork;
        _notificationService = notificationService;
    }

    public async Task<IEnumerable<TranslationJob>> GetAllJobsAsync()
    {
        return await _unitOfWork.TranslationJobRepo.GetAllAsync();
    }

    public async Task<OneOf<TranslationJob, NotFound>> GetByIdAsync(int jobId)
    {
        var job = await _unitOfWork.TranslationJobRepo.GetByIdAsync(jobId);
        if(job is null)
        {
            return new NotFound();
        }

        return job;
    }

    public async Task<OneOf<Success, ValidationFailed>> Create(TranslationJob job)
    {
        job.Status = "New";

        SetPrice(job);

        var validationResult = _jobValidator.Validate(job);
        if(!validationResult.IsValid)
        {
            return new ValidationFailed(validationResult.Errors);
        }

        _unitOfWork.TranslationJobRepo.Add(job);
        await _unitOfWork.Complete();

        await _notificationService.AddNotificationToSend(job.Id);

        return new Success();
    }

    public async Task<OneOf<Success, ValidationFailed>> CreateWithFile(IFormFile file, string customer)
    {
        var fileResult = ProcessFile(file, customer);

        if(fileResult.Value is ValidationFailure)
        {
            return fileResult.AsT1;
        }

        var processedFile = fileResult.AsT0;

        var newJob = new TranslationJob()
        {
            OriginalContent = processedFile.content,
            TranslatedContent = "",
            CustomerName = processedFile.customer,
        };

        SetPrice(newJob);

        return await Create(newJob);
    }

    public void SetPrice(TranslationJob job)
    {
        job.Price = job.OriginalContent.Length * PricePerCharacter;
    }

    public OneOf<(string content, string customer), ValidationFailed> ProcessFile(IFormFile file, string customer)
    {
        var reader = new StreamReader(file.OpenReadStream());
        string content;

        if (file.FileName.EndsWith(".txt"))
        {
            content = reader.ReadToEnd();
            return (content, customer);
        }

        if (file.FileName.EndsWith(".xml"))
        {
            var xdoc = XDocument.Parse(reader.ReadToEnd());

            try
            {
                content = xdoc.Root!.Element("Content")!.Value;
                customer = xdoc.Root!.Element("Customer")!.Value.Trim();
            }
            catch
            {
                return new ValidationFailed(new ValidationFailure("file", "unable to process .xml file"));
            }

            return (content, customer);
        }

        return new ValidationFailed(new ValidationFailure("file", "unsupported file"));
 
    }

    public async Task<OneOf<Success, NotFound, ValidationFailed>> UpdateJobStatus(int jobId, string newStatus)
    {
        var jobResult = await GetByIdAsync(jobId);
        if(jobResult.Value is NotFound)
        {
            return new NotFound();
        }

        var job = jobResult.AsT0;


        bool isInvalidStatusChange = (job.Status == nameof(JobStatuses.New) && newStatus == nameof(JobStatuses.Completed)) ||
                                         job.Status == nameof(JobStatuses.Completed) || newStatus == nameof(JobStatuses.New);
        if (isInvalidStatusChange)
        {
            return new ValidationFailed(new ValidationFailure("status", "invalid status change"));
        }


        job.Status = newStatus;
        
        var validationResult = _jobValidator.Validate(job);
        if(!validationResult.IsValid)
        {
            return new ValidationFailed(validationResult.Errors);
        }

        await _unitOfWork.Complete();

        return new Success();
    }
}
