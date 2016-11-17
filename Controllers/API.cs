using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;

[Route("/api/user")]
public class UserController : CRUDController<User> {
    public UserController(IRepository<User> r) : base(r){}

    [HttpGet("search")]
    public IActionResult Search([FromQuery]string term, int listId = -1){
        return Ok(r.Read(dbset => dbset.Where(user => 
            user.Name.ToLower().IndexOf(term.ToLower()) != -1
            || user.tagLine.ToLower().IndexOf(term.ToLower()) != -1
        )));
    }
}

[Route("/api/message")]
public class MessageController : CRUDController<Message> {
    public MessageController(IRepository<Message> r) : base(r){}
}

[Route("/api/room")]
public class RoomController : CRUDController<Room> {
    public RoomController(IRepository<Room> r) : base(r){}
}
