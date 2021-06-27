using System;

namespace OrganizerAPI.Shared.Exceptions
{
    public class NotActiveTokenException : Exception
    {
        public NotActiveTokenException(string message) : base(message)
        {
            
        }
    }
}
