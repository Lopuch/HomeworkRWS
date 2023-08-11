using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslationManagement.Application.Models;
using TranslationManagement.Application.Repositories;

namespace TranslationManagement.Application.Database;

public interface IUnitOfWork
{
    ITranslationJobRepo TranslationJobRepo { get; }
    ITranslatorModelRepo TranslatorModelRepo { get; }
    Task<bool> Complete();
}

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public ITranslationJobRepo TranslationJobRepo => new TranslationJobRepo(_context);
    public ITranslatorModelRepo TranslatorModelRepo => new TranslatorModelRepo(_context);

    public UnitOfWork(AppDbContext appDbContext)
    {
        _context = appDbContext;
    }

    public async Task<bool> Complete()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}
