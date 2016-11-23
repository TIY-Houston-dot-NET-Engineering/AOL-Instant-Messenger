using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

[Route("/")]
[Authorize]
public class HomeController : Controller
{
    private DB db;
    private IRepository<Room> room;
    private IAuthService auth;
    public HomeController(DB db, IRepository<Room> room, IAuthService auth){
        this.db = db;
        this.room = room;
        this.auth = auth;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Root(){
        // ViewData["User"] = await auth.GetUser(HttpContext);
        // (await auth.GetUser(HttpContext)).Log();
        return View("Index", db.Rooms.ToList());
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

    [HttpGet("room/{id}")]
    public async Task<IActionResult> Room(int id)
    {
        Room item = room.Read(id);
        if(item == null) return NotFound();
        return View("Room", item);
    }

    [HttpPost("room/{id}/messages")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateMessage([FromForm] Message m, int id){
        m.Room = null;
        string name = (await auth.GetUser(HttpContext))?.Email ?? "NOT PROVIDED";
        m.User = new User { Name = name };
        
        TryValidateModel(m);
        
        if(ModelState.IsValid){
            db.Messages.Add(m);
            db.SaveChanges();
        }

        return Redirect($"/room/{id}");
    }

    [HttpGet("login")]
    [AllowAnonymous]
    public IActionResult Login() => StatusCode(403);




    [HttpPost("login")]
    [AllowAnonymous]
    // [ValidateAntiForgeryToken]
    public async Task<JsonResult> Login([FromBody] LoginVM user){
       
        string result = await auth.Login(user.Email, user.Password);
        if(result == null) {
            // HttpContext.User
            // HttpContext.Session["SessionID"]

            return Json(new {success = false}); 
            
        }
        
        ModelState.AddModelError("", result);
        return  Json(new {success = true});
       // return View("Login", user);
    }

    [HttpPost("register")]
    [AllowAnonymous]
    // [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register([FromBody] LoginVM user){
        var errors = await auth.Register(user.Email, user.Password);
        if((errors ?? new List<string>()).Count() == 0){
            return Redirect("/");
        } else {
            foreach(var e in errors)
                ModelState.AddModelError("", e);

            return View("Login", user);
        }
    }

    [HttpPost("logout")]
    // [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout(){
        await auth.Logout();
        return Redirect("/");
    }

    [HttpGetAttribute("About")]
    public IActionResult About() => View("About");

    [HttpGetAttribute("Contact")]
    public IActionResult Contact() => View("Contact");
    
}

public class LoginVM {
    [Required]
    [EmailAddress]
    public string Email {get;set;}
    [Required]
    [DataType(DataType.Password)]
    public string Password {get;set;}
}