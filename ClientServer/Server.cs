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


namespace ClientServer
{
    public partial class Server : Form
    {
        delegate void AppendTextDelegate(Control ctrl, string s);
        AppendTextDelegate _textAppender;
        Socket eunSock;
        IPAddress thisAddress;
        SerialPort arduSerialPort = new SerialPort(); //시리얼 포트 생성
        Dictionary<string, Socket> connectedClients;
        int clientNum;
        public Server()
        {
            InitializeComponent();

            eunSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            arduSerialPort.PortName = "COM3";    //아두이노가 연결된 시리얼 포트 번호 지정
            arduSerialPort.BaudRate = 9600;       //시리얼 통신 속도 지정
                           // 포트 오픈
            
            _textAppender = new AppendTextDelegate(AppendText);
            // Client의 ID와 Socket를 담는 Dictionary초기화
            connectedClients = new Dictionary<string, Socket>();
            //클라이언트 수 초기화
            clientNum = 0;
        }
        // 텍스트 추가 메소드 (데이터를 받으면 TextBox에 출력 해 준다.
        void AppendText(Control ctrl, string s)
        {
            if (ctrl.InvokeRequired) ctrl.Invoke(_textAppender, ctrl, s);
            else
            {
                string source = ctrl.Text;
                ctrl.Text = source + Environment.NewLine + s;
            }
        }
        private void Connection_Button_Click(object sender, EventArgs e)
        {
            int port;
            int.TryParse("15000", out port);
            thisAddress = IPAddress.Parse("210.123.254.97");

            if (thisAddress == null)
            {// 로컬호스트 주소를 사용한다.                
                thisAddress = IPAddress.Loopback;
            }
            // 서버에서 클라이언트의 연결 요청을 대기하기 위해
            // 소켓을 열어둔다.
            IPEndPoint serverEP = new IPEndPoint(thisAddress, port);
            eunSock.Bind(serverEP);
            eunSock.Listen(10);

            AppendText(txtHistory, string.Format("서버 시작: @{0}", serverEP));
            // 비동기적으로 클라이언트의 연결 요청을 받는다.
            eunSock.BeginAccept(AcceptCallback, null);
        }
        void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                // 클라이언트의 연결 요청을 수락한다.
                Socket client = eunSock.EndAccept(ar);

                eunSock.BeginAccept(AcceptCallback, null);

                AsyncObject obj = new AsyncObject(4096);// 4096 buffer size
                obj.WorkingSocket = client;


                AppendText(txtHistory, string.Format("클라이언트 접속 : @{0}",
                    client.RemoteEndPoint.ToString()));

                // 클라이언트의 ID 데이터를 받는다.
                client.BeginReceive(obj.Buffer, 0, 4096, 0, DataReceived, obj);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace.ToString());
                if (eunSock != null)
                    eunSock.Close();
            }
        }
        // 데이터를 받는 부분
        void DataReceived(IAsyncResult ar)
        {
            // BeginReceive에서 추가적으로 넘어온 데이터를 AsyncObject 형식으로 변환한다.
            AsyncObject obj = (AsyncObject)ar.AsyncState;

            // 데이터 수신을 끝낸다.
            int received = obj.WorkingSocket.EndReceive(ar);

            // 받은 데이터가 없으면(연결끊어짐) 끝낸다.
            if (received <= 0)
            {
                // AppendText(txtHistory, string.Format("클라이언트 접속해제?{0}", clientNum));
                if (clientNum > 0)
                {
                    foreach (KeyValuePair<string, Socket> clients in connectedClients)
                    {
                        if (obj.WorkingSocket == clients.Value)
                        {
                            string key = clients.Key;
                            try
                            {
                                connectedClients.Remove(key);
                                AppendText(txtHistory, string.Format("접속해제완료:{0}", key));
                            }
                            catch
                            {

                            }
                            break;
                        }
                    }
                }
                return;
            }
            // 텍스트로 변환한다.
            string text = Encoding.UTF8.GetString(obj.Buffer);


            // & 기준으로 짜른다.
            string[] tokens = text.Split('&');
            // 항상 데이터의 0번째와 1번째는
            // 상태 코드, id가 각각 들어간다.
            string St_code = tokens[0];
            string us_ID = tokens[1];
            

            // 전송받은 데이터에 대한 ACK를 보내기 위한 변수
            string Message = null;

            byte[] Msg = null;
            Socket socket = null;

            switch (St_code)
            {
                // 로그인
                case "100":
                    clientNum++;
                    // 연결 완료되었다는 메세지를 띄워준다.
                    AppendText(txtHistory, "서버와 연결되었습니다.");
                    AppendText(txtHistory, string.Format("[접속{0}]ID:{1}:{2}",
                    clientNum, us_ID, obj.WorkingSocket.RemoteEndPoint.ToString()));

                    //ID 체크 하는 부분
                    if (us_ID == "kdw0439")
                    {
                        string us_PW = tokens[2];

                        //PW 체크하는 부분
                        if (us_PW == "rkddkwl125")
                        {
                            //리스트에 추가하는 부분은 로그인 시에만 하면 됨
                            connectedClients.Add(us_ID, obj.WorkingSocket);
                            //현재 id(key)에 해당하는 Socket값을 socket변수에 반환 시켜 준다.
                            connectedClients.TryGetValue(us_ID, out socket);
                            //로그인 성공 메시지
                            Message = "101&Success&로그인 OK&";

                            //TextBox에 메시지 출력
                            AppendText(txtHistory, Message);
                            //전송할 메시지를 UTF8로 인코딩
                            Msg = Encoding.UTF8.GetBytes(Message);
                            //현재 socket값에 메시지 전송
                            SendTo(socket, Msg);
                        }
                        // 로그인 실패(맞지 않는 PW)
                        // 로그인 실패시에는 ID를 Dic에 추가하지 않는다.
                        else if (us_PW != "rkddkwl125")
                        {
                            //잘못된 PW 입력하여 실패 메시지 출력
                            Message = "103&False&PW error&";
                            // TextBox에 메시지 출력
                            AppendText(txtHistory, Message);
                            //메시지를 UTF8로 인코딩
                            Msg = Encoding.UTF8.GetBytes(Message);
                            //현재 sock값에 메시지 전송
                            SendTo(obj.WorkingSocket, Msg);
                            // 로그인에 실패한 소켓 값은 닫는다.
                            obj.WorkingSocket.Close();
                        }
                    }
                    // 로그인 실패(맞지 않는 ID)
                    // 로그인 실패시에는 ID를 Dic에 추가하지 않는다.
                    else if (us_ID != "kdw0439")
                    {
                        //id 오류 메시지 출력
                        Message = "102&False&id error&";
                        // TextBox에 메시지 출력
                        AppendText(txtHistory, Message);
                        //메시지를 UTF8로 인코딩
                        Msg = Encoding.UTF8.GetBytes(Message);
                        //현재 sock값에 메시지 전송
                        SendTo(obj.WorkingSocket, Msg);
                        // 로그인에 실패한 소켓 값은 닫는다.
                        obj.WorkingSocket.Close();
                    }
                    break;
                // 아두이노(LCD) 메시지 전송
                case "200":
                    
                    string us_Msg = tokens[1];
                    // 연결 완료되었다는 메세지를 띄워준다.
                    AppendText(txtHistory, "서버와 연결되었습니다.");
                    AppendText(txtHistory, string.Format("[접속{0}] Message:{1} :{2}",
                    clientNum, us_Msg, obj.WorkingSocket.RemoteEndPoint.ToString()));
                   
                    if (us_Msg.Length < 21)
                    {
                        
                        
                        connectedClients.Add(us_Msg, obj.WorkingSocket);
                        //현재 id(key)에 해당하는 Socket값을 socket변수에 반환 시켜 준다.
                        connectedClients.TryGetValue(us_Msg, out socket);
                        //메시지 전송 성공 
                        Message = "201&Success&message OK&";
                       
                        arduSerialPort.Write(us_Msg);

                        //TextBox에 메시지 출력
                        AppendText(txtHistory, Message);
                        //전송할 메시지를 UTF8로 인코딩
                        Msg = Encoding.UTF8.GetBytes(Message);
                        //현재 sock값에 메시지 전송
                        SendTo(socket, Msg);
                    }
                    else
                    {
                        // 메시지 전송 실패
                        Message = "400&False&message fail&";
                        AppendText(txtHistory, Message);
                        //메시지를 UTF8로 인코딩
                        Msg = Encoding.UTF8.GetBytes(Message);
                        SendTo(obj.WorkingSocket, Msg);
                        // 실패한 소켓 값은 닫는다.
                        obj.WorkingSocket.Close();
                    }
                    break;

                case "500":
                    Message = "501&End&";
                    connectedClients.TryGetValue(us_ID, out socket);
                    Msg = Encoding.UTF8.GetBytes(Message);
                    connectedClients.Remove(us_ID);
                    AppendText(txtHistory, string.Format("접속해제완료:{0}", us_ID));
                    break;
            }
            //--------------------------------------------------------------------

            // 텍스트박스에 추가해준다.

            // 비동기식으로 작업하기 때문에 폼의 UI 스레드에서 작업을 해줘야 한다.

            // 따라서 대리자를 통해 처리한다.

            // AppendText(txtHistory, string.Format("[받음]{0}: {1}", id, msg));

            // 데이터를 받은 후엔 다시 버퍼를 비워주고 같은 방법으로 수신을 대기한다.

            obj.ClearBuffer();

            // 수신 대기

            if (socket == null)
            {
                Console.WriteLine("비어있음");
            }

            else if (socket.Connected)
            {
                obj.WorkingSocket.BeginReceive(obj.Buffer, 0, 4096, 0, DataReceived, obj);
            }

        }
        void SendTo(Socket socket, byte[] buffer)
        {
            try
            {
                socket.Send(buffer);
            }
            catch
            {// 오류 발생하면 전송 취소
                try
                {
                    AppendText(txtHistory, "dispose");
                    socket.Dispose();
                }
                catch { }
            }
        }
    }
}





