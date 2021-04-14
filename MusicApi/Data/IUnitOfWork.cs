using System.Threading.Tasks;
using MusicApi.Repositories;

namespace MusicApi.Data
{
    public interface IUnitOfWork
    {
        public ISongRepository Songs { get; set; }
        
        Task Save();
    }
}