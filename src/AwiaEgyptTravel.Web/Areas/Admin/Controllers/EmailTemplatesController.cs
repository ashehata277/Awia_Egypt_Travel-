using AutoMapper;
using AwiaEgyptTravel.Application.Features.EmailTemplates.Commands;
using AwiaEgyptTravel.Application.Features.EmailTemplates.Queries;
using AwiaEgyptTravel.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AwiaEgyptTravel.Web.Areas.Admin.Controllers
{
    public class EmailTemplatesController : AdminBaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public EmailTemplatesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var query = new GetEmailTemplatesQuery();
            var response = await _mediator.Send(query);

            if (!response.Success)
            {
                TempData["Error"] = response.Message;
                return View(new List<EmailTemplateViewModel>());
            }

            var viewModel = _mapper.Map<List<EmailTemplateViewModel>>(response.Data);
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var query = new GetEmailTemplateByIdQuery { Id = id };
            var response = await _mediator.Send(query);

            if (!response.Success)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<EmailTemplateViewModel>(response.Data);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmailTemplateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var command = new UpdateEmailTemplateCommand
            {
                Id = model.Id,
                Subject = model.Subject,
                Body = model.Body,
                IsDefault = model.IsDefault
            };

            var response = await _mediator.Send(command);
            if (!response.Success)
            {
                ModelState.AddModelError("", response.Message);
                return View(model);
            }

            TempData["Success"] = "Email template updated successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}
