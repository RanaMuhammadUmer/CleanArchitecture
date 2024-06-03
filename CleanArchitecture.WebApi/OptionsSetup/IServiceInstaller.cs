namespace CleanArchitecture.WebApi.OptionsSetup
{
    public interface IServiceInstaller
    {
        void Install(IServiceCollection services, IConfiguration configuration);
    }
}
