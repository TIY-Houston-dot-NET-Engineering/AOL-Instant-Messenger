using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

// [Route("/")]
// public class HomeController : Controller
// {
//     public HomeController(){}

//     [HttpGet]
//     public IActionResult Root(string username = "you")
//     {
//         return View("Empty");
//     }
// }


[Route("/")]
[Authorize]
public class HomeController : Controller
{   
    private DB db;
    private IRepository<Room> rooms;
    private IAuthService auth;
    public HomeController(DB db, IRepository<Room> rooms, IAuthService auth){
        this.db = db;
        this.rooms = rooms;
        this.auth = auth;
    }

    // [HttpGet]
    // public IActionResult Root(string username = "you")
    // {
    //     return View("Empty");
    // }


    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Root()
    {
        return View("Empty");  //, db.Rooms.ToList()
    }

   [HttpGet("room/new")]
   public IActionResult CreateRoom() => View("CreateRoom");

   [HttpPost("room/new")]
   [ValidateAntiForgeryToken]
   public IActionResult CreateRoom([FromForm] Room room)
   {
       if(!ModelState.IsValid)
       return View("CreateRoom", room);
       
       db.Rooms.Add(room);
       db.SaveChanges();
       return Redirect("/");
   }

   [HttpGet("room/{id})")]
   public async Task<IActionResult> Room(int id)
   {
       Room item = rooms.Read(id);
       if(item == null) return NotFound();
       return View("Room", item);
   }

   [HttpPost("room/{id}/messages")]
   [ValidateAntiForgeryToken]
   public async Task<IActionResult> CreateMessage([FromForm] Message m, int id)
   {
       m.Room = null;
       string name = (await auth.GetUser(HttpContext)) ?.Email ?? "NOT PROVIDED";
       m.User = new User {Name = name};

       TryValidateModel(m);

       if(ModelState.IsValid){
           db.Messages.Add(m);
           db.SaveChanges();
       }

       return Redirect($"/room/{id}");
   }

   [HttpGet("login")]
   [AllowAnonymous]
   public IActionResult Login() => View("Login");

   [HttpPost("login")]
   [AllowAnonymous]
   [ValidateAntiForgeryToken]
   public async Task<IActionResult> Login([FromForm] LoginVM user)
   {
    string result = await auth.Login(user.Email, user.Password);
    if(result == null) {
        return Redirect("/");
    }
        ModelState.AddModelError("", result);
        return View("Login", user);
   }

   [HttpPost("register")]
   [AllowAnonymous]
   [ValidateAntiForgeryToken]
   public async Task<IActionResult> Register([FromForm] LoginVM user)
   {
       var errors = await auth.Register(user.Email, user.Password);
       if((errors ?? new List<string>()).Count() == 0){
           return Redirect("/");
       } else {
           foreach(var e in errors)
           ModelState.AddModelError("", e);

           return View("Login");  //, user
       }
   }

   [HttpPost("logout")]
   [ValidateAntiForgeryToken]
   public async Task<IActionResult> Logout(){
       await auth.Logout();
       return Redirect("/");
   }
}

public class LoginVM {
    [Required]
    [EmailAddress]
    public string Email {get; set;}
    [Required]
    [DataType(DataType.Password)]
    public string Password {get; set;}
}