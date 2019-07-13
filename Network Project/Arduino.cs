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
using System.IO.Ports;

namespace Network_Project
{
    public partial class Arduino : Form
    {
        Socket eunSock;
        
        public Arduino(Socket eunSock)
        {
            InitializeComponent();

            this.eunSock = eunSock;
        }




        private void Arduno_write_Click(object sender, EventArgs e)
        {
            LCD_txt.Text.Trim();
            
            if (string.IsNullOrEmpty(LCD_txt.Text.Trim()))
            {
                MessageBox.Show("작성을 하지 않았습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LCD_txt.Focus();
                return;
            }
            //서버에 연결
           

            SendID();

            // 연결 완료, 서버에서 데이터가 올 수 있으므로 수신 대기한다.
            AsyncObject obj = new AsyncObject(4096);
            obj.WorkingSocket = eunSock;
            eunSock.BeginReceive(obj.Buffer, 0, obj.BufferSize, 0, DataReceived, obj);
        }

        void SendID()
        {
            try
            {
                String AR_User = "200&" + LCD_txt.Text + "&";
                byte[] ETbyte = Encoding.UTF8.GetBytes(AR_User);

                //서버에 전송한다.
                eunSock.Send(ETbyte);
            }
            catch (Exception e)
            {
                eunSock.Close();
                MessageBox.Show("다시 실행해주세요.\n예외 : " + e.StackTrace.ToString());
            }
        }

        void DataReceived(IAsyncResult ar)
        {
            // BeginReceive에서 추가적으로 넘어온 데이터를 AsyncObject 형식으로 변환한다.
            AsyncObject obj = (AsyncObject)ar.AsyncState;

            // 데이터 수신을 끝낸다.
            int received = obj.WorkingSocket.EndReceive(ar);

            // 받은 데이터가 없으면(연결끊어짐) 끝낸다.
            if (received <= 0)
            {
                obj.WorkingSocket.Disconnect(false);
                obj.WorkingSocket.Close();
                return;
            }
            // 텍스트로 변환한다.
            string text = Encoding.UTF8.GetString(obj.Buffer);

            string[] tokens = text.Split('&');
            string St_code = tokens[0];

            if (St_code.Equals("201"))
            {
                MessageBox.Show("메세지 전송이 되었습니다.");

                
                // 클라이언트에선 데이터를 전달해줄 필요가 없으므로 바로 수신 대기한다.
                // 데이터를 받은 후엔 다시 버퍼를 비워주고 같은 방법으로 수신을 대기한다.
                obj.ClearBuffer();

                // 수신 대기
                obj.WorkingSocket.BeginReceive(obj.Buffer, 0, 4096, 0, DataReceived, obj);

            }
            else if (St_code.Equals("400"))
            {
                MessageBox.Show("메세지 전송이 실패하였습니다.");
                eunSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            }
        }
        void OnSendData(object sender, EventArgs e)
        {
            // 서버가 대기중인지 확인한다.
            if (!eunSock.IsBound)
            {
                MessageBox.Show("서버가 실행되고 있지 않습니다!.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }
        private void Arduino_FormClosing(object sender, FormClosingEventArgs e)
        {
            string EndMsg = "500&End&";

            byte[] ETDts = Encoding.UTF8.GetBytes(EndMsg);
            if (eunSock.Connected)
            {
                eunSock.Close();
            }
        }
    }
}

    
