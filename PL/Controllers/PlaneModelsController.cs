using AutoMapper;
using BLL.Interfaces;
using BLL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PL.Interfaces;
using PL.Models;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace PL.Controllers
{
    public class PlaneModelsController : Controller
    {
        private readonly IPlaneModelService _service;
        private readonly Mapper _mapper;
        public PlaneModelsController(IPlaneModelService service, IViewProfile viewProfile)
        {
            _mapper = viewProfile.GetMapper();
            _service = service;
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            var list = _mapper.Map<IEnumerable<PlaneModelViewModel>>(_service.GetPlaneModels());
            ViewBag.ListOFModels = list;
            return View(list);
        }

        [Authorize(Roles = "1, 2")]
        public ActionResult Details(int id)
        {
            return View("DetailsPlaneModel", _mapper.Map<PlaneModelViewModel>(_service.GetPlaneModel(id)));
        }
        [Authorize(Roles = "1")]
        public ActionResult Create()
        {
            return View("CreatePlaneModel");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PlaneModelViewModel planeModelViewModel, IFormFile uploadedImage)
        {
            try
            {

                planeModelViewModel.Image = ImageToArray(Image.FromStream(uploadedImage.OpenReadStream()));
                _service.CreatePlaneModel(_mapper.Map<PlaneModelDto>(planeModelViewModel));
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        


        [Authorize(Roles = "1")]
        public ActionResult Edit(int id)
        {
            return View("EditPlaneModel", _mapper.Map<PlaneModelViewModel>(_service.GetPlaneModel(id)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PlaneModelViewModel plane)
        {
            try
            {
                _service.UpdatePlaneModel(_mapper.Map<PlaneModelDto>(plane));
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "1")]
        public ActionResult Delete(int id)
        {
            return View("DeletePlaneModel", _mapper.Map<PlaneModelViewModel>(_service.GetPlaneModel(id)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(PlaneModelViewModel plane)
        {
            try
            {
                _service.DeletePlaneModel(_mapper.Map<PlaneModelDto>(plane));
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        private byte[] ImageToArray(Image source)
        {
            using (Image image = source)
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    return m.ToArray();
                }
            }
        }
    }
}
