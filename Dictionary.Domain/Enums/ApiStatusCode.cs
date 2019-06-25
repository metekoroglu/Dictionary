namespace Dictionary.Domain.Enums
{
    public enum ApiStatusCode
    {
        Unknown = 0,
        Success = 200,
        BadRequest = 400,
        Unauthorized = 401,
        Forbidden = 403,
        NotFound = 404,
        InternalServerError = 500,
        NotImplemented = 501,
        BadGateway = 502
    }
}
