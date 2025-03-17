using Microsoft.EntityFrameworkCore;
using Rise.Domain.Exceptions;
using Rise.Persistence;
using Rise.Services.Auth;
using Rise.Shared.Tenturncards;

namespace Rise.Services.Tenturncards;

public class TenturncardService : ITenturncardService
{
    private readonly ApplicationDbContext dbContext;
    private readonly IAuthContextProvider authContextProvider;

    public TenturncardService(ApplicationDbContext dbContext, IAuthContextProvider authContextProvider)
    {
        if (authContextProvider is null)
            throw new ArgumentNullException($"{nameof(TenturncardService)} requires a {nameof(authContextProvider)}");
        this.dbContext = dbContext;
        this.authContextProvider = authContextProvider;
    }

    public async Task<List<TenturncardDto>> GetTenturncardsAsync()
    {
        var accountId = authContextProvider.GetAccountIdFromMetadata(authContextProvider.User!);

        var tenturncards = await dbContext.Tenturncards
            .Where(t => t.AccountId == accountId)
            .ToListAsync();


        var tenturncardDtos = tenturncards.Select(t => new TenturncardDto
        {
            Id = t.Id,
            AmountLeft = t.AmountLeft,
            PurchaseDate = t.PurchaseDate,
            ExpirationDate = t.ExpirationDate,
            ActivationCode = t.ActivationCode,
        }).ToList();
        Console.WriteLine(tenturncardDtos);

        return tenturncardDtos;
    }
    public async Task AddTenturncard(string tenturncardCode)
    {
        var accountId = authContextProvider.GetAccountIdFromMetadata(authContextProvider.User!);

        var account = await dbContext.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);

        var tenturncard = await dbContext.Tenturncards.FirstOrDefaultAsync(t => t.ActivationCode == tenturncardCode && !t.IsActivated);
        if (tenturncard is null)
        {
            throw new EntityNotFoundException("Tienbeurtenkaart");
        }
        if(tenturncard.AccountId != null)
        {
            throw new EntityAlreadyExistsException("Tienbeurtenkaart");
        }
        tenturncard.AccountId = account!.Id;
        tenturncard.Account = account;
        
        account.Tenturncards.Add(tenturncard);

        dbContext.Accounts.Update(account);
        dbContext.Tenturncards.Update(tenturncard);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateTenturncardValueAsync(string ActivationCode)
    {
        var tenturncard = await dbContext.Tenturncards.FirstOrDefaultAsync(t => t.ActivationCode == ActivationCode) ?? throw new EntityNotFoundException("Tenturncard was not found");
        //If it is the first use of tenturncard (AmountLeft == 10) activate it
        if (tenturncard.AmountLeft == 10)
        {
            tenturncard.IsActivated = true;
            tenturncard.PurchaseDate = DateTime.Now;
            tenturncard.ExpirationDate = DateTime.Now.AddMonths(6);
        }
        // Update tenturncard value
        tenturncard.AmountLeft -= 1;
        dbContext.Tenturncards.Update(tenturncard);
        await dbContext.SaveChangesAsync();
    }

    public async Task EditTenturncardAsync(TenturncardDto tenturncardDto)
    {
        //Find the tenturncard by id
        var tenturncard = await dbContext.Tenturncards.FirstOrDefaultAsync(t => t.Id == tenturncardDto.Id) ?? throw new EntityNotFoundException("Tenturncard was not found");
        // Check if the tenturncard still has value left
        if (tenturncard.AmountLeft == 0)
        {
            throw new ArgumentException("Tienbeurten kaart heeft geen beurten meer over");
        }
        // New value cannot be larger then 10
        if(tenturncardDto.AmountLeft > 10)
        {
            throw new ArgumentException("Tienbeurten kaart kan niet meer beurten dan 10 hebben");
        }
        //If it is the first use of tenturncard (AmountLeft == 10) activate it
        if(tenturncard.AmountLeft == 10)
        {
            await UpdateTenturncardValueAsync(tenturncard.ActivationCode);
        }
        //Update the tenturncard value AmountLeft with the provided new value
        tenturncard.AmountLeft = tenturncardDto.AmountLeft;
        dbContext.Tenturncards.Update(tenturncard);
        await dbContext.SaveChangesAsync();
    }
}
