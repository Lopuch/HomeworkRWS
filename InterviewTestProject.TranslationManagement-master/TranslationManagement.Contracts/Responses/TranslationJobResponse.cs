using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslationManagement.Contracts.Responses;
public class TranslationJobResponse
{
    public required int Id { get; set; }
    public required string CustomerName { get; set; }
    public required string Status { get; set; }
    public required string OriginalContent { get; set; }
    public required string TranslatedContent { get; set; }
    public required double Price { get; set; }
}
