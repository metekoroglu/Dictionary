using Dictionary.API.Base;
using Dictionary.API.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Dictionary.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnglishController : BaseApiController
    {
        private readonly ApiSettings _appSettings;
        public EnglishController(IOptions<ApiSettings> appSettings) : base(appSettings)
        {
            _appSettings = appSettings.Value;
        }
    }
}