using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslationManagement.Application.Models;
public class TranslationJob
{
    public int Id { get; set; }
    public required string CustomerName { get; set; }
    public string? Status { get; set; }
    public required string OriginalContent { get; set; }
    public required string TranslatedContent { get; set; }
    public double Price { get; set; }
}