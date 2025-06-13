using Microsoft.AspNetCore.Mvc;
using AwiaEgyptTravel.Web.Models;

namespace AwiaEgyptTravel.Web.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            var cart = HttpContext.Session.Get<List<CartItemViewModel>>("Cart") ?? new List<CartItemViewModel>();
            return View(cart);
        }

        [HttpPost]
        public IActionResult RemoveItem(int tourId)
        {
            var cart = HttpContext.Session.Get<List<CartItemViewModel>>("Cart") ?? new List<CartItemViewModel>();
            var item = cart.FirstOrDefault(x => x.TourId == tourId);
            
            if (item != null)
            {
                cart.Remove(item);
                HttpContext.Session.Set("Cart", cart);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult UpdateQuantities(List<CartItemViewModel> items)
        {
            if (items != null && items.Any())
            {
                var cart = HttpContext.Session.Get<List<CartItemViewModel>>("Cart") ?? new List<CartItemViewModel>();
                
                foreach (var item in items)
                {
                    var cartItem = cart.FirstOrDefault(x => x.TourId == item.TourId);
                    if (cartItem != null)
                    {
                        cartItem.AdultCount = item.AdultCount;
                        cartItem.ChildCount = item.ChildCount;
                        cartItem.InfantCount = item.InfantCount;
                        cartItem.TotalAmount = (cartItem.AdultPrice * item.AdultCount) +
                                            (cartItem.ChildPrice * item.ChildCount) +
                                            (cartItem.InfantPrice * item.InfantCount);
                    }
                }

                HttpContext.Session.Set("Cart", cart);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
