using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using ShopApp.Business.Abstract;
using ShopApp.Business.Concrete;
using ShopApp.DataAccess.Abstract;
using ShopApp.DataAccess.Concrete.EfCore;
using ShopApp.WebUI.Identity;
using ShopApp.WebUI.EmailServices;
using Microsoft.Extensions.Configuration;

namespace ShopApp.WebUI
{
    public class Startup
    {
        // App settingse ulaşmak için yazıldı.
        private IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }



        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(
                    @"Data Source=ACER\MSSQLSERVER01;Initial Catalog=ShopDb;Integrated Security=true;"));
            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {// Identity ayarları
                // Password
                options.Password.RequireDigit = true; // Parola içinde rakam olacak
                options.Password.RequireLowercase = true; // Parola içinde küçük harf olacak
                options.Password.RequireUppercase = true; // Parola içinde büyük harf olacak
                options.Password.RequiredLength = 6; // Parola uzunluğu minimum 6 karakter
                options.Password.RequireNonAlphanumeric = true;

                // Lockout
                options.Lockout.MaxFailedAccessAttempts = 5; // Kullanıcı 5 kere şifresini yanlış girerse hesap kilitlenir.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Hesap kilitlendikten 5 dk sonra açılır.
                options.Lockout.AllowedForNewUsers = true; // Lockout özelliği aktif edildi.


                //options.User.AllowedUserNameCharacters = "";
                options.User.RequireUniqueEmail = true; // true ise => Aynı mail adresiyle iki kullanıcı oluşamaz.
                options.SignIn.RequireConfirmedEmail = true; //true ise Kullanıcı kayıt olduktan sonra email mesajından hesabını onaylaması gerekiyor. 
                options.SignIn.RequireConfirmedPhoneNumber = false; // Kullanıcı hesabı onayı için telefonuna mesaj gidecek.


            });

            // Cookie ayarları
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login"; // Login sayfası
                options.LogoutPath = "/Account/Logout"; // Logout sayfası
                options.AccessDeniedPath = "/Account/AccessDenied"; // Erişim engellendi sayfası
                options.SlidingExpiration = true; // Her istek yapıldığında 20 dk sıfırlanır. Tekrar 20 dk hak verir. false olursa sadece 20 dk süre vardır.
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // 1 saat sonra tekrar login olmak gerekiyor. Eğer 59. dk'de istek yapılırsa tekrar 1 saat hak veriliyor. 
                options.Cookie = new CookieBuilder
                {
                    HttpOnly = true,
                    Name = ".ShopApp.Security.Cookie", //Cookie ismi tanımladık
                    SameSite = SameSiteMode.Strict //Csrf Token
                };
            });

            services.AddScoped<ICategoryRepository, EfCoreCategoryRepository>();
            services.AddScoped<ICategoryService, CategoryManager>();

            services.AddScoped<IProductRepository, EfCoreProductRepository>();
            services.AddScoped<IProductService, ProductManager>();
            
            services.AddScoped<ICartRepository,EfCoreCartRepository>();
            services.AddScoped<ICartService,CartManager>();

            services.AddScoped<IEmailSender, SmtpEmailSender>(i =>
            new SmtpEmailSender(
                _configuration["EmailSender:Host"],
                // GetValue<int> ile gelen değeri int türüne çevirdik
                _configuration.GetValue<int>("EmailSender:Port"),
                _configuration.GetValue<bool>("EmailSender:EnableSSL"),
                _configuration["EmailSender:UserName"],
                _configuration["EmailSender:Password"])


            );

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // configuration, userManager ve roleManager sonradan eklendi.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,IConfiguration configuration,UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "node_modules")),
                RequestPath = "/modules"
            });
            if (env.IsDevelopment())
            {
                SeedDatabase.Seed();
                app.UseDeveloperExceptionPage();

            }

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>

            {
                 endpoints.MapControllerRoute(
                    name: "Cart",
                    pattern: "Cart",
                    defaults: new { controller = "Cart", action = "Index" }
                );

                endpoints.MapControllerRoute(
                    name: "AdminUserList",
                    pattern: "Admin/User/List",
                    defaults: new { controller = "Admin", action = "UserList" }
                );

                endpoints.MapControllerRoute(
                    name: "AdminUserEdit",
                    pattern: "Admin/User/{id?}",
                    defaults: new { controller = "Admin", action = "UserEdit" }
                );

                endpoints.MapControllerRoute(
                   name: "AdminRoleList",
                   pattern: "Admin/Role/List",
                   defaults: new { controller = "Admin", action = "RoleList" }
               );

                endpoints.MapControllerRoute(
                    name: "AdminRoleCreate",
                    pattern: "Admin/Role/Create",
                    defaults: new { controller = "Admin", action = "RoleCreate" }
                );

                endpoints.MapControllerRoute(
                    name: "AdminRoleEdit",
                    pattern: "Admin/Role/{id?}",
                    defaults: new { controller = "Admin", action = "RoleEdit" }
                );

                endpoints.MapControllerRoute(
                    name: "AdminCategoryList",
                    pattern: "Admin/Categories",
                    defaults: new { controller = "Admin", action = "CategoryList" }
                );

                endpoints.MapControllerRoute(
                    name: "AdminCategoryCreate",
                    pattern: "Admin/Categories/Create",
                    defaults: new { controller = "Admin", action = "CategoryCreate" }
                );

                endpoints.MapControllerRoute(
                    name: "AdminCategoryEdit",
                    pattern: "Admin/Categories/{id?}",
                    defaults: new { controller = "Admin", action = "CategoryEdit" }
                );

                endpoints.MapControllerRoute(
                    name: "AdminProductList",
                    pattern: "Admin/Products",
                    defaults: new { controller = "Admin", action = "ProductList" }
                );

                endpoints.MapControllerRoute(
                    name: "AdminProductCreate",
                    pattern: "Admin/Products/Create",
                    defaults: new { controller = "Admin", action = "ProductCreate" }
                );

                endpoints.MapControllerRoute(
                    name: "AdminProductEdit",
                    pattern: "Admin/Products/{id?}",
                    defaults: new { controller = "Admin", action = "ProductEdit" }
                );

                endpoints.MapControllerRoute(
                    name: "Search",
                    pattern: "Search",
                    defaults: new { controller = "Shop", action = "Search" }
                );

                endpoints.MapControllerRoute(
                    name: "productdetails",
                    pattern: "{url}",
                    defaults: new { controller = "Shop", action = "Details" }
            );

                endpoints.MapControllerRoute(
                    name: "products",
                    pattern: "Products/{category?}",
                    defaults: new { controller = "Shop", action = "List" }
            );

                endpoints.MapControllerRoute
                (name: "default",
                 pattern: "{controller=Home}/{action=Index}/{id?}"
                );
            });
            SeedIdentity.Seed(userManager,roleManager, configuration).Wait();
        }
    }
}
