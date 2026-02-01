using Models;

namespace Engine.Models.ViewModels
{
    public static class AppExtensions
    {
        public static AppViewModel ToViewModel(this App model) => new AppViewModel
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,
            CreatedDate = model.CreatedDate,
            UpdatedDate = model.UpdatedDate
        };

        public static App ToDao(this AppViewModel vm) => new App
        {
            Id = vm.Id,
            Name = vm.Name,
            Description = vm.Description,
            CreatedDate = vm.CreatedDate,
            UpdatedDate = vm.UpdatedDate
        };
    }
}
