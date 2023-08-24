using FluentValidation;
using Microsoft.Extensions.Logging;
using OneOf;
using OneOf.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslationManagement.Application.Database;
using TranslationManagement.Application.Models;
using TranslationManagement.Application.Validators;

namespace TranslationManagement.Application.Services;

public interface ITranslatorModelService
{
    Task<IEnumerable<TranslatorModel>> GetAllTranslators();
    Task<IEnumerable<TranslatorModel>> GetTranslatorsByName(string name);
    Task<OneOf<TranslatorModel, NotFound>> GetById(int id);
    Task<OneOf<Success, ValidationFailed>> Create(TranslatorModel translator);
    Task<OneOf<TranslatorModel, NotFound, ValidationFailed>> Update(TranslatorModel translator);
    Task<OneOf<Success, NotFound, ValidationFailed>> UpdateStatus(int translatorId, string status);
}

public class TranslatorModelService : ITranslatorModelService
{
    private readonly IValidator<TranslatorModel> _translatorValidator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TranslatorModelService> _logger;

    public TranslatorModelService(IValidator<TranslatorModel> translatorValidator, 
        IUnitOfWork unitOfWork, 
        ILogger<TranslatorModelService> logger)
    {
        _translatorValidator = translatorValidator;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<IEnumerable<TranslatorModel>> GetAllTranslators()
    {
        return await _unitOfWork.TranslatorModelRepo.GetAllAsync();
    }

    public async Task<IEnumerable<TranslatorModel>> GetTranslatorsByName(string name)
    {
        return await _unitOfWork.TranslatorModelRepo.GetByName(name);
    }

    public async Task<OneOf<Success, ValidationFailed>> Create(TranslatorModel translator)
    {
        var validationResult = _translatorValidator.Validate(translator);
        if(!validationResult.IsValid)
        {
            return new ValidationFailed(validationResult.Errors);
        }

        _unitOfWork.TranslatorModelRepo.Add(translator);
        await _unitOfWork.Complete();

        return new Success();
    }

    public async Task<OneOf<TranslatorModel, NotFound, ValidationFailed>> Update(TranslatorModel translator)
    {
        var validationResult = _translatorValidator.Validate(translator);
        if (!validationResult.IsValid)
        {
            return new ValidationFailed(validationResult.Errors);
        }

        var existingValidatorRes = await GetById(translator.Id);
        if(existingValidatorRes.Value is NotFound)
        {
            return new NotFound();
        }

        var translatorDb = existingValidatorRes.AsT0;

        translatorDb.Status = translator.Status;
        translatorDb.CreditCardNumber = translator.CreditCardNumber;
        translatorDb.HourlyRate = translator.HourlyRate;
        translatorDb.Name = translator.Name;

        await _unitOfWork.Complete();
        return translatorDb;
    }

    public async Task<OneOf<TranslatorModel, NotFound>> GetById(int id)
    {
        var translator = await _unitOfWork.TranslatorModelRepo.GetByIdAsync(id);
        if(translator is null)
        {
            return new NotFound();
        }

        return translator;
    }

    public async Task<OneOf<Success, NotFound, ValidationFailed>> UpdateStatus(int translatorId, string status)
    {
        var translator = await _unitOfWork.TranslatorModelRepo.GetByIdAsync(translatorId);
        if (translator is null)
        {
            return new NotFound();
        }

        _logger.LogInformation("User status update request: {status} for user {translator}", status, translator);

        translator.Status = status;

        var validationResult = _translatorValidator.Validate(translator);
        if (!validationResult.IsValid)
        {
            return new ValidationFailed(validationResult.Errors);
        }

        

        await _unitOfWork.Complete();

        

        return new Success();
    }
}
