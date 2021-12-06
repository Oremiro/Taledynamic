using System.Threading.Tasks;

namespace Taledynamic.Core.Interfaces
{
    public interface ITableDataService
    {
        public Task UpdateTableDataAsync();
        public Task CreateTableDataAsync();
        public Task DeleteTableDataAsync();
        public Task ReadTableDataAsync();

    }
}