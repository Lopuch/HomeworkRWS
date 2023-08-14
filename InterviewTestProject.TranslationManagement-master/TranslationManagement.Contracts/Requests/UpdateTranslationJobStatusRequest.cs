using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslationManagement.Contracts.Requests;
public class UpdateTranslationJobStatusRequest
{
    public required int JobId { get; init; }
    public required int TranslatorId { get; init; }
    public required string NewStatus { get; init; } = string.Empty;
}
