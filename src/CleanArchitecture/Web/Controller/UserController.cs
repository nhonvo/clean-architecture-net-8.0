using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Shared.Models.AuthIdentity.UsersIdentity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CleanArchitecture.Web.Controller;

public class UserController(IUserService userService) : BaseController
{
    private readonly IUserService _userService = userService;

    /// <summary>
    /// Retrieves a list of all users. Only accessible by Admins.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [SwaggerResponse(200, "List of users retrieved successfully.", typeof(List<UserViewModel>))]
    [SwaggerResponse(403, "Access denied. Admin role required.")]
    public async Task<ActionResult<List<UserViewModel>>> Get(CancellationToken cancellationToken)
    {
        var users = await _userService.Get(cancellationToken);
        return Ok(users);
    }

    /// <summary>
    /// Updates the details of a user.
    /// </summary>
    /// <param name="request">Details to update.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    [HttpPut]
    [Authorize]
    [SwaggerResponse(204, "User updated successfully.")]
    [SwaggerResponse(400, "Invalid request.")]
    [SwaggerResponse(401, "Unauthorized access.")]
    public async Task<IActionResult> Update([FromBody] UserUpdateRequest request, CancellationToken cancellationToken)
    {
        await _userService.Update(request, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Deletes a user by their ID.
    /// </summary>
    /// <param name="userId">The ID of the user to delete.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    [HttpDelete("{userId}")]
    [Authorize]
    [SwaggerResponse(204, "User deleted successfully.")]
    [SwaggerResponse(404, "User not found.")]
    [SwaggerResponse(401, "Unauthorized access.")]
    public async Task<IActionResult> Delete(string userId, CancellationToken cancellationToken)
    {
        await _userService.Delete(userId, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Assigns roles to a user. Only accessible by Admins.
    /// </summary>
    /// <param name="request">Role assignment details.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    [HttpPost("assign-role")]
    [Authorize(Roles = "Admin")]
    [SwaggerResponse(204, "Role assigned successfully.")]
    [SwaggerResponse(400, "Invalid request.")]
    [SwaggerResponse(403, "Access denied. Admin role required.")]
    public async Task<IActionResult> RoleAssign([FromBody] RoleAssignRequest request, CancellationToken cancellationToken)
    {
        await _userService.RoleAssign(request, cancellationToken);
        return NoContent();
    }
}
