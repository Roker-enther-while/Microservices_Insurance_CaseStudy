using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace DocumentService.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DocumentsController : ControllerBase
{
    private readonly string certsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Certs");

    [HttpGet]
    public IActionResult Get()
    {
        var files = Directory.GetFiles(certsDirectory, "*.pdf")
            .Select(Path.GetFileName)
            .ToList();
        return Ok(files);
    }

    [HttpGet("{fileName}")]
    public IActionResult Download(string fileName)
    {
        var filePath = Path.Combine(certsDirectory, fileName);
        if (!System.IO.File.Exists(filePath))
            return NotFound();

        var fileBytes = System.IO.File.ReadAllBytes(filePath);
        return this.File(fileBytes, "application/pdf", fileName);
    }
}
