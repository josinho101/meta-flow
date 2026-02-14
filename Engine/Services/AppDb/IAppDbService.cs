using Engine.Models.ViewModels;
using Models;

namespace Engine.Services.AppDb
{
    public interface IAppDbService
    {
        Task<bool> CreateDbAsync(string appName, DbMetadataViewModel viewModel);
        Task<bool> DeleteDbAsync(string appName);
    }
}
