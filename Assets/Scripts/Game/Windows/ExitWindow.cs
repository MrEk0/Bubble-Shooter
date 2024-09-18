using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Windows
{
    public class ExitWindow : AWindow<ExitWindowSetup>
    {
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _stayButton;

        [CanBeNull] private WindowSystem _windowSystem;
        [CanBeNull] private Action _exitCallback;

        private void OnEnable()
        {
            _exitButton.onClick.AddListener(OnExitButtonClicked);
            _stayButton.onClick.AddListener(OnStayButtonClicked);
        }

        private void OnDisable()
        {
            _exitButton.onClick.RemoveListener(OnExitButtonClicked);
            _stayButton.onClick.RemoveListener(OnStayButtonClicked);
        }

        public override void Init(ExitWindowSetup windowSetup)
        {
            _windowSystem = windowSetup.WindowSystem;
            _exitCallback = windowSetup.ExitCallback;
        }

        private void OnExitButtonClicked()
        {
            if (_windowSystem == null)
                return;

            _exitCallback?.Invoke();
            
            _windowSystem.Close<ExitWindow>();
        }
        
        private void OnStayButtonClicked()
        {
            if (_windowSystem == null)
                return;

            _windowSystem.Close<ExitWindow>();
        }
    }
}