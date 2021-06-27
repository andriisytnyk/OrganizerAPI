using System;

namespace OrganizerAPI.Shared.Exceptions
{
    public class InvalidAuthDataException : Exception
    {
        public InvalidAuthDataException(string message) : base(message)
        {
            
        }
    }
}
