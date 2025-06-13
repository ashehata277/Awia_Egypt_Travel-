using AutoMapper;
using AwiaEgyptTravel.Application.Features.Orders.Commands;
using AwiaEgyptTravel.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AwiaEgyptTravel.Web.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CheckoutController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var cart = HttpContext.Session.Get<List<CartItemViewModel>>("Cart");
            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            var model = new CheckoutViewModel
            {
                CartItems = cart
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(CheckoutViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.CartItems = HttpContext.Session.Get<List<CartItemViewModel>>("Cart");
                return View("Index", model);
            }

            var cart = HttpContext.Session.Get<List<CartItemViewModel>>("Cart");
            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            try
            {
                foreach (var item in cart)
                {
                    var command = new CreateOrderCommand
                    {
                        CustomerName = model.CustomerName,
                        Email = model.Email,
                        Phone = model.Phone,
                        Address = model.Address,
                        City = model.City,
                        Country = model.Country,
                        ZipCode = model.ZipCode,
                        HotelName = model.HotelName,
                        RoomNumber = model.RoomNumber,
                        AdultCount = item.AdultCount,
                        ChildCount = item.ChildCount,
                        InfantCount = item.InfantCount,
                        TourId = item.TourId
                    };

                    var response = await _mediator.Send(command);
                    if (!response.Success)
                    {
                        ModelState.AddModelError("", response.Message);
                        model.CartItems = cart;
                        return View("Index", model);
                    }
                }

                // Clear the cart after successful order
                HttpContext.Session.Remove("Cart");
                TempData["Success"] = "Your order has been placed successfully. You will receive a confirmation email shortly.";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while processing your order. Please try again.");
                model.CartItems = cart;
                return View("Index", model);
            }
        }
    }
}
