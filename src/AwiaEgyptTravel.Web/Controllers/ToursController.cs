using AutoMapper;
using AwiaEgyptTravel.Application.Features.Tours.Queries;
using AwiaEgyptTravel.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AwiaEgyptTravel.Web.Controllers
{
    public class ToursController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ToursController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<IActionResult> Details(int id)
        {
            var query = new GetTourByIdQuery { Id = id };
            var response = await _mediator.Send(query);

            if (!response.Success)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<TourDetailViewModel>(response.Data);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int id, int adultCount, int childCount, int infantCount)
        {
            var query = new GetTourByIdQuery { Id = id };
            var response = await _mediator.Send(query);

            if (!response.Success)
            {
                return Json(new { success = false, message = "Tour not found" });
            }

            var cartItem = new CartItemViewModel
            {
                TourId = response.Data.Id,
                TourName = response.Data.Name,
                Location = response.Data.Location,
                StartTime = response.Data.StartTime,
                AdultPrice = response.Data.AdultPrice,
                ChildPrice = response.Data.ChildPrice,
                InfantPrice = response.Data.InfantPrice,
                Currency = response.Data.Currency,
                AdultCount = adultCount,
                ChildCount = childCount,
                InfantCount = infantCount,
                TotalAmount = (response.Data.AdultPrice * adultCount) +
                             (response.Data.ChildPrice * childCount) +
                             (response.Data.InfantPrice * infantCount)
            };

            var cart = HttpContext.Session.Get<List<CartItemViewModel>>("Cart") ?? new List<CartItemViewModel>();
            cart.Add(cartItem);
            HttpContext.Session.Set("Cart", cart);

            return Json(new { success = true, message = "Tour added to cart successfully" });
        }
    }
}
