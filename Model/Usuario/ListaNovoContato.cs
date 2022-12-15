namespace Whatsapp.Model.Usuario
{
    public class ListaNovoContato
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public byte[] Imagem { get; set; }

        public ListaNovoContato(int id, string nome, string email, byte[] imagem)
        {
            Id = id;
            Nome = nome;
            Email = email;
            Imagem = imagem;
        }
    }
}
