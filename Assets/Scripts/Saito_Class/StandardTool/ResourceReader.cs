using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ResourceReader : MonoBehaviour
{
    public static List<string> GetCSVReadData(string fileName)
    {
        TextAsset       csvFile     = Resources.Load(fileName) as TextAsset;
        StringReader    reader      = new StringReader(csvFile.text);
        List<string>    readData    = new List<string>();

        // 説明箇所の読み込み
        reader.ReadLine();

        while (reader.Peek() != -1)
        {
            readData.Add(reader.ReadLine());
        }

        return readData;
    }
}
