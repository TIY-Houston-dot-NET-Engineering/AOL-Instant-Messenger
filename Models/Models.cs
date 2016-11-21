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
public class Room : HasId {  //1-M w/ Users and Msgs
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name {get; set; }
    public string Summary { get; set; }
    public List<User> Users { get; set; } = new List <User>();
    public List<Message> Messages { get; set; } = new List<Message>();
}
public class Message : HasId { //1-1 w/ Room; 1-1 w/ a User
    [Required]
    public int Id { get; set; }
    [Required]
    public string Text { get; set; } = "";
    public int RoomId{get; set;}  //foreign key declaration x2 below
    public Room Room {get;set;} 
    public int UserId{get; set;}
    public User User {get; set; }
    public DateTime createdAt {get; set; } = DateTime.Now;
}
public class User : HasId{ //1-M w/ Msgs; 1-1 w/ a Room
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }

    [StringLength(50, MinimumLength = 5)]
    public string tagLine { get; set; }
    public string Avatar {get;set;}
    public List <Message> Messages{get; set;}
    public Room Room {get; set; }
}

// declare the DbSet<T>'s of our DB context, thus creating the tables
public partial class DB : IdentityDbContext<IdentityUser> {
    public DbSet<User> AOLUsers { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Room> Rooms { get; set; }
}

// create a Repo<T> services
public partial class Handler {
    public void RegisterRepos(IServiceCollection services){
        Repo<User>.Register(services, "Users");
        Repo<Message>.Register(services, "Messages", dbset => dbset.Include(x => x.User));
        Repo<Room>.Register(services, "Rooms",
            dbset => dbset.Include(x => x.Users).ThenInclude(x => x.Messages));
    }
}