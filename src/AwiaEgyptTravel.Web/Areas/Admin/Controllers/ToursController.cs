using AutoMapper;
using AwiaEgyptTravel.Application.Features.Tours.Commands;
using AwiaEgyptTravel.Application.Features.Tours.Queries;
using AwiaEgyptTravel.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AwiaEgyptTravel.Web.Areas.Admin.Controllers
{
    public class ToursController : AdminBaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ToursController(IMediator mediator, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _mediator = mediator;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var query = new GetToursQuery { OnlyAvailable = false };
            var response = await _mediator.Send(query);

            if (!response.Success)
            {
                TempData["Error"] = response.Message;
                return View(new List<TourListViewModel>());
            }

            var viewModel = _mapper.Map<List<TourListViewModel>>(response.Data);
            return View(viewModel);
        }

        public IActionResult Create()
        {
            return View(new CreateTourViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTourViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var photoUrls = new List<string>();
                if (model.Photos != null && model.Photos.Any())
                {
                    foreach (var photo in model.Photos)
                    {
                        if (photo.Length > 0)
                        {
                            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(photo.FileName)}";
                            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "tours");
                            Directory.CreateDirectory(uploadsFolder);
                            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await photo.CopyToAsync(fileStream);
                            }

                            photoUrls.Add($"/uploads/tours/{uniqueFileName}");
                        }
                    }
                }

                var command = new CreateTourCommand
                {
                    Name = model.Name,
                    Description = model.Description,
                    StartTime = model.StartTime,
                    Duration = model.Duration,
                    Location = model.Location,
                    AdultPrice = model.AdultPrice,
                    ChildPrice = model.ChildPrice,
                    InfantPrice = model.InfantPrice,
                    Currency = model.Currency,
                    IsAvailable = model.IsAvailable,
                    Category = model.Category,
                    PhotoUrls = photoUrls
                };

                var response = await _mediator.Send(command);
                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Message);
                    return View(model);
                }

                TempData["Success"] = "Tour created successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while creating the tour.");
                return View(model);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var query = new GetTourByIdQuery { Id = id };
            var response = await _mediator.Send(query);

            if (!response.Success)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<CreateTourViewModel>(response.Data);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateTourViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var photoUrls = new List<string>();
                if (model.Photos != null && model.Photos.Any())
                {
                    foreach (var photo in model.Photos)
                    {
                        if (photo.Length > 0)
                        {
                            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(photo.FileName)}";
                            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "tours");
                            Directory.CreateDirectory(uploadsFolder);
                            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await photo.CopyToAsync(fileStream);
                            }

                            photoUrls.Add($"/uploads/tours/{uniqueFileName}");
                        }
                    }
                }

                var command = new UpdateTourCommand
                {
                    Id = id,
                    Name = model.Name,
                    Description = model.Description,
                    StartTime = model.StartTime,
                    Duration = model.Duration,
                    Location = model.Location,
                    AdultPrice = model.AdultPrice,
                    ChildPrice = model.ChildPrice,
                    InfantPrice = model.InfantPrice,
                    Currency = model.Currency,
                    IsAvailable = model.IsAvailable,
                    Category = model.Category,
                    PhotoUrls = photoUrls
                };

                var response = await _mediator.Send(command);
                if (!response.Success)
                {
                    ModelState.AddModelError("", response.Message);
                    return View(model);
                }

                TempData["Success"] = "Tour updated successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the tour.");
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteTourCommand { Id = id };
            var response = await _mediator.Send(command);

            if (!response.Success)
            {
                TempData["Error"] = response.Message;
            }
            else
            {
                TempData["Success"] = "Tour deleted successfully";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
