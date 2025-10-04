using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class Servidor
{
    private TcpListener ouvinte;
    private List<TcpClient> clientes = new List<TcpClient>();

    public void Iniciar(int porta)
    {
        ouvinte = new TcpListener(IpAddress.Any, porta);
        ouvinte.Start();
        Console.WriteLine("Server Iniciado!!! Aguardando conexões... ");

        Thread aceitarThread = new Thread(AceitarClientes);
        aceitarThread.Start();
    }

    private void AceitarClientes()
    {
        while (true)
        {
            try
            {
                TcpClient cliente = ouvinte.AcceptTcpClient();
                clientes.Add(cliente);
                Console.WriteLine("Cliente Conectado");

                Thread threadCliente = new Thread(() => ClienteTratado(cliente));
                threadCliente.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error em aceitar cliente: " + ex.Message);
            }
        }
    }

    private void ClienteTratado(TcpClient cliente)
    {
        NetworkStream stream = cliente.GetStream();
        byte[] buffer = new byte[1024];
        int bytesLidos;

        try
        {
            while ((bytesLidos = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                string messagem = Encoding.UTF8.GetString(buffer, 0, bytesLidos);
                Console.WriteLine("Recebido: " + messagem);

                // Echo a mensagem de volta ao cliente
                byte[] resposta = Encoding.UTF8.GetBytes("Echo: " + messagem);
                stream.Write(resposta, 0, resposta.Length);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erro na comunicação com o cliente: " + ex.Message);
        }
        finally
        {
            stream.Close();
            cliente.Close();
            clientes.Remove(cliente);
            Console.WriteLine("Cliente Desconectado");
        }
    }
}

class ProgramServidor
{
    static void Main()
    {
        SimpleServer servidor = new SimpleServer();
        servidor.Iniciar(8000);
    }
}
