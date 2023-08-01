namespace Application.Common.Interfaces
{
    public interface IApplicationDbContextInitializer
    {
        public Task InitializeAsync();
    }
}
