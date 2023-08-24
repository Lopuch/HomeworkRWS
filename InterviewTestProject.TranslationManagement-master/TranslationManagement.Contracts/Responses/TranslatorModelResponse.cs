using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslationManagement.Contracts.Responses;
public class TranslatorModelResponse
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required decimal HourlyRate { get; init; }
    public required string Status { get; init; }
    public required string CreditCardNumber { get; init; }
}
