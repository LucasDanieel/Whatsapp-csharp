using Whatsapp.Model.Imagem;
using Whatsapp.Model.Mensagem;
using Whatsapp.Model.Usuario;

namespace Whatsapp.Data
{
    public interface IRepository
    {
        // Usuario
        void CriarUsuario(CriarUsuario usuario);
        void AtualizarUltimoAcesso(string email);
        Task<bool> Validar(string email, string senha);
        Task<Usuario> PegarLoginUsuario(string email, string senha);
        IEnumerable<ListaNovoContato> PegarContatoPeloEmail(string email, int meuId);
        IEnumerable<ListaContatos> PegarMeusContatos(int id);
        Task AdicionarContato(int meuId, int contatoId);

        // Imagem
        Task<bool> AtualizarImagemPerfil(ImagemPerfil img);
        Task<ImagemValidar> PegarImagemPeloUsuarioId(int usuarioId);

        // Mensagem
        Task EnviarMensagem(Mensagem mensagem);
        IEnumerable<Mensagem> PegarMensagens(int meuId, int amigoId);
    }
}
