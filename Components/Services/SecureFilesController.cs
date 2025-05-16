using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

//[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SecureFilesController : ControllerBase
{
	[HttpGet("{fileName}")]
	public IActionResult DownloadFile(string fileName)
	{
		var secureFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "securefiles");
		var fullPath = Path.Combine(secureFolder, fileName);

		if (!System.IO.File.Exists(fullPath))
			return NotFound();

		var fileBytes = System.IO.File.ReadAllBytes(fullPath);
		var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

		return File(fileBytes, contentType, fileName);
	}
}
