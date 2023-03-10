using System;

namespace Infrastructure.Security.Authentication.Abstract
{
    public interface ICredentialProvider : IDisposable
    {
        string GetEmail { get; }
        string GetPassword { get; }
    }
}
