using System.IO;
using APICalls;
using UnityEngine;
using UnityEngine.Networking;

public class DownloadHandlerFiles : DownloadHandlerScript
{

    public int contentLength { get { return _received > _contentLength ? _received : _contentLength; } }

    private int _contentLength;
    private int _received;
    private FileStream _stream;

    public float fileProgress;

    public DownloadHandlerFiles(string localFilePath, int bufferSize = 4096, FileShare fileShare = FileShare.ReadWrite) : base(new byte[bufferSize])
    {
        //localFilePath = localFilePath.Substring(6);
        localFilePath = Path.Combine(Application.persistentDataPath, localFilePath);
        Debug.Log("Saving to " + localFilePath);
        string directory = Path.GetDirectoryName(localFilePath);
        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

        _contentLength = -1;
        _received = 0;
        _stream = new FileStream(localFilePath, FileMode.OpenOrCreate, FileAccess.Write, fileShare, bufferSize);
    }

    protected override float GetProgress()
    {
        // Debug.Log("GetProgress = " + Mathf.Clamp01((float)_received / (float)contentLength));
        
        
        float progress =  contentLength <= 0 ? 0 : Mathf.Clamp01((float)_received / (float)contentLength);
      //  LogSystem.LogEvent("ContenT Length {0}",contentLength);
        return progress;
    }

    // [System.Obsolete("This is an obsolete method")]
    protected override void ReceiveContentLength(int contentLength)
    {
        _contentLength = contentLength;
        Debug.Log("contentLength - " + contentLength);
    }

    protected override void ReceiveContentLengthHeader(ulong contentLength) 
    {
        LogSystem.LogEvent("CONTENT LENGTH HEADER {0}", contentLength);
        _contentLength = (int) contentLength;
    }

    protected override bool ReceiveData(byte[] data, int dataLength)
    {
        if (data == null || data.Length == 0) return false;

        // Debug.Log("dataLength - " + dataLength);
        _received += dataLength;
       // LogSystem.LogEvent("Recieved {0}", _received);
        _stream.Write(data, 0, dataLength);

        return true;
    }

    protected override void CompleteContent()
    {
        CloseStream();
    }

    public new void Dispose()
    {
        CloseStream();
        base.Dispose();
    }

    private void CloseStream()
    {
        if (_stream != null)
        {
            _stream.Dispose();
            _stream = null;
        }
    }

    public float GetFileProgress()
    {
        // Debug.Log("GetFileProgress- " + GetProgress());
        return GetProgress();
    }

}