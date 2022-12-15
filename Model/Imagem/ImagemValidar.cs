namespace Whatsapp.Model.Imagem
{
    public class ImagemValidar
    {
        public int UsuarioId { get; set; }
        public string ImagemId { get; set; }

        public ImagemValidar(int usuarioId, string imagemId)
        {
            UsuarioId = usuarioId;
            ImagemId = imagemId;
        }
    }
}
