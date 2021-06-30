using System;

namespace OrganizerAPI.Shared.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string message) : base(message)
        {
            
        }
    }
}
