namespace Gym.Client.Extensions
{
    public static class HttpClientExtensions
    {
        public static IServiceCollection AddMainApiClient(this IServiceCollection services, IConfiguration config)
        {
            var baseUrl = config["ApiSettings:BaseUrl"]
                ?? throw new Exception("ApiSettings:BaseUrl is missing");

            services.AddHttpClient("MainApi", client =>
            {
                client.BaseAddress = new Uri(baseUrl);
            });

           
            services.AddScoped(sp =>
                sp.GetRequiredService<IHttpClientFactory>().CreateClient("MainApi"));

            return services;
        }
    }
}
