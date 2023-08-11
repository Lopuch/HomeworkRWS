using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslationManagement.Application.Database;
using TranslationManagement.Application.Models;

namespace TranslationManagement.Application.Repositories;

public interface ITranslatorModelRepo
{
    void Add(TranslatorModel translator);
    Task<TranslatorModel?> GetByIdAsync(int id);
    Task<IEnumerable<TranslatorModel>> GetAllAsync();
}

public class TranslatorModelRepo : ITranslatorModelRepo
{
    private readonly AppDbContext _context;

    public TranslatorModelRepo(AppDbContext context)
    {
        _context = context;
    }

    public void Add(TranslatorModel translator)
    {
        _context.Translators.Add(translator);
    }

    public async Task<TranslatorModel?> GetByIdAsync(int id)
    {
        var translator = await _context.Translators.FindAsync(id);

        return translator;
    }

    public async Task<IEnumerable<TranslatorModel>> GetAllAsync()
    {
        return await _context.Translators.ToListAsync();
    }
}
