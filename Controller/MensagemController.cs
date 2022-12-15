using Microsoft.AspNetCore.Mvc;
using Whatsapp.Data;
using Whatsapp.Model.Mensagem;

namespace Whatsapp.Controller
{
    [ApiController]
    [Route("mensagem")]
    public class MensagemController : ControllerBase
    {
        private readonly IRepository _repo;

        public MensagemController(IRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("lista-mensagens/{meuId:int}/{amigoId:int}")]
        public async Task<ActionResult<IEnumerable<Mensagem>>> ListaMensagens(int meuId, int amigoId)
        {
            try
            {
                var listaMensagem = _repo.PegarMensagens(meuId, amigoId);
                if (listaMensagem == null)
                {
                    return BadRequest(listaMensagem);
                }
                return Ok(listaMensagem);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("enviar")]
        public async Task<ActionResult> EnviarMensagem([FromBody] Mensagem mensagem)
        {
            try
            {
                await _repo.EnviarMensagem(mensagem);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
