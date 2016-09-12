﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Threading.Tasks;
using IdentityServer4.MongoDB.Entities;
using System.Linq;
using System.Linq.Expressions;

namespace IdentityServer4.MongoDB.Interfaces
{
    public interface IPersistedGrantDbContext //: IDisposable
    {
        IQueryable<PersistedGrant> PersistedGrants { get; }

        Task Add(PersistedGrant entity);

        Task Update(Expression<Func<PersistedGrant, bool>> filter, PersistedGrant entity);

        Task Remove(Expression<Func<PersistedGrant, bool>> filter);

        Task RemoveExpired();
    }
}