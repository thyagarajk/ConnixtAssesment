namespace Connixt.Api.Services;

public enum SoapVersion
{
    Soap11,
    Soap12
}

public interface ISoapClient
{
    Task<TResponse> InvokeAsync<TRequest, TResponse>(string action, TRequest request, SoapVersion version = SoapVersion.Soap11);
}
