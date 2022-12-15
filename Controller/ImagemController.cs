using Microsoft.AspNetCore.Mvc;
using Whatsapp.Data;
using Whatsapp.Model.Imagem;

namespace Whatsapp.Controller
{
    [ApiController]
    [Route("imagem")]
    public class ImagemController : ControllerBase
    {
        private readonly IRepository _repo;
        public ImagemController(IRepository repo)
        {
            _repo = repo;
        }

        [HttpPost("atualizar-foto"), Consumes("multipart/form-data")]
        public async Task<ActionResult> AtualizarImagemPerfil()
        {
            try
            {
                var file = Request.Form.Files[0];
                if (file == null)
                    return BadRequest();

                ImagemPerfil img;

                using (var stream = new MemoryStream())
                {
                    await file.OpenReadStream().CopyToAsync(stream);
                    img = new ImagemPerfil(int.Parse(file.Name), stream.ToArray());
                };
                var retorno = _repo.AtualizarImagemPerfil(img);

                if (retorno.Result == false)
                {
                    return BadRequest("A foto não foi alterada");
                }
                return Ok(img);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error c#: {ex.Message}");
            }
        }

        [HttpGet("pegar-imagem/{usuarioId:int}")]
        public async Task<ActionResult> PegarImagem(int usuarioId)
        {
            try
            {
                var retorno = await _repo.PegarImagemPeloUsuarioId(usuarioId);
                if(retorno == null)
                {
                    BadRequest("Imagem não encontrada");
                }

                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error c#: {ex.Message}");
            }
        }

    }
}
