using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslationManagement.Application.Models;
public class TranslatorModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string HourlyRate { get; set; }
    public required string Status { get; set; }
    public required string CreditCardNumber { get; set; }
}