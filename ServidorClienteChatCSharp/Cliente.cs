using System;
using System.Net.Sockets;
using System.Text;

public class Cliente
{
    private TcpClient cliente;
    private NetworkStream stream;

    public void ConectarAoServidor(string enderecoIp, int porta)
    {
        cliente = new ClienteTcp(enderecoIp, porta);
        stream = cliente.GetStream();
        Console.WriteLine("Conectado ao servidor");

        LerMensagens();
    }

    private void LerMensagens()
    {
        byte[] buffer = new byte[1024];
        int bytesLidos;

        while ((bytesLidos = stream.Read(buffer, 0, buffer.Length)) > 0)
        {
            string mensagem = Encoding.UTF8.GetString(buffer, 0, bytesLidos);
            Console.WriteLine("Servidor avisa: " + mensagem);
        }
    }

    public void MandarMensagem(string mensagem)
    {
        if (stream != null && stream.CanWrite)
        {
            byte[] dados = Encoding.UTF8.GetBytes(mensagem);
            stream.Write(dados, 0, dados.Length);
            Console.WriteLine("Mensagem enviada: " + mensagem);
        }
    }
}

class ProgramaCliente
{
    static void Main() {
        Cliente cliente = new Cliente();
        cliente.ConectarAoServidor("127.0.0.1", 8000);

        Console.WriteLine("Digita a sua mensagem:");
        string mensagem = Console.ReadLine();
        cliente.MandarMensagem(mensagem);
    }
}
