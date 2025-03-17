using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rise.Shared.Tenturncards;
public interface ITenturncardService
{
    public Task<List<TenturncardDto>> GetTenturncardsAsync();
    public Task UpdateTenturncardValueAsync(string ActivationCode);
    public Task AddTenturncard(string tenturncardCode);

    public Task EditTenturncardAsync(TenturncardDto tenturncardDto);

}