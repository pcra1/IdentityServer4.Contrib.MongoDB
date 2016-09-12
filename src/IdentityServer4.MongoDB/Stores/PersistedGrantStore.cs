﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.MongoDB.Interfaces;
using IdentityServer4.MongoDB.Mappers;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace IdentityServer4.MongoDB.Stores
{
    public class PersistedGrantStore : IPersistedGrantStore
    {
        private readonly IPersistedGrantDbContext _context;
        private readonly ILogger _logger;

        public PersistedGrantStore(IPersistedGrantDbContext context, ILogger<PersistedGrantStore> logger)
        {
            _context = context;
            _logger = logger;
        }

        public Task StoreAsync(PersistedGrant token)
        {
            try
            {
                var existing = _context.PersistedGrants.SingleOrDefault(x => x.Key == token.Key);
                if (existing == null)
                {
                    var persistedGrant = token.ToEntity();
                    _context.Add(persistedGrant);
                }
                else
                {
                    token.UpdateEntity(existing);
                    _context.Update(x => x.Key == token.Key, existing);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "StoreAsync");
            }

            return Task.FromResult(0);
        }

        public Task<PersistedGrant> GetAsync(string key)
        {
            var persistedGrant = _context.PersistedGrants.FirstOrDefault(x => x.Key == key);
            var model = persistedGrant.ToModel();

            return Task.FromResult(model);
        }

        public Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId)
        {
            var persistedGrants = _context.PersistedGrants.Where(x => x.SubjectId == subjectId).ToList();
            var model = persistedGrants.Select(x => x.ToModel());

            return Task.FromResult(model);
        }

        public Task RemoveAsync(string key)
        {
            _context.Remove(x => x.Key == key);

            return Task.FromResult(0);
        }

        public Task RemoveAllAsync(string subjectId, string clientId)
        {
            _context.Remove(x => x.SubjectId == subjectId && x.ClientId == clientId);

            return Task.FromResult(0);
        }

        public Task RemoveAllAsync(string subjectId, string clientId, string type)
        {
            _context.Remove(
               x =>
               x.SubjectId == subjectId &&
               x.ClientId == clientId &&
               x.Type == type);

            return Task.FromResult(0);
        }
    }
}