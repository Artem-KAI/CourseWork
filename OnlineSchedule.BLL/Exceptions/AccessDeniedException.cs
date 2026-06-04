using System;

namespace BLL.Exceptions;

public class AccessDeniedException : Exception
{
    public AccessDeniedException(string message) : base(message) { }
}
