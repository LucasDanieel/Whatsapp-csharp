using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections;
using System.ComponentModel;
using Whatsapp.Model.Imagem;
using Whatsapp.Model.Mensagem;
using Whatsapp.Model.Usuario;

namespace Whatsapp.Data
{
    public class Repository : IRepository
    {
        private readonly string _connectionString = "Data Source=DESKTOP-8NNECFJ;Initial Catalog=WhatsApp;Integrated Security=SSPI;Persist Security Info=False;TrustServerCertificate=true";
        private readonly SqlConnection _connection;

        public Repository()
        {
            _connection = new SqlConnection(_connectionString);
        }
        public void CriarUsuario(CriarUsuario usuario)
        {
            var sql = "INSERT INTO [Usuario] VALUES(@Nome, @Email, @Senha, @Recado)";

            _connection.Execute(sql, new
            {
                usuario.Nome,
                usuario.Email,
                usuario.Senha,
                Recado = ""
            });

        }

        public void AtualizarUltimoAcesso(string email)
        {
            var sql = "UPDATE [Usuario] SET [Ultimo_Acesso] = GETDATE() WHERE [Email] = @email";

            _connection.Execute(sql, new { email });
        }

        public async Task<bool> Validar(string email, string senha)
        {
            var sql = "SELECT [Email], [Senha] FROM [Usuario] WHERE [Email] = @email AND [Senha] = @senha";

            var usuario = await _connection.QueryFirstOrDefaultAsync(sql, new
            {
                email,
                senha
            });

            if (usuario == null) return false;

            if (usuario.Email == email && usuario.Senha == senha)
            {
                return true;
            }
            return false;
        }

        public async Task<Usuario> PegarLoginUsuario(string email, string senha)
        {
            var sql = @"
                SELECT 
                    [Usuario].[Id],
                    [Usuario].[Nome],
                    [Usuario].[Email],
                    [Usuario].[Senha],
                    [Usuario].[Recado],
                    [UsuarioImagem].[Imagem]
                FROM 
                    [Usuario] LEFT JOIN [UsuarioImagem] ON [Usuario].[Id] = [UsuarioImagem].[UsuarioId]
                WHERE [Usuario].[Email] = @Email AND [Usuario].[Senha] = @Senha";

            return await _connection.QueryFirstOrDefaultAsync<Usuario>(sql, new
            {
                Email = email,
                Senha = senha
            });

        }

        public IEnumerable<ListaNovoContato> PegarContatoPeloEmail(string email, int meuId)
        {
            var sql = @"
                SELECT
                    [Usuario].[Id],
                    [Usuario].[Nome],
                    [Usuario].[Email],
                    [UsuarioImagem].[Imagem]
                FROM [Usuario] LEFT JOIN [UsuarioImagem] ON [UsuarioImagem].[UsuarioId] = [Usuario].[Id]
                WHERE [Usuario].[Email] LIKE @Exp AND [Usuario].[Id] != @meuId";

            return _connection.Query<ListaNovoContato>(sql, new
            {
                Exp = $"%{email}%",
                meuId
            });
        }

        public IEnumerable<ListaContatos> PegarMeusContatos(int id)
        {
            var sql = @"
                SELECT
                    [Usuario].[Id],
                    [Usuario].[Nome],
                    [Usuario].[Email],
                    [Usuario].[Recado],
                    [Usuario].[Ultimo_Acesso],
                    [Contato].[Aceito],
                    [UsuarioImagem].[Imagem],
                    (SELECT TOP(1)
                        [Texto]
                    FROM [Mensagem]
                    WHERE [Mensagem].[Id_Usuario_Enviou] = [Usuario].[Id] OR [Mensagem].[Id_Usuario_Recebeu] = [Usuario].[Id]
                    ORDER BY [Mensagem].[Data_Hora] DESC ) AS UltimaMensagem,
                    (SELECT TOP(1)
                            [Data_Hora]
                        FROM [Mensagem]
                        WHERE [Mensagem].[Id_Usuario_Enviou] = [Usuario].[Id] OR [Mensagem].[Id_Usuario_Recebeu] = [Usuario].[Id]
                        ORDER BY [Mensagem].[Data_Hora] DESC )  AS Hora
                FROM
                    [Usuario]
                    INNER JOIN [Contato] ON ([Contato].[Id_Usuario_Enviou] = @id OR [Contato].[Id_Usuario_Recebeu] = @id)
                    LEFT JOIN [UsuarioImagem] ON ([UsuarioImagem].[UsuarioId] = [Usuario].[Id])
                WHERE 
                    (([Usuario].[Id] = [Contato].[Id_Usuario_Enviou] OR [Usuario].[Id] = [Contato].[Id_Usuario_Recebeu]) AND ([Usuario].[Id] != @id))
                ";

            return _connection.Query<ListaContatos>(sql, new { id });
        }

        public async Task AdicionarContato(int meuId, int contatoId)
        {
            var sql = "INSERT INTO [Contato] VALUES(@Id_Usuario_Enviou, @Id_Usuario_Recebeu, @Aceito)";

            await _connection.ExecuteAsync(sql, new
            {
                Id_Usuario_Enviou = meuId,
                Id_Usuario_Recebeu = contatoId,
                Aceito = 0
            });
        }
        public async Task EnviarMensagem(Mensagem mensagem)
        {
            var sql = @"INSERT INTO [Mensagem] VALUES(@Msg, @Id_Usuario_Enviou, @Id_Usuario_Recebeu, GETDATE())";

            await _connection.ExecuteAsync(sql, new
            {
                Msg = mensagem.Texto,
                mensagem.Id_Usuario_Enviou,
                mensagem.Id_Usuario_Recebeu,
            });
        }

        public IEnumerable<Mensagem> PegarMensagens(int meuId, int amigoId)
        {
            var sql = @"
                SELECT
                    *
                FROM
                    [Mensagem]
                WHERE 
                    ([Mensagem].[Id_Usuario_Enviou] = @meuId AND [Mensagem].[Id_Usuario_Recebeu]= @amigoId) OR 
                    ([Mensagem].[Id_Usuario_Enviou] = @amigoId AND [Mensagem].[Id_Usuario_Recebeu]= @meuId)
                ORDER BY [Mensagem].[Data_Hora] ASC";

            return _connection.Query<Mensagem>(sql, new
            {
                meuId,
                amigoId
            });
        }

        public async Task<bool> AtualizarImagemPerfil(ImagemPerfil img)
        {
            var pegarImagem = await PegarImagemPeloUsuarioId(img.UsuarioId);

            string sql;
            int retorno;

            if (pegarImagem == null)
            {
                sql = "INSERT INTO [UsuarioImagem] VALUES(@UsuarioId, @ImagemId, @Imagem)";
                retorno = await _connection.ExecuteAsync(sql, new
                {
                    img.UsuarioId,
                    img.ImagemId,
                    img.Imagem
                });
            }
            else
            {
                sql = "UPDATE [UsuarioImagem] SET [Imagem] = @Imagem WHERE [UsuarioId] = @UsuarioId";
                retorno = await _connection.ExecuteAsync(sql, new
                {
                    img.UsuarioId,
                    img.Imagem
                });
            }

            if (retorno > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<ImagemValidar> PegarImagemPeloUsuarioId(int usuarioId)
        {
            var sql = @"SELECT [UsuarioId], [ImagemId] FROM [UsuarioImagem] WHERE [UsuarioId] = @usuarioId";

            return await _connection.QueryFirstOrDefaultAsync<ImagemValidar>(sql, new { usuarioId });
        }
    }
}
