namespace Whatsapp.Model.Usuario
{
    public class ListaContatos
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Recado { get; set; }
        public string UltimaMensagem { get; set; }
        public DateTime Ultimo_Acesso { get; set; }
        public DateTime Hora { get; set; }
        public int Aceito { get; set; }
        public byte[] Imagem { get; set; }
    }
}
