﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Multilinks.ApiService.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Linq;
using Multilinks.ApiService.Entities;

namespace Multilinks.ApiService.Services
{
   public class EndpointService : IEndpointService
   {
      private readonly ApiServiceDbContext _context;
      private readonly IUserInfoService _userInfoService;

      public EndpointService(ApiServiceDbContext context,
         IUserInfoService userInfoService)
      {
         _context = context;
         _userInfoService = userInfoService;
      }

      public async Task<EndpointViewModel> GetEndpointByIdAsync(Guid id, CancellationToken ct)
      {
         var entity = await _context.Endpoints.SingleOrDefaultAsync(r => r.EndpointId == id, ct);
         if(entity == null) return null;

         return Mapper.Map<EndpointViewModel>(entity);
      }

      public async Task<EndpointViewModel> GetOwnEndpointByNameAsync(string name, CancellationToken ct)
      {
         var entity = await _context.Endpoints.SingleOrDefaultAsync(
            r => (r.CreatorId.ToString() == _userInfoService.UserId && r.Name == name),
            ct);

         if(entity == null)
         {
            entity = new EndpointEntity
            {
               EndpointId = Guid.NewGuid(),
               CreatorId = new Guid(_userInfoService.UserId),
               ClientId = _userInfoService.ClientId,
               ClientType = _userInfoService.ClientType,
               Name = name,
               Description = "No description yet."
            };

            _context.Endpoints.Add(entity);

            var created = await _context.SaveChangesAsync(ct);

            if(created < 1) throw new InvalidOperationException("Could not create new endpoint.");
         }

         return Mapper.Map<EndpointViewModel>(entity);
      }

      public async Task<bool> CheckEndpointExistsAsync(Guid creatorId, string name, CancellationToken ct)
      {
         var entity = await _context.Endpoints.SingleOrDefaultAsync(
            r => (r.CreatorId == creatorId && r.Name == name),
            ct);
         if(entity == null) return false;

         return true;
      }

      public async Task<PagedResults<EndpointViewModel>> GetEndpointsAsync(PagingOptions pagingOptions,
                                                                           SortOptions<EndpointViewModel, EndpointEntity> sortOptions,
                                                                           SearchOptions<EndpointViewModel, EndpointEntity> searchOptions,
                                                                           CancellationToken ct)
      {
         IQueryable<EndpointEntity> query = _context.Endpoints;
         query = searchOptions.Apply(query);
         query = sortOptions.Apply(query);

         var size = await query.CountAsync(ct);

         var items = await query
                .Skip(pagingOptions.Offset.Value)
                .Take(pagingOptions.Limit.Value)
                .ProjectTo<EndpointViewModel>()
                .ToArrayAsync(ct);

         return new PagedResults<EndpointViewModel>
         {
            Items = items,
            TotalSize = size
         };
      }

      public async Task<PagedResults<EndpointViewModel>> GetEndpointsByCreatorIdAsync(Guid creatorId,
                                                                                      PagingOptions pagingOptions,
                                                                                      SortOptions<EndpointViewModel, EndpointEntity> sortOptions,
                                                                                      SearchOptions<EndpointViewModel, EndpointEntity> searchOptions,
                                                                                      CancellationToken ct)
      {
         IQueryable<EndpointEntity> query = _context.Endpoints;
         query = query.Where(ep => ep.CreatorId == creatorId);
         query = searchOptions.Apply(query);
         query = sortOptions.Apply(query);

         var size = await query.CountAsync(ct);

         var items = await query
                .Skip(pagingOptions.Offset.Value)
                .Take(pagingOptions.Limit.Value)
                .ProjectTo<EndpointViewModel>()
                .ToArrayAsync(ct);

         return new PagedResults<EndpointViewModel>
         {
            Items = items,
            TotalSize = size
         };
      }

      public async Task<Guid> CreateEndpointAsync(Guid creatorId,
                                     string name,
                                     string description,
                                     CancellationToken ct)
      {
         var endpointId = Guid.NewGuid();

         var newEndpoint = new EndpointEntity
         {
            EndpointId = endpointId,
            CreatorId = creatorId,
            Name = name,
            Description = description
         };

         _context.Endpoints.Add(newEndpoint);

         var created = await _context.SaveChangesAsync(ct);

         if(created < 1) throw new InvalidOperationException("Could not create new endpoint.");

         return newEndpoint.EndpointId;
      }

      public async Task<bool> DeleteEndpointByIdAsync(Guid endpointId, CancellationToken ct)
      {
         var endpoint = await _context.Endpoints.SingleOrDefaultAsync(ep => ep.EndpointId == endpointId, ct);

         if(endpoint == null) return false;

         _context.Endpoints.Remove(endpoint);

         var deleted = await _context.SaveChangesAsync(ct);

         if(deleted < 1) return false;

         return true;
      }

      public async Task<EndpointViewModel> ReplaceEndpointByIdAsync(Guid endpointId,
                                                                    Guid creatorId,
                                                                    string name,
                                                                    string description,
                                                                    CancellationToken ct)
      {
         var entity = await _context.Endpoints.SingleOrDefaultAsync(r => r.EndpointId == endpointId, ct);

         if(entity == null) return null;

         entity.CreatorId = creatorId;
         entity.Name = name;
         entity.Description = description;

         var replaced = await _context.SaveChangesAsync();

         if(replaced < 1) return null;

         return Mapper.Map<EndpointViewModel>(entity);
      }
   }
}
