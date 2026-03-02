using Corporate_Management.DTOs;
using Corporate_Management.Models;

namespace Corporate_Management.Repositories.IRepositories
{
    public interface IUserRepositories
    {
        Task<int> AddUserAsync(RegisterDto userDto);
        Task<int> UpdateUserAsync(int id,RegisterDto userDto);
        Task<int> DeleteUserAsync(int id);
        Task<User> GetUserByIdAsync(int id);
        Task<IEnumerable<UserListDto>> GetAllUserAsync();
        Task<User> LoginUser(LoginDto logindto);
    }
}



