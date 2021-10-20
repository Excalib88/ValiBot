using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ValiBot.Entities;

namespace ValiBot.Services
{
    public class OperationService : IOperationService
    {
        private readonly DataContext _context;

        public OperationService(DataContext context)
        {
            _context = context;
        }

        public async Task<Operation> GetLast(long userId)
        {
            return await _context.Operations.OrderBy(x => x.CreatedAt).LastOrDefaultAsync(x => x.UserId == userId && !x.IsFinished);
        }

        public async Task<List<Operation>> GetOperations(long userId, DateTime byDate)
        {
            return await _context.Operations.OrderBy(x => x.CreatedAt).Where(x => x.CreatedAt >= byDate).ToListAsync();
        }
    }
}