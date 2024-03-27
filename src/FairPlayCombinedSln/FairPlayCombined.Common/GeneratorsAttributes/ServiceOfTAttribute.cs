using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Common.GeneratorsAttributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
#pragma warning disable S2326 // Unused type parameters should be removed
#pragma warning disable S2436 // Types and methods should not have too many generic parameters
    public class ServiceOfTAttribute<TCreateMode, TUpdateModel, TListModel, TDbContext,
#pragma warning restore S2436 // Types and methods should not have too many generic parameters

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
