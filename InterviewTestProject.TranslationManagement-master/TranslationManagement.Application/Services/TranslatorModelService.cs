using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslationManagement.Application.Database;
using TranslationManagement.Application.Models;

namespace TranslationManagement.Application.Services;

public interface ITranslatorModelService
{
    Task<IEnumerable<TranslatorModel>> GetAllTranslators();
    Task<IEnumerable<TranslatorModel>> GetTranslatorsByName(string name);
}

public class TranslatorModelService : ITranslatorModelService
{
    private readonly IValidator<TranslatorModel> _translatorValidator;
    private readonly IUnitOfWork _unitOfWork;

    public TranslatorModelService(IValidator<TranslatorModel> translatorValidator, IUnitOfWork unitOfWork)
    {
        _translatorValidator = translatorValidator;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<TranslatorModel>> GetAllTranslators()
    {
        return await _unitOfWork.TranslatorModelRepo.GetAllAsync();
    }

    public async Task<IEnumerable<TranslatorModel>> GetTranslatorsByName(string name)
    {
        return await _unitOfWork.TranslatorModelRepo.GetByName(name);
    }
}
