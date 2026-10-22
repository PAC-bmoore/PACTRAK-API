using System.Text.Json;

namespace PAC.Web.API.Services
{
    /// <summary>
    /// Forwards a read to PAC.SageWeb (WebSettings:ApiBaseUrl) and hands
    /// back exactly what it answered: status, content type and body bytes.
    /// Nothing is deserialized on the way through, so the shape the inner
    /// service produces is the shape the caller receives. An inner failure
    /// or an unreachable inner service becomes a plain problem response
    /// with a gateway status, never the inner service's own error.
    /// </summary>
    public sealed class InnerApi(HttpClient http, ILogger<InnerApi> logger)
    {
        public const string ProblemContentType = "application/problem+json";

        public sealed record Answer(int Status, string ContentType, byte[] Body);

        public async Task<Answer> GetAsync(string pathAndQuery, CancellationToken ct)
        {
            try
            {
                using var response = await http.GetAsync(pathAndQuery, HttpCompletionOption.ResponseHeadersRead, ct);
                var status = (int)response.StatusCode;
                if (status >= 500)
                {
                    logger.LogError("PAC.SageWeb answered {Status} for {Path}", status, pathAndQuery);
                    return Problem(StatusCodes.Status502BadGateway, "The operations service did not answer.");
                }
                var body = await response.Content.ReadAsByteArrayAsync(ct);
                var contentType = response.Content.Headers.ContentType?.ToString() ?? "application/json";
                return new Answer(status, contentType, body);
            }
            catch (OperationCanceledException) when (!ct.IsCancellationRequested)
            {
                // the client's timeout, not the caller going away
                logger.LogError("PAC.SageWeb did not answer {Path} within {Timeout}", pathAndQuery, http.Timeout);
                return Problem(StatusCodes.Status503ServiceUnavailable, "The operations service did not answer in time.");
            }
            catch (HttpRequestException ex)
            {
                logger.LogError(ex, "PAC.SageWeb could not be reached for {Path}", pathAndQuery);
                return Problem(StatusCodes.Status503ServiceUnavailable, "The operations service could not be reached.");
            }
        }

        private static Answer Problem(int status, string title)
            => new(status, ProblemContentType, JsonSerializer.SerializeToUtf8Bytes(new { title, status }));
    }
}
