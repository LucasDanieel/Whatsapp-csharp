using Microsoft.AspNetCore.SignalR;
using Whatsapp.Data;
using Whatsapp.Model.Mensagem;

namespace Whatsapp.SignalRChat
{
    public class ChatHub : Hub
    {
        private readonly static ConnectionMapping _connections = new ConnectionMapping();

        public void OnConnected(string email)
        {
            _connections.Add(email, Context.ConnectionId);
            Clients.All.SendAsync("Conectou", email);
        }

        public void EnviarMensagem(string enviarPara, MensagemChatHub msg)
        {
            string conexaoId = _connections.GetConnection(enviarPara);

            if (conexaoId == "") return;
            Clients.Client(conexaoId).SendAsync("Msg", msg);
        }

        public void Escrevendo(string enviarPara)
        {
            string conexaoId = _connections.GetConnection(enviarPara);

            if (conexaoId == "") return;
            Clients.Client(conexaoId).SendAsync("ContatoEscrevendo");
        }

        public void VerificarConexao(string email)
        {
            string conexaoId = _connections.GetConnection(email);
            if (conexaoId == "")
            {
                Clients.Client(Context.ConnectionId).SendAsync("Online", false);
                return;
            };
            Clients.Client(Context.ConnectionId).SendAsync("Online", true);
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            string email = _connections.PegarEmail(Context.ConnectionId);
            Clients.All.SendAsync("Desconectou", email);
            _connections.Desconectar(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
