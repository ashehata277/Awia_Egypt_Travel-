using AutoMapper;
using AwiaEgyptTravel.Application.Features.Orders.Queries;
using AwiaEgyptTravel.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AwiaEgyptTravel.Web.Areas.Admin.Controllers
{
    public class OrdersController : AdminBaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public OrdersController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var query = new GetAllOrdersQuery();
            var response = await _mediator.Send(query);

            if (!response.Success)
            {
                TempData["Error"] = response.Message;
                return View(new List<OrderListViewModel>());
            }

            var viewModel = _mapper.Map<List<OrderListViewModel>>(response.Data);
            return View(viewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var query = new GetOrderByIdQuery { Id = id };
            var response = await _mediator.Send(query);

            if (!response.Success)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<OrderDetailViewModel>(response.Data);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResendEmail(int id)
        {
            var command = new ResendOrderEmailCommand { OrderId = id };
            var response = await _mediator.Send(command);

            if (!response.Success)
            {
                TempData["Error"] = response.Message;
            }
            else
            {
                TempData["Success"] = "Email sent successfully";
            }

            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
