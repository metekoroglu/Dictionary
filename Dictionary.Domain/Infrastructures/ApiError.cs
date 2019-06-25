using Dictionary.Domain.Enums;

namespace Dictionary.Domain.Infrastructures
{
    public class ApiError
    {
        public string Key { get; set; }
        public string Message { get; set; }
        public ApiStatusCode Code { get; set; }
        public string InternalMessage { get; set; }
    }
}
