using System;
using System.Collections.Generic;
using UnityEngine;

public class IPLogWindowUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Transform rowsParent;
    [SerializeField] private IPLogRow rowPrefab;

    private readonly List<IPLogRow> spawnedRows = new List<IPLogRow>();

    /// <summary>
    /// JSONLファイルを読み込み、ログ一覧を表示する。
    /// </summary>
    public void ShowLog(TextAsset jsonlFile)
    {
        ClearRows();

        if (jsonlFile == null)
        {
            Debug.LogWarning("IPLogWindowUI: JSONL file is null.");
            return;
        }

        string[] lines = jsonlFile.text.Split(
            new[] { "\r\n", "\n" },
            StringSplitOptions.RemoveEmptyEntries
        );

        foreach (string line in lines)
        {
            string json = line.Trim();

            if (string.IsNullOrWhiteSpace(json))
                continue;

            try
            {
                IPLogEntry entry = JsonUtility.FromJson<IPLogEntry>(json);

                if (entry == null)
                {
                    Debug.LogWarning(
                        $"IPLogWindowUI: Failed to parse log entry:\n{json}"
                    );

                    continue;
                }

                CreateRow(entry);
            }
            catch (Exception exception)
            {
                Debug.LogError(
                    $"IPLogWindowUI: JSON parse error.\n" +
                    $"JSON: {json}\n" +
                    $"Error: {exception.Message}"
                );
            }
        }
    }

    /// <summary>
    /// 1件分のログ行を生成する。
    /// </summary>
    private void CreateRow(IPLogEntry entry)
    {
        if (rowPrefab == null)
        {
            Debug.LogError("IPLogWindowUI: Row Prefab is not assigned.");
            return;
        }

        if (rowsParent == null)
        {
            Debug.LogError("IPLogWindowUI: Rows Parent is not assigned.");
            return;
        }

        IPLogRow row = Instantiate(rowPrefab, rowsParent);

        row.Setup(entry);

        spawnedRows.Add(row);
    }

    /// <summary>
    /// 現在表示されているログ行をすべて削除する。
    /// </summary>
    public void ClearRows()
    {
        foreach (IPLogRow row in spawnedRows)
        {
            if (row != null)
                Destroy(row.gameObject);
        }

        spawnedRows.Clear();
    }
}