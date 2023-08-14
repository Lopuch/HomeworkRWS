using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslationManagement.Contracts.Requests;
public class CreateTranslationJobRequest
{
    public required string CustomerName { get; set; }
    public required string OriginalContent { get; set; }
    public required string TranslatedContent { get; set; }
}
