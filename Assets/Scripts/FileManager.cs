using UnityEngine;
using System.IO;

public class FileManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    void Start()
    {
        // EventManager.current.onSendFileReadRequest += OnSendFileReadRequest;
        // EventManager.current.onSendFileWriteRequest += OnSendFileWriteRequest;
    }

    void OnSendFileReadRequest(TextAsset fileToRead, DataType dataType)
    {
        if (fileToRead != null && dataType == DataType.PlayerData && gameManager != null)
        {
            // gameManager._playerContainer = ReadFile<PlayersContainer>(fileToRead);
            Debug.Log("File read operation SUCCESSFUL!");
        }
        else
        {
            Debug.Log("File read operation FAILED!");
        }

        // EventManager.current.OnEndFileRead();
    }

    void OnSendFileWriteRequest(object objectType, string fileName)
    {
        if (objectType != null && fileName != "" && gameManager != null)
        {
            WriteFile(objectType, fileName);
            //Debug.Log("File write operation SUCCESSFUL!");
        }
        else
        {
            Debug.Log("File write operation FAILED!");
        }
        
        // EventManager.current.OnEndFileWrite();
    }

    T ReadFile<T>(TextAsset fileToRead)
    {
        return JsonUtility.FromJson<T>(fileToRead.text);
    }

    void WriteFile(object objectType, string fileName)
    {
        string data = JsonUtility.ToJson(objectType);
        File.WriteAllText(Application.dataPath + "/Resources/Jsons/" + fileName, data);
    }

    void OnDestroy()
    {
        // EventManager.current.onSendFileReadRequest -= OnSendFileReadRequest;
        // EventManager.current.onSendFileWriteRequest -= OnSendFileWriteRequest;
    }
}