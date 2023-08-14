using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslationManagement.Contracts.Responses;
public class TranslatorModelsResponse
{
    public required IEnumerable<TranslatorModelResponse> Items { get; init; } = Enumerable.Empty<TranslatorModelResponse>();
}
