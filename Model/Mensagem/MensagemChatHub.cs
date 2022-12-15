namespace Whatsapp.Model.Mensagem
{
    public class MensagemChatHub
    {
        public string Texto { get; set; }
        public int Id_Usuario_Enviou { get; set; }
        public int Id_Usuario_Recebeu { get; set; }
        public DateTime Data_Hora { get; set; }

        public MensagemChatHub(string texto, int id_Usuario_Enviou, int id_Usuario_Recebeu, DateTime data_Hora)
        {
            Texto = texto;
            Id_Usuario_Enviou = id_Usuario_Enviou;
            Id_Usuario_Recebeu = id_Usuario_Recebeu;
            Data_Hora = data_Hora;
        }
    }
}
