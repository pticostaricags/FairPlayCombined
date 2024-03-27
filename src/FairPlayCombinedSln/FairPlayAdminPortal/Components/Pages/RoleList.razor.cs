using FairPlayCombined.Common;
using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.Models.Pagination;
using FairPlayCombined.Models.Role;
using Microsoft.AspNetCore.Identity;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FairPlayAdminPortal.Components.Pages
{
    public partial class RoleList : IDisposable
    {
        private readonly CancellationTokenSource cancellationTokenSource = new();
        private GridItemsProvider<RoleModel>? ItemsProvider;
        private readonly PaginationState pagination = new()
        {
            ItemsPerPage = Constants.Pagination.PageSize
        };
        private bool IsBusy { get; set; }

        protected override void OnInitialized()
        {
            ItemsProvider ??= async req =>
            {
                this.IsBusy = true;
                PaginationRequest paginationRequest = new()
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
                paginationResult.Items = [.. req.ApplySorting(paginationResult!.Items!.AsQueryable())];
                var result = GridItemsProviderResult.From(
                items: paginationResult.Items!,
                totalItemCount: paginationResult!.TotalItems);
                this.IsBusy = false;
                return result;
            };
        }

        private async Task OnDeleteRoleAsync(string? roleId)
        {
            this.IsBusy = true;
            StateHasChanged();
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
            this.IsBusy = false;
        }

        private bool isDisposed = false;
        // Dispose() calls Dispose(true)
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed) return;

            if (disposing)
            {
                // free managed resources
                this.cancellationTokenSource.Dispose();
            }

            isDisposed = true;
        }
    }
}
