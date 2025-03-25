using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreOrderBlindBox.Services.DTO.RequestDTO.RoleModel;
using PreOrderBlindBox.Services.IServices;

namespace PreOrderBlindBox.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RoleController : ControllerBase
	{
		private readonly IRoleService _roleService;

		public RoleController(IRoleService roleService)
		{
			_roleService = roleService;
		}
		[HttpGet("{roleId}")]
		public async Task<IActionResult> GetRoleById(int roleId)
		{
			try
			{
				var role = await _roleService.GetRoleByIdAsync(roleId);
				return Ok(role);
			}
			catch (Exception ex)
			{
				return NotFound(ex.Message);
			}
		}

		[HttpPost]
		public async Task<IActionResult> CreateRole(string roleName)
		{
			try
			{
				int ressult = await _roleService.CreateRoleAsync(roleName);
				if (ressult > 0)
				{
					return Ok(new { Message = "Role created successfully" });
				}
				else
				{
					return BadRequest(new { Message = "Failed to create role" });
				}
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPut]
		public async Task<IActionResult> UpdateRoleById(RequestRole role)
		{
			try
			{
				var result = await _roleService.UpdateRoleAsync(role);
				if (result > 0)
				{
					return Ok(new { Message = "Role update successfully" });
				}
				else
				{
					return BadRequest(new { Message = "Failed to update role" });
				}
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpDelete("{roleId}")]
		public async Task<IActionResult> DeleteRoleById(int roleId)
		{
			try
			{
				var result = await _roleService.DeleteRoleByIdAsync(roleId);
				if (result > 0)
				{
					return Ok(new { Message = "Role delete successfully" });
				}
				else
				{
					return BadRequest(new { Message = "Failed to delete role" });
				}
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
