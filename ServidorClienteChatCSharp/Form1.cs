using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SimpleTcp;

namespace ServidorClienteChat
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string ip;
        SimpleTcpServer server;

        private void botaoComecar_Click(object sender, EventArgs e)
        {
            ip = textboxip.Text;
            server = new SimpleTcpServer(ip);
            server.Events.ClientConnected += Evento_ClienteConectado;
            server.Events.DataReceived += Evento_DadoRecebido;
            server.Events.ClientDisconnected += Evento_ClienteDesconectado;
            
            // Start the server
            server.Start();
        }

        private void Evento_ClienteDesconectado(object sender, ConnectionEventArgs e)
        {
            if (richtextboxchat.InvokeRequired)
            {
                richtextboxchat.Invoke(new Action(() => 
                {
                    richtextboxchat.Text += $"{Environment.NewLine} {e.IpPort} Cliente Desconectado!... {e.Reason}";
                }));
            }
            else
            {
                richtextboxchat.Text += $"{Environment.NewLine} {e.IpPort} Cliente Desconectado!... {e.Reason}";
            }
        }

        private void Evento_DadoRecebido(object sender, DataReceivedEventArgs e)
        {
            if (richtextboxchat.InvokeRequired)
            {
                richtextboxchat.Invoke(new Action(() => 
                {
                    richtextboxchat.Text += $"{Environment.NewLine} {e.IpPort}: {Encoding.UTF8.GetString(e.Data)}";
                }));
            }
            else
            {
                richtextboxchat.Text += $"{Environment.NewLine} {e.IpPort}: {Encoding.UTF8.GetString(e.Data)}";
            }
        }

        private void Evento_ClienteConectado(object sender, ConnectionEventArgs e)
        {
            if (richtextboxchat.InvokeRequired)
            {
                richtextboxchat.Invoke(new Action(() => 
                {
                    richtextboxchat.Text += $"{Environment.NewLine} {e.IpPort} Cliente Conectado!";
                    listboxip.Items.Add(e.IpPort);
                }));
            }
            else
            {
                richtextboxchat.Text += $"{Environment.NewLine} {e.IpPort} Cliente Conectado!";
                listboxip.Items.Add(e.IpPort);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Initialization code if needed
        }

        // Method to send messages (you might want to add this)
        private void botaoEnviar_Click(object sender, EventArgs e)
        {
            if (server != null && server.IsListening)
            {
                string message = textboxMensagem.Text;
                if (!string.IsNullOrEmpty(message))
                {
                    server.SendToAll(message);
                    richtextboxchat.Text += $"{Environment.NewLine} Servidor: {message}";
                    textboxMensagem.Text = "";
                }
            }
        }
    }
}
