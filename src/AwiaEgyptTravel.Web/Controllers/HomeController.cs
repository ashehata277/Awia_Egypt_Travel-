using AutoMapper;
using AwiaEgyptTravel.Application.Common.Models;
using AwiaEgyptTravel.Application.Features.Tours.Queries;
using AwiaEgyptTravel.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AwiaEgyptTravel.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public HomeController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var query = new GetToursQuery { OnlyAvailable = true };
            var response = await _mediator.Send(query);

            if (!response.Success)
            {
                TempData["Error"] = response.Message;
                return View(new List<TourListViewModel>());
            }

            var viewModel = _mapper.Map<List<TourListViewModel>>(response.Data);
            return View(viewModel);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contact(ContactViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // TODO: Implement contact form submission
            TempData["Success"] = "Thank you for your message. We'll get back to you soon.";
            return RedirectToAction(nameof(Contact));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
