using Dictionary.Domain.Enums;

namespace Dictionary.Domain.Infrastructures
{
    public class ApiReturn<T> : ApiReturn
    {
        public new T Data { get; set; }
    }

    public class ApiReturn
    {
        public ApiStatusCode Code { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public string InternalMessage { get; set; }
        public ApiErrorCollection Errors { get; set; }
        public object Data { get; set; }
    }  

}
