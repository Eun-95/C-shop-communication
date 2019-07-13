using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace Network_Project
{
    public partial class Login : Form
    {
        string ID;
        string PW;
        Socket eunSock;
        IPAddress eunAddress = IPAddress.Parse("210.123.254.97");


        public Login()
        {
            InitializeComponent();
            //TCP 소켓 초기화
            eunSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        }

        private void User_Login_Click(object sender, EventArgs e)
        {
            if (eunSock.Connected)
            {
                MessageBox.Show("이미 연결되어 있습니다..", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            ID = UserID.Text.Trim(); //ID, Trim은 공백제거
            PW = UserPw.Text.Trim();
            int port = 15000; //포트 고정

            if (string.IsNullOrEmpty(ID))
            {
                MessageBox.Show("입력(ID)이 안된 부분이 있습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UserID.Focus();
                return;
            }
            else if (string.IsNullOrEmpty(PW))
            {
                MessageBox.Show("입력(PW)이 안된 부분이 있습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //서버에 연결
            try
            {
                eunSock.Connect(eunAddress, port);

            }
            catch (Exception)
            {
                eunSock.Close();
                MessageBox.Show("연결에 실패했습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }



            SendID();    //서버로 ID, password 묶어서 전송

            // 연결 완료, 서버에서 데이터가 올 수 있으므로 수신 대기한다.
            AsyncObject obj = new AsyncObject(4096);
            obj.WorkingSocket = eunSock;
            eunSock.BeginReceive(obj.Buffer, 0, obj.BufferSize, 0, DataReceived, obj);
        }

        void SendID()
        {
            try
            {
                //Login 정보
                string User = "100&" + UserID.Text + "&" + UserPw.Text + "&";
                byte[] ETbyte = Encoding.UTF8.GetBytes(User);

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

            //전송받은 메시지를 '&' 기준으로 나눔
            string[] tokens = text.Split('&');
            //전송 받은 메시지 코드
            string St_code = tokens[0];

            if (St_code.Equals("101"))
            {

                MessageBox.Show("로그인에 성공했습니다.");

                // 비동기식으로 작업하기 때문에 폼의 UI 스레드에서 작업을 해줘야 한다.
                // 따라서 대리자를 통해 처리한다.


                // 클라이언트에선 데이터를 전달해줄 필요가 없으므로 바로 수신 대기한다.
                // 데이터를 받은 후엔 다시 버퍼를 비워주고 같은 방법으로 수신을 대기한다.
                obj.ClearBuffer();

                //Arduino 폼 띄우기
                new Arduino(eunSock).ShowDialog();

                // 수신 대기
                obj.WorkingSocket.BeginReceive(obj.Buffer, 0, 4096, 0, DataReceived, obj);

            }
            else if (St_code.Equals("102"))
            {
                MessageBox.Show("ID가 맞지 않습니다.");
                //로그인에 실패하면 Socket이 닫히기 때문에 다시 생성 시켜주어야 한다.
                eunSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            }
            else if (St_code.Equals("103"))
            {
                MessageBox.Show("PW가 맞지 않습니다.");
                //로그인에 실패하면 Socket이 닫히기 때문에 다시 생성 시켜주어야 한다.
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
        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            string End_Msg = "500&End&";
            byte[] ETbyte = Encoding.UTF8.GetBytes(End_Msg);

            if (eunSock.Connected)
            {
                eunSock.Close();
            }
        }

        private void LoginClose_Click(object sender, EventArgs e)
        {
            string End_Msg = "500&End&";
            byte[] ETbyte = Encoding.UTF8.GetBytes(End_Msg);

            if (eunSock.Connected)
            {
                eunSock.Close();
            }
            this.Dispose(); //닫기
            Application.ExitThread(); //종료
            
        }
    }
}
        

       