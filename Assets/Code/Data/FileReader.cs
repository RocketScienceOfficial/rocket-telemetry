using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileReader : DataSupplyBase
{
    private const int DATA_RATE_MS = 10;

    public static FileReader Instance { get; private set; }

    public string CurrentFileName { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void ReadFile(string fullPath)
    {
        StartCoroutine(ReadFileCoroutune(fullPath));
    }

    private IEnumerator ReadFileCoroutune(string fullPath)
    {
        if (!File.Exists(fullPath))
        {
            AlertManager.Alert("Couldn't find provided file");

            yield break;
        }

        CurrentFileName = Path.GetFileName(fullPath);

        CallOnConnected();

        var content = File.ReadAllText(fullPath);
        var rows = content.Split('\n');

        foreach (var row in rows)
        {
            if (!string.IsNullOrEmpty(row))
            {
                CallOnSupplyReadData("/*" + row + "*/");

                yield return new WaitForSeconds(DATA_RATE_MS / 1000.0f);
            }
        }

        CurrentFileName = null;

        CallOnDisconnected();
    }

    public override string GetPath() => CurrentFileName;
    public override bool IsConnected() => !string.IsNullOrEmpty(CurrentFileName);
}