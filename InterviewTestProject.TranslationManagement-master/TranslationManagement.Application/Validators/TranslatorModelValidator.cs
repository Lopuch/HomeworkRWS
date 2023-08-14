using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslationManagement.Application.Models;

namespace TranslationManagement.Application.Validators;
public class TranslatorModelValidator : AbstractValidator<TranslatorModel>
{
    public TranslatorModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.Status)
            .Must(ValidateStatus)
            .WithMessage("unknown status");

        ///RuleFor...
    }

    private bool ValidateStatus(TranslatorModel model, string newStatus)
    {
        if (string.IsNullOrWhiteSpace(newStatus))
        {
            return false;
        }

        var enums = typeof(TranslatorStatuses).GetEnumNames();
        if (enums.Count(name => name == newStatus) == 0)
        {
            return false;
        }

        return true;
    }
}
