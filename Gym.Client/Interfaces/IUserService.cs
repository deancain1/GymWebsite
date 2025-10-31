namespace Gym.Client.Interfaces
{
    public interface IUserService
    {
        Task<int> GetTotalUserAsync();
        Task <int> GetTotalAdminsAsync();
    }
}
