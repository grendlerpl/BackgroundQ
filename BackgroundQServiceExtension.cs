using Microsoft.Extensions.DependencyInjection;


namespace CodeServiceJD.BackgroundQ
{
    public static class BackgroundQServiceExtension
    {
        public static IServiceCollection AddQService<QMessage,QProcessor>(this IServiceCollection services)
            where QProcessor : class, IBackgroundQProcessor<QMessage>
            {
            services.AddSingleton<IBackgroundQ<QMessage>, BackgroundQ<QMessage>>();
            services.AddSingleton<IBackgroundQProcessor<QMessage>, QProcessor>();
            services.AddHostedService<BackgroundQService<QMessage>>();
            return services;
            }
       
    }

}