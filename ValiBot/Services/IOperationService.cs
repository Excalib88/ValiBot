using System.Threading.Tasks;
using ValiBot.Entities;

namespace ValiBot.Services
{
    public interface IOperationService
    {
        Task<Operation> GetLast(long userId);
    }
}