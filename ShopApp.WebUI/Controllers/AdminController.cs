using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using ShopApp.Business.Abstract;
using ShopApp.Entity;
using ShopApp.WebUI.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Microsoft.AspNetCore.Authorization;
using ShopApp.WebUI.Extensions;
using Microsoft.AspNetCore.Identity;
using ShopApp.WebUI.Identity;

namespace ShopApp.WebUI.Controllers
{
    [Authorize(Roles = "admin")] // Yetkilendirilmiş kullanıcı olması gerekiyor
    public class AdminController : Controller
    {
        private IProductService _productService;
        private ICategoryService _categoryService;
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<User> _userManager;

        public AdminController(IProductService productService, ICategoryService categoryService, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _productService = productService;
            _categoryService = categoryService;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult UserList()
        {
            var userList = _userManager.Users;
            return View(userList);
        }
        public async Task<IActionResult> UserEdit(string id)
        {

            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var selectedRoles = await _userManager.GetRolesAsync(user);
                var roleList = _roleManager.Roles.Select(i => i.Name);
                ViewBag.Roles = roleList;

                return View(new UserDetailsModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    SelectedRoles = selectedRoles

                });
            }



            return View("~/Admin/User/List");
        }

        [HttpPost]
        public async Task<IActionResult> UserEdit(UserDetailsModel model, string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user != null)
                {

                    user.UserName = model.UserName;
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Email = model.Email;
                    user.EmailConfirmed = model.EmailConfirmed;


                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        var userRoles = await _userManager.GetRolesAsync(user);
                        // Null reference hatası almamk için yazdık.
                        selectedRoles = selectedRoles ?? new string[] { };
                        // Kullanıcının seçmiş olduğu rolü veritabanında yoksa ekliyoruz.
                        await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles).ToArray<string>());
                        await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles).ToArray<string>());
                        return Redirect("/Admin/User/List");
                    }
                    return Redirect("/Admin/User/List");

                }
            }

            return View(model);
        }
        public IActionResult RoleList()
        {
            var roleList = _roleManager.Roles;
            return View(roleList);
        }
        [HttpGet]
        public IActionResult RoleCreate()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RoleCreate(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole()
                {
                    Name = model.Name
                });

                if (result.Succeeded)
                {
                    return RedirectToAction("RoleList");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);

                    }
                }
            }


            return View(model);
        }
        public async Task<IActionResult> RoleEdit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            var members = new List<User>();
            var nonMembers = new List<User>();
            foreach (var user in _userManager.Users.ToList())
            {
                // kullanıcının rolü olanlar members olmayanlar nonMembers listesine referans verilecek.
                var list = await _userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;

                // Hangi listeye referans verildiyse o listeye kullanıcı ekleniyor.
                list.Add(user);
            }

            var model = new RoleDetails()
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            };
            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> RoleEdit(RoleEditModel model)
        {
            if (ModelState.IsValid)
            {
                foreach (var userId in model.IdsToAdd ?? new string[] { })

                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        var result = await _userManager.AddToRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }
                foreach (var userId in model.IdsToDelete ?? new string[] { })
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        var result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }
                return Redirect("/Admin/Role/" + model.RoleId);
            }


            return View(model);
        }
        public IActionResult ProductList()
        {
            return View(new ProductListViewModel()
            {
                Products = _productService.GetAll()
            });
        }

        // Get metodu
        [HttpGet]
        public IActionResult ProductCreate()
        {
            return View();
        }

        // Post metodu
        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductModel model, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var imageUrl = "";
                if (file != null)
                {
                    var extension = Path.GetExtension(file.FileName);// Resmin uzantısını alıyoruz.
                    var randomName = string.Format($"{Guid.NewGuid()}{extension}");// Guid ile benzersiz isim üretiyoruz.
                    imageUrl = randomName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", randomName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }

                var entity = new Product()
                {
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    ImageUrl = imageUrl,
                    Url = model.Url
                };

                if (_productService.Create(entity))
                {
                    TempData.Put("message", new AlertMessage()
                    {
                        Title = "Success",
                        Message = $"The product name {entity.Name} added.",
                        AlertType = "success"
                    });

                    // View kullansaydık ViewData kullanabilirdik.
                    // TempData için mesajı Json'a çevirdik.
                    //Mesaj oluşturmak için metot yazdık.
                    // CreateMessage($"The product name {entity.Name} added.","success");
                    return RedirectToAction("ProductList");
                }
                TempData.Put("message", new AlertMessage()
                {
                    Title = "Danger",
                    Message = _productService.ErrorMessage,
                    AlertType = "danger"
                });
                return View(model);
            }

            return View(model);

        }

        // Get
        [HttpGet]
        public IActionResult ProductEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = _productService.GetByIdWithCategories((int)id);
            if (entity == null)
            {
                return NotFound();
            }

            var model = new ProductModel()
            {
                ProductId = entity.ProductId,
                Name = entity.Name,
                Url = entity.Url,
                Description = entity.Description,
                Price = entity.Price,
                ImageUrl = entity.ImageUrl,
                IsApproved = entity.IsApproved,
                IsHome = entity.IsHome,
                SelectedCategories = entity.ProductCategories.Select(p => p.Category).ToList()
            };

            ViewBag.Categories = _categoryService.GetAll();

            return View(model);
        }

        // Post
        [HttpPost]
        public async Task<IActionResult> ProductEdit(ProductModel model, int[] categoryIds, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var entity = _productService.GetById(model.ProductId);
                if (entity == null)
                {
                    return NotFound();
                }

                entity.ProductId = model.ProductId;
                entity.Name = model.Name;
                entity.Url = model.Url;
                entity.Description = model.Description;
                entity.Price = model.Price;
                entity.IsApproved = model.IsApproved;
                entity.IsHome = model.IsHome;

                if (file != null)
                {
                    var extension = Path.GetExtension(file.FileName);// Resmin uzantısını alıyoruz.
                    var randomName = string.Format($"{Guid.NewGuid()}{extension}");// Guid ile benzersiz isim üretiyoruz.
                    entity.ImageUrl = randomName;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", randomName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }



                if (_productService.Update(entity, categoryIds))
                {
                    TempData.Put("message", new AlertMessage()
                    {
                        Title = "Success",
                        Message = $"The product name {entity.Name} updated.",
                        AlertType = "success"
                    });
                    // View kullansaydık ViewData kullanabilirdik.
                    // TempData için mesajı Json'a çevirdik.
                    //Mesaj oluşturmak için metot yazdık.
                    //CreateMessage($"The product name {entity.Name} updated.", "success");
                    return RedirectToAction("ProductList");
                }
                TempData.Put("message", new AlertMessage()
                {
                    Title = "Danger",
                    Message = _productService.ErrorMessage,
                    AlertType = "danger"
                });
            }
            ViewBag.Categories = _categoryService.GetAll();
            return View(model);
        }

        public IActionResult DeleteProduct(int productId)
        {
            var entity = _productService.GetById(productId);
            if (entity != null)
            {
                _productService.Delete(entity);
            }
            TempData.Put("message", new AlertMessage()
            {
                Title = "Error",
                Message = $"The product name {entity.Name} deleted!",
                AlertType = "danger"
            });

            return RedirectToAction("ProductList");
        }

        public IActionResult CategoryList()
        {
            var categoryList = new CategoryListViewModel() { Categories = _categoryService.GetAll() };
            return View(categoryList);
        }
        // Get metodu
        [HttpGet]
        public IActionResult CategoryCreate()
        {
            return View();
        }
        // Post metodu
        [HttpPost]
        public IActionResult CategoryCreate(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = new Category()
                {
                    Name = model.Name,
                    Url = model.Url
                };
                _categoryService.Create(entity);
                TempData.Put("message", new AlertMessage()
                {
                    Title = "Success",
                    Message = $"The category name {entity.Name} added.",
                    AlertType = "success"
                });

                return RedirectToAction("CategoryList");
            }

            return View(model);

        }

        // Get
        [HttpGet]
        public IActionResult CategoryEdit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = _categoryService.GetByIdWithProducts((int)id);
            if (entity == null)
            {
                return NotFound();
            }

            var model = new CategoryModel()
            {
                CategoryId = entity.CategoryId,
                Name = entity.Name,
                Url = entity.Url,
                Products = entity.ProductCategories.Select(p => p.Product).ToList()
            };

            return View(model);

        }

        // Post metodu
        [HttpPost]
        public IActionResult CategoryEdit(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = _categoryService.GetById(model.CategoryId);
                if (entity == null)
                {
                    return NotFound();
                }

                entity.Name = model.Name;
                entity.Url = model.Url;
                _categoryService.Update(entity);
                TempData.Put("message", new AlertMessage()
                {
                    Title = "Success",
                    Message = $"Category name {entity.Name} modified.",
                    AlertType = "success"
                });

                return RedirectToAction("CategoryList");
            }

            return View(model);


        }

        public IActionResult DeleteCategory(int categoryId)
        {
            var entity = _categoryService.GetById(categoryId);
            if (entity != null)
            {
                _categoryService.Delete(entity);
            }
            TempData.Put("message", new AlertMessage()
            {
                Title = "Danger",
                Message = $"The category name {entity.Name} deleted!",
                AlertType = "danger"
            });
            return RedirectToAction("CategoryList");
        }
        [HttpPost]
        public IActionResult DeleteFromCategory(int productId, int categoryId)
        {
            _categoryService.DeleteFromCategory(productId, categoryId);
            return Redirect("/Admin/Categories/" + categoryId);
        }

        //message => mesajın içeriği, alertType ise uyarı mesajının class ismi. danger, success vb
        private void CreateMessage(string message, string alertType)
        {
            var msj = new AlertMessage()
            {
                AlertType = alertType,
                Message = message
            };
            TempData["message"] = JsonConvert.SerializeObject(msj);
        }




    }
}
