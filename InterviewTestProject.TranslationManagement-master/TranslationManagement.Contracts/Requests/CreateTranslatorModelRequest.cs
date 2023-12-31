﻿namespace TranslationManagement.Contracts.Requests;
public class CreateTranslatorModelRequest
{
    public required string Name { get; init; }
    public required decimal HourlyRate { get; init; }
    public required string Status { get; init; }
    public required string CreditCardNumber { get; init; }
}
