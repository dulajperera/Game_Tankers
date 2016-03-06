using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class Client
{
    public string m_IPAdress = "127.0.0.1"; //Server ip address
    public const int outPort = 6000;        //outgoing port number
    public const int inPort = 7000;         //incoming port number

    private Socket outSocket;

    private NetworkStream serverStream;     //Stream - incoming        
    private TcpListener listener;           //To listen to the clinets        
    public string reply = "";               //The message to be written

    private void Close()
    {
        outSocket.Close();
    }
    private void Connect()
    {
        System.Net.IPAddress remoteIPAddress = System.Net.IPAddress.Parse(m_IPAdress);
        outSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        outSocket.Connect(new System.Net.IPEndPoint(remoteIPAddress, outPort));

    }
    public void Listen()    //listening to the server
    {                               
        System.Net.IPAddress remoteIPAddress = System.Net.IPAddress.Parse(m_IPAdress);

        bool errorOcurred = false;
        Socket connection = null; //The socket that is listened to       
        try
        {
            //Creating listening Socket
            this.listener = new TcpListener(remoteIPAddress, inPort);
            //Starts listening
            this.listener.Start();
            Debug.Log("listening...");
            //Establish connection upon client request
            while (!navigator.gameInstance.GameFinished)
            {
                //connection is connected socket
                connection = listener.AcceptSocket();
                if (connection.Connected)
                {
                    //To read from socket create NetworkStream object associated with socket
                    this.serverStream = new NetworkStream(connection);

                    List<Byte> inputStr = new List<byte>();

                    int asw = 0;
                    while (asw != -1)
                    {
                        asw = this.serverStream.ReadByte();
                        inputStr.Add((Byte)asw);
                    }

                    reply = Encoding.UTF8.GetString(inputStr.ToArray());
                    this.serverStream.Close();

                    ThreadPool.QueueUserWorkItem(new WaitCallback(navigator.gameInstance.Resolve), (object)reply);    //parse the message from the 
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("RECEIVING Failed! \n " + e.Message);
            errorOcurred = true;
        }
        finally
        {
            if (connection != null)
                if (connection.Connected)
                    connection.Close();
            if (errorOcurred)
                Debug.Log("Error occured while listening! \n ");
            //this.Listen();
        }
    }

    void OnApplicationQuit()
    {
        outSocket.Close();      // closing the socket when exit
        outSocket = null;
    }

    public void Send(string msgData)     // send message to the server
    {
        Connect();
        if (this.outSocket == null)
            return;
        Byte[] tempStr = Encoding.ASCII.GetBytes(msgData);
        Byte[] sendData = tempStr;
        this.outSocket.Send(sendData);
        Close();
    }
}