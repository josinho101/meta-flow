using Models;

namespace Engine.Models.ViewModels
{
    public static class DbMetadataExtensions
    {
        public static DbMetadataViewModel ToViewModel(this DbMetadata model) => new DbMetadataViewModel
        {
            DbName = model.DbName,
            Password = model.Password,  
            Username = model.Username
        };

        public static DbMetadata ToDao(this DbMetadataViewModel vm) => new DbMetadata
        {
            DbName = vm.DbName,
            Password = vm.Password,
            Username = vm.Username
        };
    }
}
