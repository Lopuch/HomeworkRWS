using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslationManagement.Contracts.Requests;
public class CreateTranslatorModelRequest
{
    public required string Name { get; init; }
    public required string HourlyRate { get; init; }
    public required string Status { get; init; }
    public required string CreditCardNumber { get; init; }
}
