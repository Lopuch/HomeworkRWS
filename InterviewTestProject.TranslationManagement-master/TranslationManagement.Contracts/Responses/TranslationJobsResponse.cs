using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslationManagement.Contracts.Responses;
public class TranslationJobsResponse
{
    public required IEnumerable<TranslationJobResponse> Items { get; init; } = Enumerable.Empty<TranslationJobResponse>();
}
