﻿using Microsoft.AspNetCore.Mvc.Filters;

namespace eHospitalServer.WebAPI;

public class SerhanAuthorize : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        throw new NotImplementedException();
    }
}
