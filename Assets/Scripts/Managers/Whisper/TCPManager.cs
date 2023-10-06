using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using System;

/// <summary>
/// Received Speech-To-Text data from Whisper, i.e. Transcribe.py
/// Saved the data in receivedTranscribe
/// </summary>
public class TCPManager : MonoBehaviour {
  [SpaceAttribute(10)]
  [HeaderAttribute("-------   Port    ------- ")]
  [SpaceAttribute(10)]
  [SerializeField] private int Port = 25001;

  /// <summary>-------   Private stuff    ------- </summary>
  private Thread thread;
  private TcpListener server;
  private TcpClient client;
  private bool running; 
  private string receivedTranscribe;

  /// <summary>-------   Received new sentence event    ------- </summary> 
  public static event Action<String> onNewSpeechInput; // Subscriber: OpenAI

  /// <summary>-------   Singleton stuff    ------- </summary>
  public static TCPManager instance;

  void Awake() {
    if (instance == null) instance = this;
  }

  void Start() {
    // Receive on a separate thread so Unity doesn't freeze waiting for data
    ThreadStart ts = new ThreadStart(GetData);

    thread = new Thread(ts);
    thread.Start();
  }

  void GetData() {
    server = new TcpListener(IPAddress.Any, Port); // Create the server
    server.Start();

    client = server.AcceptTcpClient(); // Create a client to get the data stream

    running = true; // Start listening
      while (running) {
        Connection();
      }

      server.Stop();
    }

  void Connection() {
    NetworkStream nwStream = client.GetStream(); // Read data from the network stream
    byte[] buffer = new byte[client.ReceiveBufferSize];
    int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

    // Decode the bytes into a string
    string dataReceived = Encoding.UTF8.GetString(buffer, 0, bytesRead);

    dataReceived.Trim();

    // Make sure we're not getting an empty string
    if (dataReceived != null && dataReceived != "") {                                       
      if (dataReceived == "HaHaHa") {
        Debug.Log("Successfully Connected to Whisper");
        nwStream.Write(buffer, 0, bytesRead); // 回傳
      }
      else {
        // Debug.Log(dataReceived); 
        receivedTranscribe = dataReceived;
      }
    }
  }

  public string TranscribeFromWhipser() { // Invoke OpenAI
    return receivedTranscribe;
  }

  public void ResetReceivedTranscribe() {
    receivedTranscribe = null;
  }

  /// <summary>-------   Close thread and udp connection   ------- </summary>
  // Close the tcp listener
  void OnApplicationQuit() {                                                                  
    try {
      if (this.thread != null) {
        this.thread.Abort();
        running = false;
      }

      if (this.client != null) {
        this.client.Close();
        running = false;
      }
    }
    catch (Exception e) {
      Debug.Log(e.Message);
    }

    try {
      server.Stop();
      running = false;
    }
    catch (Exception e) {
      Debug.Log(e.Message);
    }
  }
}