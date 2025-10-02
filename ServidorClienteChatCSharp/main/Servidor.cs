using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class Servidor
{
    private TcpOuvinte ouvinte;
    private List<ClienteTcp> clientes = new List<ClienteTcp>();

    public void Iniciar(int porta)
    {
        ouvinte = new TcpOuvinte(IpAddress.Any, porta);
        ouvinte.Iniciar();
        Console.WriteLine("Server Iniciado!!! Aguardando conexões... ");

        Thread aceitarThread = new Thread(AceitarClientes);
        acceptThread.Iniciar();
    }

    private void AceitarClientes()
    {
        while (true)
        {
            try
            {
                ClienteTcp cliente = ouvinte.AceitarClienteTcp();
                clientes.Add(cliente);
                Console.WriteLine("Cliente Conectado");

                Thread threadCliente = new Thread(() => ClienteTratado(cliente));
                threadCliente.Iniciar;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error em aceitar cliente: " + ex.Message);
            }
        }
    }

    private void ClienteTratado(ClienteTcp cliente)
    {
        NetworkStream stream = cliente.GetStream();
        byte[] buffer = new byte[1024];
        int bytesLidos;

        try
        {
            while ((bytesLidos = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                string messagem = Encoding.UTF8.GetString(buffer, 0, bytesLidos);
                console.WriteLine("Recebido: " + messagem);

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

class Program
{
    static void Main()
    {
        SimpleServer servidor = new SimpleServer();
        servidor.Iniciar(8000);
    }
}
