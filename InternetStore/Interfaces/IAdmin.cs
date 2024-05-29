using Microsoft.AspNetCore.Mvc;

namespace InternetStore.Interfaces
{
    public interface IAdmin
    {
        Task<IActionResult> ManageAccounts();
        Task<IActionResult> ManageProducts();
        Task<IActionResult> AddAccount();
        Task<IActionResult> AddProduct();
        Task<IActionResult> EditAccount(string Id);
        Task<IActionResult> EditProduct(int Id);
        Task<IActionResult> DeleteAccount(string id);
        Task<IActionResult> DeleteProduct(int id);
    }
}
