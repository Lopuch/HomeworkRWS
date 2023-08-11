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

        ///RuleFor...
    }
}
