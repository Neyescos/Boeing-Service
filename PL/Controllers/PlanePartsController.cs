using AutoMapper;
using BLL.Interfaces;
using BLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PL.Interfaces;
using PL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PL.Controllers
{
    public class PlanePartsController : Controller
    {
        private readonly IPlanePartService _service;
        private readonly IPlaneModelService _modelService;
        private readonly IUserService _userService;
        private readonly Mapper _mapper;
        public PlanePartsController(IPlanePartService service, IViewProfile viewProfile, IPlaneModelService planeModelService, IUserService userService)
        {
            _userService = userService;
            _modelService = planeModelService;
            _mapper = viewProfile.GetMapper();
            _service = service;
        }

        [Authorize(Roles = "1, 2")]
        public ActionResult Index(int id)
        {

            var parts = _mapper.Map<IEnumerable<PlanePartViewModel>>(_service.GetPlaneParts().Skip(id * 8));
            ViewBag.Counter = 1;
            ViewBag.Parts = parts;
            return View(parts);
        }

        [Authorize(Roles = "1, 2")]
        public ActionResult Details(int id)
        {
            var part = _mapper.Map<PlanePartViewModel>(_service.GetPlanePart(id));
            ViewBag.PlaneModel = _mapper.Map<PlaneModelViewModel>(_modelService.GetPlaneModel(part.PlaneModelId));
            return View("DetailsPlanePart", part);
        }

        [Authorize(Roles = "1")]
        public ActionResult Create()
        {
            ViewBag.PlaneModels = _mapper.Map<IEnumerable<PlaneModelViewModel>>(_modelService.GetPlaneModels());
            return View("CreatePlanePart");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PlanePartViewModel planePartViewModel)
        {
            try
            {
                _service.CreatePlanePart(_mapper.Map<PlanePartDto>(planePartViewModel));
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("CreatePlanePart");
            }
        }

        [Authorize(Roles = "1")]
        public ActionResult Edit(int id)
        {
            ViewBag.PlaneModels = _mapper.Map<IEnumerable<PlaneModelViewModel>>(_modelService.GetPlaneModels());
            return View("EditPlanePart", _mapper.Map<PlanePartViewModel>(_service.GetPlanePart(id)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PlanePartViewModel planePartViewModel)
        {
            try
            {
                _service.UpdatePlanePart(_mapper.Map<PlanePartDto>(planePartViewModel));
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("EditPlanePart");
            }
        }

        [Authorize(Roles = "1")]
        public ActionResult Delete(int id)
        {
            ViewBag.PlaneModel = _modelService.GetPlaneModel(_service.GetPlanePart(id).PlaneModelId);
            return View("DeletePlanePart", _mapper.Map<PlanePartViewModel>(_service.GetPlanePart(id)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(PlanePartViewModel planePartViewModel)
        {
            try
            {
                _service.DeletePlanePart(_mapper.Map<PlanePartDto>(planePartViewModel));
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("DeletePlanePart");
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult SortByName()
        {
            var parts = _mapper.Map<IEnumerable<PlanePartViewModel>>(_service.GetPlaneParts().OrderBy(p => p.Name));
            ViewBag.Counter = 1;
            ViewBag.Parts = parts;
            return View("Index", parts);
        }

        [HttpGet]
        [Authorize]
        public IActionResult SortByPrice()
        {
            var parts = _mapper.Map<IEnumerable<PlanePartViewModel>>(_service.GetPlaneParts().OrderBy(p => p.Price));
            ViewBag.Counter = 1;
            ViewBag.Parts = parts;
            return View("Index", parts);
        }

        [HttpGet]
        [Authorize]
        public IActionResult SortByYear()
        {
            var parts = _mapper.Map<IEnumerable<PlanePartViewModel>>(_service.GetPlaneParts().OrderBy(p => p.ManufacturingDate));
            ViewBag.Counter = 1;
            ViewBag.Parts = parts;
            return View("Index", parts);
        }

        [HttpGet]
        [Authorize]
        public IActionResult SortByManufacturer()
        {
            var parts = _mapper.Map<IEnumerable<PlanePartViewModel>>(_service.GetPlaneParts().OrderBy(p => p.Manufacturer));
            ViewBag.Counter = 1;
            ViewBag.Parts = parts;
            return View("Index", parts);
        }

        [HttpGet]
        [Authorize]
        public IActionResult SortByPlaneModel()
        {
            var parts = _mapper.Map<IEnumerable<PlanePartViewModel>>(_service.GetPlaneParts().OrderBy(p => p.PlaneModel.Name));
            ViewBag.Counter = 1;
            ViewBag.Parts = parts;
            return View("Index", parts);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Index(string startDate, string endDate, string endPrice, string searchValue, bool isChecked = false, string startPrice = "0")
        {
            if (searchValue != null)
            {
                var parts = _service.GetPlaneParts().Where(p => (p.Name.ToLower()).Contains(searchValue.ToLower()));
                return View("Index", _mapper.Map<IEnumerable<PlanePartViewModel>>(parts));
            }
            if (endDate == null)
            {
                endDate = DateTime.UtcNow.ToString();
            }
            if (isChecked)
            {
                return IsCheckedAction(startDate, endDate, endPrice, startPrice);
            }
            else if (startDate != null)
            {
                return IsNotCheckedAction(startDate, endDate, endPrice);
            }
            else if (endPrice != null)
            {
                var parts = _service.GetPlaneParts().Where(x => x.Price <= Convert.ToDecimal(endPrice));
                return View("Index", _mapper.Map<IEnumerable<PlanePartViewModel>>(parts));
            }
            return RedirectToAction("Index");
        }

        private IActionResult IsNotCheckedAction(string startDate, string endDate, string endPrice)
        {
            var parts = _service.GetPlaneParts().Where(p => p.ManufacturingDate >= DateTime.Parse(startDate) && p.ManufacturingDate <= DateTime.Parse(endDate));
            if (endPrice != null)
            {
                return View("Index", parts.Where(x => x.Price <= Convert.ToDecimal(endPrice)));
            }
            return View("Index", _mapper.Map<IEnumerable<PlanePartViewModel>>(parts));
        }

        private IActionResult IsCheckedAction(string startDate, string endDate, string endPrice, string startPrice = "0")
        {
            var parts = GetPreferences();
            if (startDate != null)
            {
                var query = parts.Where(p => p.ManufacturingDate >= DateTime.Parse(startDate) && p.ManufacturingDate <= DateTime.Parse(endDate));
                if (endPrice != null)
                {
                    return View("Index", query.Where(x => x.Price <= Convert.ToDecimal(endPrice) && x.Price >= Convert.ToDecimal(startPrice)));
                }
                return View("Index", query);
            }

            if (endPrice != null)
            {
                return View("Index", parts.Where(x => x.Price <= Convert.ToDecimal(endPrice)));
            }
            return View("Index", parts);
        }

        private IEnumerable<PlanePartViewModel> GetPreferences()
        {
            var parts = _service.GetPlaneParts();
            var models = _userService.GetUsers().FirstOrDefault(u => u.Id == Convert.ToInt32(User.Identity.Name))?.PlaneModels;
            var query = from part in parts
                        join model in models
                        on part.PlaneModel.Id equals model.Id
                        select part;
            return _mapper.Map<IEnumerable<PlanePartViewModel>>(query);
        }
    }
}
