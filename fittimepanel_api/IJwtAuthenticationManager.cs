using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FittimePanelApi
{
    public interface IJwtAuthenticationManager
    {
        string Authenticate(bool validate, string username);
    }
}
