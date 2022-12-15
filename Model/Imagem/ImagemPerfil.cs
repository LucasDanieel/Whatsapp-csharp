namespace Whatsapp.Model.Imagem
{
    public class ImagemPerfil
    {
        public string ImagemId { get; set; }
        public int UsuarioId { get; set; }
        public byte[] Imagem { get; set; }


        public ImagemPerfil(int usuarioId, byte[] imagem)
        {
            ImagemId = Guid.NewGuid().ToString("N");
            UsuarioId = usuarioId;
            Imagem = imagem;
        }
    }
}
