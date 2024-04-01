//Thokozile Snono

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Diagnostics;


namespace ChatApp
{
    public partial class Timelapse : Form
    {
        //public static FileStream fs = new FileStream(@"C:\Users\Thokozile\source\repos\ChatApp\ChatApp\bin\Debug\Text.txt", FileMode.OpenOrCreate, FileAccess.Write);
        //public StreamWriter m_streamWriter = new StreamWriter(fs);

        Socket sck;
        EndPoint endpointLocal, endpointRemote;
        byte[] buffer; // For sending and recieving messages
        DateTime dateTime = DateTime.Now;
       
        //Timer.Interval = 1; // 100 ms

        public Timelapse()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // set user socket
            sck = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            sck.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            // get user IP
            textLocalIP.Text = GetLocalIp();
            textRemoteIP.Text = GetLocalIp();

           
            // Write to the file using StreamWriter class
            //m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);
            //m_streamWriter.Write("File Write Operation Starts: ");
            //m_streamWriter.WriteLine("{0} {1}",
            //DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
            //m_streamWriter.WriteLine("===================================== \n");
            //m_streamWriter.Flush();
        
    }

        private string GetLocalIp()
        {
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip.ToString();
            }
            return "127.0.0.1";
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            // binding the socket
            endpointLocal = new IPEndPoint(IPAddress.Parse(textLocalIP.Text), Convert.ToInt32(textLocalPort.Text));
            sck.Bind(endpointLocal);

            // connecting to remote IP
            endpointRemote = new IPEndPoint(IPAddress.Parse(textRemoteIP.Text), Convert.ToInt32(textRemotePort.Text));
            sck.Connect(endpointRemote);

            // listening the specific port
            buffer = new byte[1500];
            sck.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref endpointRemote, new AsyncCallback(MessageCallBack), buffer);

        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            // convert string message to byte[]
            ASCIIEncoding aEnconding = new ASCIIEncoding();
            byte[] sendingMessage = new byte[1500];
            sendingMessage = aEnconding.GetBytes(textMessage.Text);

            // sending the Encoded message
            sck.Send(sendingMessage);

            //string data = textMessage.Text;
            //listMessage.Items.Add();         
            // adding to the listbox
            // adding message timestamp to the listbox
            listShowData.Items.Add("Client 1: " + textMessage.Text);
            textMessage.Text = "";
            listShowData.Items.Add(dateTime);
            textMessage.Text = "";
            MessageBox.Show(Properties.Settings.Default.String, "Saved Data" +"\n"+"Client 1: ");

        }

        private void MessageCallBack(IAsyncResult aResult)
        {
            try
            {

                byte[] receivedData = new byte[1500];
                receivedData = (byte[])aResult.AsyncState;

                // converting byte[] to string
                ASCIIEncoding aEncoding = new ASCIIEncoding();
                string receivedMessage = aEncoding.GetString(receivedData);

                // adding this message into ListBox
                // adding message timestamp to the listbox
                listShowData.Items.Add("Client 2: " + receivedMessage);
                buffer = new byte[1500];
                sck.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref endpointRemote, new AsyncCallback(MessageCallBack), buffer);           
                listShowData.Items.Add(dateTime);
                textMessage.Text = "";
                MessageBox.Show(Properties.Settings.Default.String, "Saved Data" + "\n" + "Client 2: ");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private void Form_Main_Load(object sender, EventArgs e)
        {

            Timer.Interval = 100; // 100 ms
        }
         

        private void button1_Click(object sender, EventArgs e)
        {
            string data = textMessage.Text;
            //listMessage.Items.Add(data);
            Properties.Settings.Default.String = data;
            Properties.Settings.Default.Save();
        }


        private void buttonShowData_Click(object sender, EventArgs e)
        {
            string data = textMessage.Text;
            //try
            //{
                MessageBox.Show(Properties.Settings.Default.String, "Saved Data");
                listBoxShowData.Items.Add(data);
                //listBoxShowData.Items.Add("Client 2: " + receivedMessage);
                textMessage.Text = "";
            //}
            //catch (Exception ex)
            //{
                //MessageBox.Show(ex.ToString());
            //}
        }


        private void buttonReset_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reset();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
           //m_streamWriter.WriteLine("{0} {1}",
           //DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
           //m_streamWriter.Flush();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            Timer.Enabled = true;
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            Timer.Enabled = false;
        }

    }
}