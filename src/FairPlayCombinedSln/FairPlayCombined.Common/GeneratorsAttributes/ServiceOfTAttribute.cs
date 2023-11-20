﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Common.GeneratorsAttributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
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
}