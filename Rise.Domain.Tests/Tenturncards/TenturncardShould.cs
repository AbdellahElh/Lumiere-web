using Rise.Domain.Tenturncards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rise.Domain.Tests.Tenturncards;

public class TenturncardShould
{
    [Fact]
    public void BeCreated()
    {
        Tenturncard card = new() { ActivationCode = "test" };
    }

}