using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Application.Common.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException(string identifier) : base($"No user found with identifier: {identifier}") { }
}

