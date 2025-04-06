using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGAMarket.Services;

public class PosServices
{
    private readonly TokenService _tokenService;
    private readonly IConfiguration _configuration;

    public PosServices(TokenService tokenService, IConfiguration configuration)
    {
        _tokenService = tokenService;
        _configuration = configuration;
    }


}
