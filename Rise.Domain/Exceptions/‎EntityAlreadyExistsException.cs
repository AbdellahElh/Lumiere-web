using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rise.Domain.Exceptions;

public class EntityAlreadyExistsException : ApplicationException
{
    /// <param name="entityName">Name / type of the <see cref="Entity"/>.</param>
    /// <param name="parameterName">Name of the property that is invalid.</param>
    /// <param name="parameterValue">The value that was marked as a duplicate.</param>
    public EntityAlreadyExistsException(string entityName) : base($"{entityName} already exists.")
    {
    }
}
