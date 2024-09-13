using System;
using System.Collections.Generic;
using System.Text;
using Enums;
using UnityEngine;
using Utils;

public class LevelDataLoader : MonoBehaviour
{
    [Serializable]
    public class RowSettings
    {
        public BallEnum Name;
        public bool IsAvailable;
    }

    private const string URL_PATTERN = "https://docs.google.com/spreadsheets/d/{0}/export?format=csv&gid={1}";
    private const string EDIT_URL_PATTERN = "https://docs.google.com/spreadsheets/d/{0}/edit#gid={1}";
    
    [SerializeField] string tableId = string.Empty;
    [SerializeField] string tableGid = "0";

    public IReadOnlyList<RowSettings> LevelRowSettings => _rowSettings;

    private List<RowSettings> _rowSettings = new();

    private void LoadData()
    {
        var text = new StringBuilder();
        text.Append(HttpHelper.HttpGet(string.Format(URL_PATTERN, tableId, tableGid), "text/csv"));

        CSVReader.LoadFromString(text.ToString(), (index, line) =>
        {
            if (line.Count < 1)
                return;
            
            for (var i = 1; i < line.Count; i++)
            {
                if (index == 0 && Enum.TryParse<BallEnum>(line[i], out var result))
                {
                    _rowSettings.Add(new RowSettings { Name = result, IsAvailable = true });
                    continue;
                }

                if (bool.TryParse(line[i], out var boolResult))
                {
                    _rowSettings[i - 1].IsAvailable = boolResult;
                }
            }
        });
    }

#if UNITY_EDITOR
    public void LoadFromGoogle()
    {
        LoadData();
    }
    
    public void OpenForEdit()
    {
        Application.OpenURL(string.Format(EDIT_URL_PATTERN, tableId, tableGid));
    }
#endif
}
