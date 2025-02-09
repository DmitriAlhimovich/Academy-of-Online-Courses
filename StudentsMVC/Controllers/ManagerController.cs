﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Students.BLL.Classes;
using Students.MVC.ViewModels;
using Students.DAL.Models;
using Students.BLL.Services;
using System;

namespace Students.MVC.Controllers
{
    public class ManagerController : Controller
    {

        private readonly IManagerService _managerService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ManagerController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IManagerService managerService)
        {
            _managerService = managerService;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        #region Отображения менеджеров
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Index()
        {
            var methodologists = await _managerService.GetAllAsync();
            List<ManagerViewModel> models = new();
            ManagerViewModel model;
            foreach (var manager in methodologists)
            {
                model = Mapper.ConvertViewModel<ManagerViewModel, Manager>(manager);
                models.Add(model);
            }
            return View(models);

        }
        #endregion
        #region Отображения подробностей о менеджере 
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Details(int id)
        {
            var manager = await _managerService.GetAsync(id);
            if (manager == null)
            {
                return NotFound();
            }
            ManagerViewModel model = Mapper.ConvertViewModel<ManagerViewModel, Manager>(manager);
            return View(model);

        }
        #endregion
        #region Отображения регистрации менеджера
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }
        #endregion
        #region Регистрация менеджера
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(ManagerViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new() { UserName = model.Email, Email = model.Email, PhoneNumber = model.PhoneNumber };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "manager");
                    var manager = Mapper.ConvertViewModel<Manager, ManagerViewModel>(model);
                    manager.UserId = user.Id;
                    await _managerService.CreateAsync(manager);
                    await _managerService.Save();
                    return Redirect(Request.Headers["Referer"].ToString());
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                        return View(model);
                    }
                }
            }
            return View(model);
        }
        #endregion
        #region Отображения редактирования менеджера
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Edit(int id)
        {
            var manager = await _managerService.GetAsync(id);
            if (manager == null)
            {
                return NotFound();
            }
            var model = Mapper.ConvertViewModel<EditManagerViewModel, Manager>(manager);
            return View(model);
        }
        #endregion
        #region Редактирования менеджера
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,manager")]
        public async Task<IActionResult> Edit(EditManagerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var manager = Mapper.ConvertViewModel<Manager, EditManagerViewModel>(model);
                try
                {
                    await _managerService.Update(manager);
                    await _managerService.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _managerService.ExistsAsync(manager.ManagerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect(Request.Headers["Referer"].ToString());
            }
            return View(model);
        }
        #endregion
        #region Удаления менеджера
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(int ManagerId)
        {
            var manager = await _managerService.GetAsync(ManagerId);
            if (manager == null)
            {
                return NotFound();
            }
            await _managerService.DeleteAsync(ManagerId);
            await _managerService.Save();
            return RedirectToAction("Index");
        }
        #endregion
    }
}
