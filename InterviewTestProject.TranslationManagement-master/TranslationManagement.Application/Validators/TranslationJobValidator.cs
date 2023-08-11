using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslationManagement.Application.Models;
using TranslationManagement.Application.Repositories;

namespace TranslationManagement.Application.Validators;
public class TranslationJobValidator : AbstractValidator<TranslationJob>
{
    private readonly ITranslationJobRepo _translationJobRepo;

    public TranslationJobValidator(ITranslationJobRepo translationJobRepo)
    {
        _translationJobRepo = translationJobRepo;

        RuleFor(x => x.TranslatedContent)
            .NotEmpty();

        RuleFor(x => x.Status)
            .Must(ValidateStatus)
            .WithMessage("invalid status");

        ///RuleFor...
    }

    private bool ValidateStatus(TranslationJob job, string? newStatus)
    {
        if (string.IsNullOrWhiteSpace(newStatus))
        {
            return false;
        }

        var enums = typeof(JobStatuses).GetEnumNames();
        if (enums.Count(name => name == newStatus) == 0)
        {
            return false;
        }

        return true;
    }
}
