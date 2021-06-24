﻿using OrganizerAPI.Models.Models;
using OrganizerAPI.Shared.ModelsDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrganizerAPI.Domain.Interfaces
{
    public interface IUserService
    {
        UserAuthResponseDTO Authenticate(UserAuthRequestDTO model, string ipAddress);
        UserAuthResponseDTO UpdateRefreshToken(string token, string ipAddress);
        bool RevokeToken(string token, string ipAddress);
        UserDTO GetCurrentUser(string token);
        int? GetCurrentUserId(string token);
    }
}
