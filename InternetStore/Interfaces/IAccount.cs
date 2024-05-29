using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InternetStore.Interfaces
{
    public interface IAccount
    {
        Task<IActionResult> Login();
        Task<IActionResult> Register();
        Task<IActionResult> Logout();
        Task<IActionResult> Profile();
        Task<IActionResult> ChangePassword();
        Task<IActionResult> History();
    }
}
