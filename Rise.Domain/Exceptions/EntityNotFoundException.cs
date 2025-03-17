using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rise.Domain.Exceptions;
/// <summary>
/// <see cref="Exception"/> to throw when the <see cref="Entity"/> was not found.
/// </summary>
public class EntityNotFoundException : ApplicationException
{
    /// <param name="entityName">Name / type of the <see cref="Entity"/>.</param>
    /// <param name="id">The identifier of the duplicate key.</param>
    public EntityNotFoundException(string entityName) : base($"{entityName} was not found.")
    {
    }
}
