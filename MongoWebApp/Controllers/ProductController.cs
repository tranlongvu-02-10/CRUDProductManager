using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class ProductController : Controller
{
    private readonly ProductService _service = new ProductService();

    public async Task<IActionResult> Index()
    {
        var products = await _service.GetAllAsync();
        return View(products);
    }
}
