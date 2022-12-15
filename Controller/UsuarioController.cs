using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Whatsapp.Data;
using Whatsapp.Model.Usuario;
using Whatsapp.Model;

namespace Whatsapp.Controller
{
    [ApiController]
    [Route("usuario")]
    public class UsuarioController : ControllerBase
    {
        private readonly IRepository _repo;

        public UsuarioController(IRepository repo)
        {
            _repo = repo;
        }

        [HttpPost("criar")]
        public ActionResult CriarUsuario([FromBody] CriarUsuario usuario)
        {
            try
            {
                _repo.CriarUsuario(usuario);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("validar-usuario/{email}/{senha}")]
        public async Task<ActionResult> ValidarUsuario(string email, string senha)
        {
            try
            {
                var valido = await _repo.Validar(email, senha);
                return Ok(valido);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("buscar-usuario/{email}/{senha}")]
        public async Task<ActionResult> PegarUsuario(string email, string senha)
        {
            try
            {
                var usuario = _repo.PegarLoginUsuario(email, senha);
                if (usuario.Result == null)
                {
                    return Ok(false);
                }
                return Ok(usuario.Result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("contato/{email}/{meuId:int}")]
        public async Task<ActionResult> PegarContato(string email, int meuId)
        {
            try
            {
                var contato = _repo.PegarContatoPeloEmail(email, meuId);
                return Ok(contato);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("contato/meus-contatos/{id:int}")]
        public async Task<ActionResult> MeusContatos(int id)
        {
            try
            {
                var listaContatos = _repo.PegarMeusContatos(id);
                return Ok(listaContatos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("contato/adicionar")]
        public async Task<ActionResult> AdicionarContato([FromBody] Contato contato)
        {
            try
            {
                if (contato.Id_Usuario_Enviou <= 0 || contato.Id_Usuario_Recebeu <= 0)
                {
                    return BadRequest("Id vazio");
                }
                await _repo.AdicionarContato(contato.Id_Usuario_Enviou, contato.Id_Usuario_Recebeu);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
