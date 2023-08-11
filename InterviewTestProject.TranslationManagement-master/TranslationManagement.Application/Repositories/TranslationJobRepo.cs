using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TranslationManagement.Application.Database;
using TranslationManagement.Application.Models;

namespace TranslationManagement.Application.Repositories;

public interface ITranslationJobRepo
{
    void Add(TranslationJob job);
    Task<TranslationJob?> GetByIdAsync(int id);
    Task<IEnumerable<TranslationJob>> GetAllAsync();
}

public class TranslationJobRepo : ITranslationJobRepo
{
    private readonly AppDbContext _context;

    public TranslationJobRepo(AppDbContext context)
    {
        _context = context;
    }

    public void Add(TranslationJob job)
    {
        _context.Add(job);
    }

    public async Task<TranslationJob?> GetByIdAsync(int id)
    {
        var job = await _context.TranslationJobs.FindAsync(id);

        return job;
    }

    public async Task<IEnumerable<TranslationJob>> GetAllAsync()
    {
        return await _context.TranslationJobs.ToListAsync();
    }
}
