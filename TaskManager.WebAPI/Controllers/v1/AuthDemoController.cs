// using Asp.Versioning;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using TaskManager.WebAPI.Auth;
// using TaskManager.WebAPI.Models.v1.Auth;
//
// namespace TaskManager.WebAPI.Controllers.v1
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     [ApiVersion("1.0")]
//     public class AuthDemoController : ControllerBase
//     {
//         private readonly JwtTokenGenerator _jwtTokenGenerator;
//         private readonly ILogger<AuthDemoController> _logger;
//
//         public AuthDemoController(JwtTokenGenerator jwtTokenGenerator, ILogger<AuthDemoController> logger)
//         {
//             _jwtTokenGenerator = jwtTokenGenerator;
//             _logger = logger;
//         }
//
//         [HttpPost("token")]
//         [AllowAnonymous]
//         [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
//         [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
//         public IActionResult GetToken([FromBody] LoginRequest request)
//         {
//             if (!IsValidUser(request.Username, request.Password))
//             {
//                 _logger.LogWarning("Failed login attempt for user: {Username}", request.Username);
//                 return Unauthorized(new ProblemDetails
//                 {
//                     Title = "Invalid credentials",
//                     Status = StatusCodes.Status401Unauthorized,
//                     Detail = "Username or password is incorrect"
//                 });
//             }
//
//             var token = _jwtTokenGenerator.Generate(request.Username);
//
//             _logger.LogInformation("User logged in: {Username}", request.Username);
//
//             return Ok(new AuthResponse
//             {
//                 Token = token,
//                 ExpiresIn = 3600
//             });
//         }
//
//         private bool IsValidUser(string username, string password)
//         {
//             var demoUsers = new Dictionary<string, string>
//             {
//                 { "admin", "admin" },
//                 { "user", "user" },
//             };
//
//             return demoUsers.TryGetValue(username, out var expectedPassword) && expectedPassword == password;
//         }
//     }
// }
