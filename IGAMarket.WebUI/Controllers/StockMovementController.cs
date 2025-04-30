using IGAMarket.BusinessLayer.Abstract;
using IGAMarket.EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IGAMarket.WebUI.Controllers
{
    [Authorize]
    public class StockMovementController : Controller
    {
        private readonly IProductService _productService;
        private readonly IStockMovementService _stockMovementService;

        public StockMovementController(IProductService productService, IStockMovementService stockMovementService)
        {
            _productService = productService;
            _stockMovementService = stockMovementService;
        }
        [HttpGet]
        public IActionResult StockMovementIndex()
        {
            var products = _productService.TGetList();
            ViewBag.Products = products.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).ToList();

            return View();
        }


        [HttpPost]
        public IActionResult Add(StockMovement model)
        {
            if (model.Quantity <= 0)
                return BadRequest("Geçerli bir miktar giriniz.");

            var product = _productService.Get(x => x.Id == model.ProductId);
            if (product == null) return NotFound();

            product.StockQuantity += model.Quantity;
            _productService.TUpdate(product);

            model.MovementDate = DateTime.Now;
            model.MovementType = "Stok Girişi";
            _stockMovementService.TInsert(model);

            return RedirectToAction("StockMovementIndex");
        }

    }

}
