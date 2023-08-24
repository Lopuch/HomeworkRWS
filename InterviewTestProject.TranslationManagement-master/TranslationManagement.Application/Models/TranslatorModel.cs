using Microsoft.EntityFrameworkCore;
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
    [Precision(14,4)]
    public required decimal HourlyRate { get; set; }
    public required string Status { get; set; }
    public required string CreditCardNumber { get; set; }
}