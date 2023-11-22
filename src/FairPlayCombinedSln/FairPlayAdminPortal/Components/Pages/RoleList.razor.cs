using FairPlayCombined.Common;
using FairPlayCombined.Models.Role;
using Microsoft.AspNetCore.Components.QuickGrid;
using Microsoft.AspNetCore.Identity;
using FairPlayCombined.Models.Pagination;
using FairPlayCombined.Common.GeneratorsAttributes;

namespace FairPlayAdminPortal.Components.Pages
{
    public partial class RoleList
    {
        private CancellationTokenSource cancellationTokenSource = new();
        private GridItemsProvider<RoleModel>? ItemsProvider;
        private PaginationState pagination = new PaginationState()
        {
            ItemsPerPage = Constants.Pagination.PageSize
        };
        protected override void OnInitialized()
        {
            ItemsProvider ??= async req =>
            {
                PaginationRequest paginationRequest = new PaginationRequest()
                {
                    StartIndex = req.StartIndex,
                    SortingItems = req.GetSortByProperties()
                                                .Select(p => new SortingItem()
                                                {
                                                    PropertyName = p.PropertyName,
                                                    SortType = (p.Direction == SortDirection.Ascending) ?
                                                    SortType.Ascending
                                                    :
                                                    SortType.Descending
                                                }).ToArray()
                };
                var paginationResult = await this.roleService!.GetPaginatedRoleListAsync(
                    paginationRequest, this.cancellationTokenSource.Token);
                paginationResult.Items = req.ApplySorting(paginationResult!.Items!.AsQueryable()).ToArray();
                var result = GridItemsProviderResult.From(
                items: paginationResult.Items!,
                totalItemCount: paginationResult!.TotalItems);
                return result;
            };
        }

        private async Task OnDeleteRoleAsync(string? roleId)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var role = await roleManager.FindByIdAsync(roleId!);
            if (role != null)
            {
                var result = await roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    this.toastService.ShowSuccess($"Role '{role.Name}' deleted");
                    await this.pagination.SetCurrentPageIndexAsync(0);
                }
                else
                {
                    var message = String.Join(",", result.Errors.Select(p => p.Description));
                    this.toastService!.ShowError(message);
                }
            }
        }
    }
}
