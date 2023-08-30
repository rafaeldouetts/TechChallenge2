using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoticiasAPI.Model;
using NoticiasAPI.Service;

namespace NoticiasAPI.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class NoticiaController : Controller
{
    public readonly INoticiaService _noticiaService;

    public NoticiaController(INoticiaService noticiaService)
    {
        _noticiaService = noticiaService;
    }
    [HttpPost]
    public IActionResult Post(Noticia noticia) => _noticiaService.CreateNoticia(noticia) ? Ok() : NotFound();
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var noticia = _noticiaService.GetNoticiaById(id);
        if (noticia == null) return NotFound();
        return Ok(noticia);
    }
    [HttpGet]
    public IActionResult GetAll()
    {
        var noticias = _noticiaService.GetAllNoticia();
        if (noticias == null) return NotFound();
        return Ok(noticias);
    }
    [HttpDelete]
    public IActionResult Delete(int id) => _noticiaService.DeleteNoticia(id) ? Ok() : NotFound();
    [HttpPut]
    public IActionResult Put(Noticia noticia) => _noticiaService.UpdateNoticia(noticia) ? Ok() : NotFound();
}
