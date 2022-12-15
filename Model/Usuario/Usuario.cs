namespace Whatsapp.Model.Usuario
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string? Recado { get; set; }
        public byte[] Imagem{ get; set; }

        public Usuario() { }

        public Usuario(int id, string nome, string email, string senha, string? recado, byte[] imagem)
        {
            Id = id;
            Nome = nome;
            Email = email;
            Senha = senha;
            Recado = recado;
            Imagem = imagem;
        }
    }
}
