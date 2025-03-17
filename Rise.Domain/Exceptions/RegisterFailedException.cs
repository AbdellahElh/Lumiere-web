using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rise.Domain.Exceptions;

public class RegisterFailedException : ApplicationException
{
    public RegisterFailedException(string message) : base($"Er ging iets mis bij het registreren : {message}")
    {
    }
}
