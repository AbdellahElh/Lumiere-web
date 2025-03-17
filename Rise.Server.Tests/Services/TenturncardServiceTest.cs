using Auth0.ManagementApi.Models;
using Bunit.Asserting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rise.Domain.Exceptions;
using Rise.Services.Auth;
using Rise.Shared.Accounts;
using Rise.Shared.Tenturncards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Rise.Server.Tests.Services;

public class TenturncardServiceTest : IntegrationTest
{
    private readonly ITenturncardService _tenturncardService;
    private readonly RegisterDto _testUser;
    private TenturncardDto _cardDto;

    public TenturncardServiceTest(CustomWebApplicationFactory fixture)
    : base(fixture)
    {
        _tenturncardService = fixture.Services.GetRequiredService<ITenturncardService>();
        _testUser = new RegisterDto
        {
            Email = "Test@testUser.be",
            Password = "Test1234!",
        };
        _cardDto = new TenturncardDto
        {
            Id = 1,
            AmountLeft = 7,
            PurchaseDate = DateTime.Now,
            ExpirationDate = DateTime.Now.AddYears(1)
        };
    }


    [Fact]
    public async Task AddTenturncard_card_not_found()
    {
        //Act
        //Since it is an async method , we use Func<Task> instead of Action
        Func<Task> act = () => _tenturncardService.AddTenturncard("notExistingCode");
        //Assert
        EntityNotFoundException exception = await Assert.ThrowsAsync<EntityNotFoundException>(act);
        //The thrown exception can be used for even more detailed assertions.
        Assert.Equal("Tienbeurtenkaart was not found.", exception.Message);
        Assert.IsType<EntityNotFoundException>(exception);
    }
    [Theory]
    [InlineData("GBTA789100")]
    public async Task AddTenturncard_card_already_belongsToSomeone(string tenturncardCode)
    {
        // Arrange
        var card = _dbContext.Tenturncards.FirstOrDefault(c => c.ActivationCode == tenturncardCode);
        card.AccountId = 1;
        _dbContext.Tenturncards.Update(card);
        await _dbContext.SaveChangesAsync();

        //Act
        //Since it is an async method , we use Func<Task> instead of Action
        Func<Task> act = () => _tenturncardService.AddTenturncard(tenturncardCode);
        //Assert
        EntityAlreadyExistsException exception = await Assert.ThrowsAsync<EntityAlreadyExistsException>(act);
        //The thrown exception can be used for even more detailed assertions.
        Assert.Equal("Tienbeurtenkaart already exists.", exception.Message);
        Assert.IsType<EntityAlreadyExistsException>(exception);
    }

    [Fact]
    public async Task EditTenturncard_Succes()
    {
        //Act
        await _tenturncardService.EditTenturncardAsync(_cardDto);

        //Assert
        var updatedCard = _dbContext.Tenturncards.FirstOrDefault(tc => tc.Id == _cardDto.Id);
        Assert.Equal(7, updatedCard.AmountLeft);
        //The card goes from 10 -> 7 meaning that it is the first use so it has to get activated
        Assert.True(updatedCard.IsActivated);
    }


    [Fact]
    public async Task EditTenturncard_AmountLeft_Is_null_Error()
    {
        // Arrange
        var updatedCard = _dbContext.Tenturncards.FirstOrDefault(tc => tc.Id == _cardDto.Id);
        updatedCard.AmountLeft = 0;
        _dbContext.Tenturncards.Update(updatedCard);
        await _dbContext.SaveChangesAsync();

        //Act
        Func<Task> act = () => _tenturncardService.EditTenturncardAsync(_cardDto);

        //Assert
        ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(act);
        Assert.Equal("Tienbeurten kaart heeft geen beurten meer over", exception.Message);
        Assert.IsType<ArgumentException>(exception);
    }

    [Fact]
    public async Task EditTenturncard_AmountLeft_Is_Larger_Then_ten_Error()
    {
        //Arrange
        _cardDto.AmountLeft = 11;

        //Act
        Func<Task> act = () => _tenturncardService.EditTenturncardAsync(_cardDto);


        //Assert
        ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(act);
        Assert.Equal("Tienbeurten kaart kan niet meer beurten dan 10 hebben", exception.Message);
        Assert.IsType<ArgumentException>(exception);
    }
}