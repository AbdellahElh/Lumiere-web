using Rise.Domain.Exceptions;
using Rise.Shared.Tenturncards;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rise.Client.MockServices;

public class MockTenturncardService : ITenturncardService
{
    public Task AddTenturncard(string tenturncardCode)
    {
        if(tenturncardCode == "a12345b")
        {
            return Task.CompletedTask;
        }
        else
        {
            throw new EntityNotFoundException("Er was geen tienbeurtenkaart met de code test gevonden");
        }
    }

    public Task EditTenturncardAsync(TenturncardDto tenturncardDto)
    {
        throw new NotImplementedException();
    }

    public Task UpdateTenturncardValueAsync(int id)
    {
        return Task.CompletedTask;
    }

    public Task UpdateTenturncardValueAsync(string ActivationCode)
    {
        throw new NotImplementedException();
    }

    Task<List<TenturncardDto>> ITenturncardService.GetTenturncardsAsync()
    {
        return (Task<List<TenturncardDto>>)Task.CompletedTask;
    }
}
