using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

[Authorize]
[Route("/api/room")]
public class RoomController : CRUDController<Room> {
    public RoomController(IRepository<Room> r) : base(r){}
}

[Authorize]
[Route("/api/message")]
public class MessageController : CRUDController<Message> {
    public MessageController(IRepository<Message> r) : base(r){}
}

[Authorize]
[Route("/api/user")]
public class UserController : CRUDController<User> {
    public UserController(IRepository<User> r) : base(r){}
}
