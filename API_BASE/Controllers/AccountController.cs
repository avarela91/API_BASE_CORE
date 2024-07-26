using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Persistence.Repositories;

namespace API_BASE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public AccountController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }
    }
}
