using NoticiasAPI.Model;

namespace NoticiasAPI.Service;

public interface INoticiaService
{
    bool CreateNoticia(Noticia noticia);
    Noticia? GetNoticiaById(int id);
    IEnumerable<Noticia> GetAllNoticia();
    bool UpdateNoticia(Noticia noticia);
    bool DeleteNoticia(int id);
}
