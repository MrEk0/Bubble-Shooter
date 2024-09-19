using System;
using System.Collections.Generic;
using System.Text;
using Enums;
using Interfaces;
using UnityEngine;
using Utils;

namespace Game.Level
{
    public class LevelDataLoader : MonoBehaviour, IServisable
    {
        [Serializable]
        public class LevelBallSettings
        {
            public BallEnum Type;
            public bool IsAvailable;
        }

        private const string URL_PATTERN = "https://docs.google.com/spreadsheets/d/{0}/export?format=csv&gid={1}";
        private const string EDIT_URL_PATTERN = "https://docs.google.com/spreadsheets/d/{0}/edit#gid={1}";

        [SerializeField] private string _tableId = string.Empty;
        [SerializeField] private string _tableGid = "0";
        [SerializeField] private List<LevelBallSettings> _levelSettings = new();

        public IEnumerable<LevelBallSettings> LevelSettings => _levelSettings;

        private void LoadData()
        {
            _levelSettings.Clear();

            var text = new StringBuilder();
            text.Append(HttpHelper.HttpGet(string.Format(URL_PATTERN, _tableId, _tableGid), "text/csv"));

            CSVReader.LoadFromString(text.ToString(), (index, line) =>
            {
                if (line.Count < 1)
                    return;

                for (var i = 1; i < line.Count; i++)
                {
                    if (index == 0 && Enum.TryParse<BallEnum>(line[i], out var result))
                    {
                        _levelSettings.Add(new LevelBallSettings { Type = result, IsAvailable = false });
                        continue;
                    }

                    if (bool.TryParse(line[i], out var boolResult))
                    {
                        _levelSettings[i - 1].IsAvailable = boolResult;
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
            Application.OpenURL(string.Format(EDIT_URL_PATTERN, _tableId, _tableGid));
        }
#endif
    }
}
