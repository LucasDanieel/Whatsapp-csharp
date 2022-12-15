using System.Runtime.CompilerServices;
using Whatsapp.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Whatsapp.SignalRChat
{
    public class ConnectionMapping
    {
        private readonly Dictionary<string, string> _connections = new Dictionary<string, string>();
        private readonly Repository _repo = new();


        public int Count
        {
            get { return _connections.Count; }
        }

        public void Add(string email, string connectionId)
        {
            lock (_connections)
            {
                string connection;
                if(!_connections.TryGetValue(email, out connection))
                {
                    _connections.Add(email, connectionId);
                }


                if (connection != null)
                {
                    _connections[email] = connectionId;
                }
            }

        }

        public string GetConnection(string email)
        {
            if (email == null) return string.Empty;
            string connection;
            if(_connections.TryGetValue(email, out connection))
            {
                return connection;
            }

            return string.Empty;
        }

        public string PegarEmail(string connection)
        {
            foreach (var _connection in _connections)
            {
                if (_connection.Value == connection)
                {
                    return _connection.Key;
                }
            }
            return string.Empty;
        }

        public void Desconectar(string connection)
        {
            foreach(var _connection in _connections)
            {
                if(_connection.Value == connection)
                {
                    _repo.AtualizarUltimoAcesso(_connection.Key);
                    _connections.Remove(_connection.Key);
                }
            }
        }
    }
}
