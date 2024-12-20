﻿using FairPlayCombined.Models.FairPlayBlogs.Blog;
using FairPlayCombined.Models.Pagination;

namespace FairPlayCombined.Interfaces.FairPlayBlogs;

public interface IBlogService
{
    Task<long> CreateBlogAsync(CreateBlogModel createModel,
        CancellationToken cancellationToken);
    Task<BlogModel[]> GetAllBlogAsync(
    CancellationToken cancellationToken);
    Task<BlogModel> GetBlogByIdAsync(long id, CancellationToken cancellationToken);
    Task DeleteBlogByIdAsync(long id, CancellationToken cancellationToken);
    Task<PaginationOfT<BlogModel>> GetPaginatedBlogAsync(
    PaginationRequest paginationRequest, CancellationToken cancellationToken);
    Task<PaginationOfT<BlogModel>> GetAllBlogByUserIdAsync(string userId, PaginationRequest paginationRequest,
    CancellationToken cancellationToken);
}
