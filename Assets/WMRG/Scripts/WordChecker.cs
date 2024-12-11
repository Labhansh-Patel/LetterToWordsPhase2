using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using APICalls;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class WordChecker : MonoBehaviour
{
    [SerializeField] private bool allowAnyWord = true;
    private TextAsset allWordCSV;
    [SerializeField] private List<TextAsset> allWordAsset;
    [SerializeField] private string wordtoCheck;

    public string json = string.Empty;

    public static WordChecker Instance;

    private Dictionary<char, List<string>> wordsDictionary = new Dictionary<char, List<string>>();

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        char ch;
        for (ch = 'a'; ch <= 'z'; ch++)
        {
            TextAsset currentFileToCheck = GetFile(ch);
            List<string> wordsList = new List<string>();
            wordsList = JsonConvert.DeserializeObject<List<string>>(currentFileToCheck.text);
            wordsDictionary.Add(char.ToUpper(ch), wordsList);
        }
    }

    private void PrintJsonDataInFile(string word)
    {
        word = word.ToUpper();
        LogSystem.LogEvent("word {0}", word);
        TextAsset currentFileToCheck = GetFile(word[0]);

        List<string> wordList = new List<string>();

        string[] linesInFile = currentFileToCheck.text.Split('\n', '\r');
   
        //Read all name from dictionary and add to list
        for (int i = 0; i < linesInFile.Length; i++)
        {
            string wordFromFile = linesInFile[i];

            if (wordFromFile.Length > 0)
            {
                wordList.Add(wordFromFile);
            }
        }


        json = JsonConvert.SerializeObject(wordList);

#if UNITY_EDITOR
        File.WriteAllText(AssetDatabase.GetAssetPath(currentFileToCheck), json);
#endif


        // LogSystem.LogEvent("Json {0}",json);
        // PlayerPrefs.SetString("Json",json);
    }


    IEnumerator Download(string filePath)
    {
        string url = filePath;
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.Send();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string savePath = string.Format("{0}/{1}.pdb", Application.persistentDataPath, filePath);
                System.IO.File.WriteAllText(savePath, www.downloadHandler.text);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.LeftControl))
        // {
        //   PrintJsonDataInFile(wordtoCheck);
        // }
        //
        //
        // if (Input.GetKeyDown(KeyCode.LeftShift))
        // {
        //     CheckForWord(wordtoCheck);
        // }
    }

    public void ToggleAllowAnyWord(bool enabled)
    {
        allowAnyWord = enabled;
        Debug.Log(allowAnyWord);
    }

    public bool CheckForWord(string word)
    {
        if (allowAnyWord)
            return true;
        if (word.Length < 1) return false;

        char startCh = char.ToUpper(word[0]);

        wordsDictionary.TryGetValue(startCh, out List<string> wordList);


        bool isValidWord = wordList.Contains(word.ToUpper());

        // LogSystem.LogEvent("Dictionary Valid {0} Word {1}",isValidWord , word);
        return isValidWord;
    }

    private bool CheckWordInFile(TextAsset fileAsset, string word)
    {
        try
        {
            if (fileAsset == null)
            {
                Debug.Log("allWordCSV is null, names files not loaded");
            }

            else
            {
                string matchWord = word.ToUpper();

                List<string> wordsList = new List<string>();
                wordsList = JsonConvert.DeserializeObject<List<string>>(fileAsset.text);


                if (wordsList.Contains(matchWord))
                {
                    LogSystem.LogEvent("Found Word");
                    return true;
                }
                else
                {
                    LogSystem.LogEvent("DidNot Find the word");
                    return false;
                }

                // string[] linesInFile = fileAsset.text.Split('\n','\r');
                //Read all name from dictionary and add to list
                // for (int i = 0; i < linesInFile.Length; i++)
                // {
                //     string wordFromFile = linesInFile[i];
                //     
                //     if (wordFromFile.Length > 0)
                //     {
                //        
                //         
                //         if (wordFromFile.Equals(matchWord))
                //         {
                //             return true;
                //         }
                //         // allNames.Add(linesInFile[i]);
                //     }
                //
                // }

                //return false;
                //Debug.Log("Boat count - " + linesInFile.Length);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Bot name could not be read due to ----> " + e.Message);
        }

        return false;
    }

    private TextAsset GetFile(char startingChar)
    {
        int index = char.ToUpper(startingChar) - 65;
        LogSystem.LogEvent("Index {0}", index);
        return allWordAsset[index];
    }
}