using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Common.GeneratorsAttributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
#pragma warning disable S2326 // Unused type parameters should be removed
    public class ServiceOfTAttribute<TCreateMode, TUpdateModel, TListModel, TDbContext,

        TDbModel,TPaginationRequest, TPaginationResult> : Attribute 
        where TCreateMode : ICreateModel
        where TUpdateModel : IUpdateModel
        where TListModel : IListModel
        where TDbContext : IDbContext
        where TPaginationRequest : IPaginationRequest
        where TPaginationResult: IPaginationOfT<TListModel>, new()
    {
    }
#pragma warning restore S2326 // Unused type parameters should be removed
}
