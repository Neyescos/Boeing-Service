using AutoMapper;
using BLL.Interfaces;
using BLL.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PL.Interfaces;
using PL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PL.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _service;
        private readonly Mapper _mapper;
        private readonly IPlaneModelService _modelService;
        public UserController(IUserService service, IViewProfile viewProfile, IPlaneModelService planeModel)
        {
            _modelService = planeModel;
            _mapper = viewProfile.GetMapper();
            _service = service;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View("LoginUser");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _service.Login(model.Login, model.Password);
                if (user != null)
                {
                    await Authenticate(_mapper.Map<UserViewModel>(user));
                    return RedirectToAction("Index", "PlaneParts");
                }
                ModelState.AddModelError("", "Incorrect login or password");
            }
            return View("LoginUser");
        }

        [Authorize(Roles = "1")]
        public ActionResult Index()
        {
            var usersDTO = _service.GetUsers();

            var users = _mapper.Map<IEnumerable<UserViewModel>>(usersDTO);
            Console.WriteLine(users);
            return View(users);
        }

        [AllowAnonymous]
        public ActionResult Create()
        {
            return View("CreateUser");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser(UserViewModel userViewModel)
        {
            try
            {
                var user = userViewModel;
                user.Role = 2;
                user.RegistrationDate = DateTime.UtcNow;
                _service.Registration(_mapper.Map<UserDto>(userViewModel));
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }

        }

        [Authorize]
        [HttpGet]
        public ActionResult Details(string id)
        {
            return View("DetailsUser", _mapper.Map<UserViewModel>(_service.GetUsers().FirstOrDefault(u => u.Id == Convert.ToInt32(id))));
        }

        [Authorize(Roles = "1,2")]
        [HttpGet]
        public ActionResult Edit(int id)
        {


            var selectList = new List<SelectListItem>();
            var planes = _mapper.Map<IEnumerable<PlaneModelViewModel>>(_modelService.GetPlaneModels());
            foreach (var item in planes)
            {
                selectList.Add(new SelectListItem() { Text = item.Name, Value = item.Id.ToString() });
            }

            var editViewModel = new EditUserViewModel();

            editViewModel.User = _mapper.Map<UserViewModel>(_service.GetUsers().FirstOrDefault(x => x.Id == id));
            foreach (var plane in selectList)
            {
                foreach (var userPlane in editViewModel.User.PlaneModels)
                {
                    if (Convert.ToInt32(plane.Value) == userPlane.Id)
                    {
                        plane.Selected = true;
                    }

                }
            }
            editViewModel.SelectedIds = selectList;
            return View("EditUser", editViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(List<SelectListItem> SelectedIds, UserViewModel userViewModel)
        {
            
                var planeModels = _mapper.Map<IEnumerable<PlaneModelViewModel>>(_modelService.GetPlaneModels());

                var planes = new List<PlaneModelViewModel>();
                foreach (var selectedModel in SelectedIds)
                {
                    if (selectedModel.Selected)
                    {
                        planes.Add(planeModels.FirstOrDefault(p => p.Id == Convert.ToInt32(selectedModel.Value)));
                    }
                }

                userViewModel.Id = Convert.ToInt32(User.Identity.Name);
                if (userViewModel.Name == null)
                {
                    userViewModel.Name = User.Claims.First(x => x.Type == "UserName").Value;
                }

                userViewModel.PlaneModels = planes;
                _service.UpdateUser(_mapper.Map<UserDto>(userViewModel));
                return RedirectToAction("Details", userViewModel);
           
        }

        [Authorize(Roles = "1")]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            return View("DeleteUser", _mapper.Map<UserViewModel>(_service.GetUser(id)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(UserViewModel userView)
        {
            try
            {
                _service.DeleteUser(_mapper.Map<UserDto>(User));
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<RedirectToActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        private async Task Authenticate(UserViewModel user)
        {
            var claims = new List<Claim>
            {
                new Claim("UserName",user.Name),
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Id.ToString()),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString())
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,

                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(120),

                IsPersistent = true,
                IssuedUtc = DateTime.UtcNow,

                RedirectUri = "/planeparts/"
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id), authProperties);
        }

        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword(int id)
        {
            return View("ChangePassword", _mapper.Map<UserViewModel>(_service.GetUser(id)));
        }

        [HttpPost]
        [Authorize]
        public IActionResult ChangePassword(UserViewModel userViewModel)
        {
            try
            {
                _service.UpdateUser(_mapper.Map<UserDto>(userViewModel));
                return RedirectToAction("Edit", userViewModel.Id);
            }
            catch
            {
                return View();
            }
        }
    }
}
