using AutoMapper;
using InnoGotchi_backend.Models.Dto;
using InnoGotchi_backend.Models.Entity;
using InnoGotchi_backend.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace InnoGotchi_backend.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        [Route("all-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            List<User> users = await _userService.GetAll();

            List<UserDto> dtos = _mapper.Map<List<UserDto>>(users);

            string json = JsonSerializer.Serialize(dtos);

            return Ok(json);
        }

        [Authorize]
        [HttpGet]
        [Route("users-with-no-invited")]
        public async Task<IActionResult> GetUsersWhithoutInvited()
        {
            //Get users without colaborators and yourself
            List<User> users = await _userService.GetUsersWithNoInvited(User.FindFirst(ClaimTypes.Email).Value);

            //Map it
            List<UserDto> dtos = _mapper.Map<List<UserDto>>(users);

            string json = JsonSerializer.Serialize(dtos);

            return Ok(json);
        }

        [Authorize]
        [HttpPatch]
        [Route("invite-user")]
        public async Task<IActionResult> GetUserInfo(string inviteUserEmail)
        {
            await _userService.InviteUserToColab(invitedUserEmail:inviteUserEmail,
                                                currentUserEmail: User.FindFirst(ClaimTypes.Email).Value);
            return Ok();
        }

        [Authorize]
        [HttpGet]
        [Route("find-user/{email}")]
        public async Task<IActionResult> FindUsers(string email)
        {
            User user = await _userService.GetUser(email: email);

            UserDto dto = _mapper.Map<UserDto>(user);
            
            List<UserDto> users = new List<UserDto>
            {
                dto
            };

            return Ok(JsonSerializer.Serialize(users));
        }

        [Authorize]
        [HttpGet]
        [Route("collaborators")]
        public async Task<IActionResult> GetCollaborators()
        {
            List<User> collaborators = await _userService.GetCollaborators(email: User.FindFirst(ClaimTypes.Email).Value);

            List<UserDto> dtos = _mapper.Map<List<UserDto>>(collaborators);

            return Ok(JsonSerializer.Serialize(dtos));
        }

        [Authorize]
        [HttpGet]
        [Route("i-am-collaborator")]
        public async Task<IActionResult> GetUsersWhereIAmCollaborator()
        {
            List<User> collaborators = await _userService.GetUsersIAmCollab(email: User.FindFirst(ClaimTypes.Email).Value);

            List<UserDto> dtos = _mapper.Map<List<UserDto>>(collaborators);

            return Ok(JsonSerializer.Serialize(dtos));
        }
    }
}
