using Corporate_Management.DTOs;
using Corporate_Management.Models;

namespace Corporate_Management.Repositories.IRepositories
{
    public interface IUserRepositories
    {
        Task<int> AddUserAsync(RegisterDto userDto);
        Task<int> UpdateUserAsync(UpdateUserDto userDto);
        Task<int> DeleteUserAsync(int id);
        Task<UserListDto> GetUserByIdAsync(int id);
        Task<IEnumerable<UserListDto>> GetManagerTeam(int managerId);
        Task<IEnumerable<UserListDto>> GetAllUserAsync();
        Task<IEnumerable<UserListDto>> GetAllEmployeeManagers();
        Task<IEnumerable<UserListDto>> GetAllEmployeeAsync();
        Task<IEnumerable<UserListDto>> GetAllManagerAsync();
        Task<User> LoginUser(LoginDto logindto);
        Task LogoutUser(int userId);
        Task<IEnumerable<UserListDto>> GetEmployeeByDepartment(string department);
        Task<bool> AssignManagerAsync(int userId, int managerId);

        //-------------------------------notification section-----------------------------------------
        Task CreateNotification(string title, string message, string type, List<int> userIds);
        Task<IEnumerable<dynamic>> GetUserNotifications(int userId);
        Task MarkAsRead(int notificationId, int userId);

    }
}



