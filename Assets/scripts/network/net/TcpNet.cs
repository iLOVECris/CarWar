using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using gprotocol;

public class msg_cmd
{
    public int stype;
    public int ctype;
    public byte[] body;
}

public enum Protocol_Type
{
    protocol_protobuf = 0,
    protocol_json = 1,
}

public class TcpNet:UnitySingleton<TcpNet> {
    public static Dictionary<int, messagedata> ServiceDic = new Dictionary<int, messagedata>();
    public Protocol_Type Protocol_type = 0;
    private const int Max_Data_Len = 8192;
    private const string ip_addr = "127.0.0.1";
    private const int port = 8001;
    Thread Recv_Thread;
    Socket client_socket;
    private byte[] Recv_Byte;
    private byte[] Recv_Long_Byte = null;
    int recv_data_len = 0;
    int Long_Data_Size = 0;
    public delegate void CtypeEventHandler(msg_cmd msg);
    private Queue<msg_cmd> recv_queue = new Queue<msg_cmd>();
    public static Dictionary<int, CtypeEventHandler> HandlerDic = new Dictionary<int, CtypeEventHandler>();

    // Use this for initialization
    void Start () {
        Debug.Log("tcp net start");
        Recv_Byte = new byte[Max_Data_Len];
        client_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress addr = IPAddress.Parse(ip_addr);
        IPEndPoint end_port = new IPEndPoint(addr, port);
        try
        {
            IAsyncResult result = client_socket.BeginConnect(end_port, new AsyncCallback(connected), client_socket);
            bool success = result.AsyncWaitHandle.WaitOne(5000, true);
            if(!success)
            {
                Debug.Log("链接超时....");
            }

        }
        catch(Exception e)
        {
            Debug.Log("connect error:" + e.ToString());
        }
    }

    public void RegisterServiceHandler(int stype, CtypeEventHandler handler)
    {
        if (HandlerDic.ContainsKey(stype))
        {
            HandlerDic[stype] += handler;
        }
        else
        {
            HandlerDic.Add(stype, handler);
        }
    }
    public static void RemoveServiceHandler(int stype, CtypeEventHandler handler)
    {
        if (HandlerDic.ContainsKey(stype))
        {
            HandlerDic[stype] -= handler;
        }
        else
        {
            HandlerDic.Remove(stype);
        }
    }

    void Recv_Server_Data(byte[] package)
    {
        msg_cmd msg = DecodeCmd.Decode_Protobuf(package);
        recv_queue.Enqueue(msg);

    }

    void SplitePackage()
    {
        int header_len = 0;
        byte[] package;
        byte[] data_package = Recv_Long_Byte == null ? Recv_Byte : Recv_Long_Byte;
        while (recv_data_len > 0)
        {
            package = TcpPacker.UnPackage(data_package, out header_len);
            if (package == null)//接受数据不足，继续接收
            {
                break;
            }
            Recv_Server_Data(package);
            if (recv_data_len > header_len)//多余一个包
            {
                byte[] tempArray = new byte[Max_Data_Len];
                Array.Copy(data_package, header_len, tempArray, 0, recv_data_len - header_len);
                data_package = tempArray;
            }

            recv_data_len -= header_len;

            if (recv_data_len == 0 && Recv_Long_Byte != null)
            {
                Recv_Long_Byte = null;
                Long_Data_Size = 0;
            }

        }
        

    }

    void Recv_Data()
    {
        int read_len = 0;
        while(true)
        {
            if(!client_socket.Connected)
            {
                break;
            }
            try
            {
                if (recv_data_len < Max_Data_Len)//收的数据小于范围 
                {
                    read_len = client_socket.Receive(Recv_Byte, recv_data_len, Recv_Byte.Length - recv_data_len, SocketFlags.None);
                }
                else
                {
                    if (Recv_Long_Byte == null)
                    {
                        int pkg_size;
                        TcpPacker.UnPackage(Recv_Byte,out pkg_size);
                        Long_Data_Size = pkg_size;
                        Recv_Long_Byte = new byte[Long_Data_Size];
                        Array.Copy(Recv_Byte,Recv_Long_Byte,Recv_Byte.Length);
                    }              
                    read_len = client_socket.Receive(Recv_Long_Byte, recv_data_len, Recv_Long_Byte.Length - recv_data_len, SocketFlags.None);

                }
                recv_data_len += read_len;

                SplitePackage();
            }
            catch (Exception e)//服务器主动关闭连接
            {
                Debug.Log(e.ToString());
                client_socket.Disconnect(true);
                client_socket.Shutdown(SocketShutdown.Both);
                client_socket.Close();
                //Recv_Thread.Abort();
                break;
            }
        }

    }

    void connected(IAsyncResult ar)
    {
        try
        {
            client_socket = (Socket)ar.AsyncState;
            client_socket.EndConnect(ar);
            Recv_Thread = new Thread(new ThreadStart(this.Recv_Data));
            Recv_Thread.Start();
        }
        catch(Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
    // Update is called once per frame
    void Update () {
		while(recv_queue.Count>0)
        {
            lock(recv_queue)
            {
                msg_cmd msg = recv_queue.Dequeue();
                if(HandlerDic.ContainsKey(msg.stype))
                {
                    HandlerDic[msg.stype](msg);
                }

            }
        }


	}

    public void send_proto_msg_to_client(int stype,int ctype, ProtoBuf.IExtensible msg)
    {
        Protocol_type = Protocol_Type.protocol_protobuf;
        byte[] send_bytes = EncodeCmd.Encode_Protobuf(stype, ctype, msg);
        byte[] tcp_package = TcpPacker.Package(send_bytes);
        this.client_socket.BeginSend(tcp_package, 0, tcp_package.Length, SocketFlags.None, new AsyncCallback(this.on_send_data), this.client_socket);
    }
    public void send_json_msg_to_client(int stype,int ctype,string body)
    {
        Protocol_type = Protocol_Type.protocol_json;
        byte[] send_bytes = EncodeCmd.Encode_Json(stype, ctype, body);
        byte[] tcp_package = TcpPacker.Package(send_bytes);
    }
    void on_send_data(IAsyncResult iar)
    {
        try
        {
            Socket client = (Socket)iar.AsyncState;
            client.EndSend(iar);
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    void close()
    {
        // abort recv thread
        if (this.Recv_Thread != null)
        {
            this.Recv_Thread.Abort();
        }
        // end

        if (this.client_socket != null && this.client_socket.Connected)
        {
            this.client_socket.Close();
        }
    }

    void OnApplicationQuit()
    {
        //Debug.Log("quit...");
        //TcpNet.Instance.send_proto_msg_to_client((int)Stype.TalkRoom, (int)Cmd.eExitChat, null);
        close();
    }

}
