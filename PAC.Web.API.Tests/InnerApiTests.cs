using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using PAC.Web.API.Services;
using Xunit;

namespace PAC.Web.API.Tests
{
    /// <summary>
    /// The forwarder against a stand-in for PAC.SageWeb: what the inner
    /// service answers is what the caller gets, and an inner failure is a
    /// gateway problem, never the inner service's own error.
    /// </summary>
    public class InnerApiTests
    {
        [Fact]
        public async Task Body_status_and_content_type_pass_through_untouched()
        {
            const string json = "[{\"DateTendered\":\"2026-07-10T00:00:00\",\"serv_level\":\"PRI\"}]";
            var inner = Stub(req => Json(HttpStatusCode.OK, json));

            var answer = await inner.GetAsync("api/awbs?from=2026-07-01&to=2026-07-31", CancellationToken.None);

            Assert.Equal(200, answer.Status);
            Assert.StartsWith("application/json", answer.ContentType);
            Assert.Equal(json, Encoding.UTF8.GetString(answer.Body));
        }

        [Fact]
        public async Task The_path_and_query_reach_the_inner_service_under_its_base_address()
        {
            Uri? seen = null;
            var inner = Stub(req => { seen = req.RequestUri; return Json(HttpStatusCode.OK, "[]"); });

            await inner.GetAsync("api/customers/C-30417/awbs/open?x=1", CancellationToken.None);

            Assert.Equal("http://10.0.0.6/pactrakapi/api/customers/C-30417/awbs/open?x=1", seen?.ToString());
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.NotFound)]
        [InlineData(HttpStatusCode.NoContent)]
        public async Task Client_errors_and_empty_answers_pass_through(HttpStatusCode code)
        {
            var inner = Stub(req => new HttpResponseMessage(code) { Content = new StringContent("to is before from") });

            var answer = await inner.GetAsync("api/awbs?from=2026-08-01&to=2026-07-01", CancellationToken.None);

            Assert.Equal((int)code, answer.Status);
        }

        [Fact]
        public async Task An_inner_500_becomes_a_502_with_a_plain_problem_body()
        {
            var inner = Stub(req => Json(HttpStatusCode.InternalServerError, "{\"stack\":\"System.Data.OleDb...\"}"));

            var answer = await inner.GetAsync("api/pod?from=2026-07-01&to=2026-07-31", CancellationToken.None);

            Assert.Equal(502, answer.Status);
            Assert.Equal(InnerApi.ProblemContentType, answer.ContentType);
            var body = JsonDocument.Parse(answer.Body).RootElement;
            Assert.Equal(502, body.GetProperty("status").GetInt32());
            Assert.DoesNotContain("OleDb", Encoding.UTF8.GetString(answer.Body));
        }

        [Fact]
        public async Task An_unreachable_inner_service_becomes_a_503()
        {
            var inner = Stub(req => throw new HttpRequestException("No connection could be made"));

            var answer = await inner.GetAsync("api/invoices?from=2026-07-01&to=2026-07-31", CancellationToken.None);

            Assert.Equal(503, answer.Status);
            Assert.Equal(InnerApi.ProblemContentType, answer.ContentType);
        }

        [Fact]
        public async Task A_slow_inner_service_becomes_a_503_after_the_timeout()
        {
            var inner = Stub(async (req, ct) =>
            {
                await Task.Delay(TimeSpan.FromSeconds(5), ct);
                return Json(HttpStatusCode.OK, "[]");
            }, timeout: TimeSpan.FromMilliseconds(200));

            var answer = await inner.GetAsync("api/awbs?from=2026-07-01&to=2026-07-31", CancellationToken.None);

            Assert.Equal(503, answer.Status);
        }

        private static HttpResponseMessage Json(HttpStatusCode code, string json)
            => new(code) { Content = new StringContent(json, Encoding.UTF8, "application/json") };

        private static InnerApi Stub(Func<HttpRequestMessage, HttpResponseMessage> answer, TimeSpan? timeout = null)
            => Stub((req, _) => Task.FromResult(answer(req)), timeout);

        private static InnerApi Stub(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> answer, TimeSpan? timeout = null)
        {
            var http = new HttpClient(new StubHandler(answer))
            {
                BaseAddress = new Uri("http://10.0.0.6/pactrakapi/"),
                Timeout = timeout ?? TimeSpan.FromSeconds(30),
            };
            return new InnerApi(http, NullLogger<InnerApi>.Instance);
        }

        private sealed class StubHandler(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> answer) : HttpMessageHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
                => answer(request, cancellationToken);
        }
    }
}
