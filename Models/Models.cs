using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public class User : HasId
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    [StringLength(50, MinimumLength = 10)]
    public string tagLine { get; set; }
    public string Avatar {get;set;}
    public string Email {get;set;}
    public string Password {get; set;}
    public List <Room> Rooms {get; set;}
    public List<Message> Messages {get; set; }
}
public class Room : HasId {
    [Required]
    public int Id { get; set; }
    [Required]
    public string Summary { get; set; }
    [Required]
    public List<User> Users { get; set; }
    public List<Message> Messages { get; set; }

    public int UserId {get;set;}
}
public class Message : HasId {
    [Required]
    public int Id { get; set; }
    [Required]
    public string Text { get; set; }
    [Required]
    public int RoomId {get; set; }
    public int UserId {get; set; }
    public DateTime createdAt {get; set; }
}

// declare the DbSet<T>'s of our DB context, thus creating the tables
public partial class DB : IdentityDbContext<IdentityUser> {
    public DbSet<User> Cards { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Room> Rooms { get; set; }
}

// create a Repo<T> services
public partial class Handler {
    public void RegisterRepos(IServiceCollection services){
        Repo<User>.Register(services, "Users");
        Repo<Message>.Register(services, "Messages");
        Repo<Room>.Register(services, "Rooms",
            d => d.Include(b => b.Users).ThenInclude(l => l.Messages));
    }
}