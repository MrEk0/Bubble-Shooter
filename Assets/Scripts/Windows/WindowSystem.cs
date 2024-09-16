using System;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace Windows
{
    public class WindowSystem : MonoBehaviour, IServisable
    {
        [SerializeField] private Transform _windowsParent;
        [SerializeField] private ASimpleWindow[] _windowsPrefabs;

        public event Action<ASimpleWindow> CloseWindowEvent = delegate { };

        private readonly Dictionary<Type, ASimpleWindow> _windows = new();
        private readonly Dictionary<Type, ASimpleWindow> _openedWindows = new();
        
        private void Awake()
        {
            foreach (var window in _windowsPrefabs)
                _windows.Add(window.GetType(), window);
        }
        
        public void Open<TWindow, TSetup>(TSetup setup) where TWindow : AWindow<TSetup>, new() where TSetup : AWindowSetup, new()
        {
            var windowType = typeof(TWindow);

            if (!_windows.ContainsKey(windowType))
                throw new Exception($"No prefab for window: {windowType}");

            if (_openedWindows.ContainsKey(windowType))
                return;

            var window = _windows[windowType];
            var aWindow = Instantiate<AWindow<TSetup>>((TWindow)window, _windowsParent);
            _openedWindows.Add(windowType, aWindow);

            aWindow.Init(setup);
        }
        
        public void Close<TWindow>() where TWindow : ASimpleWindow
        {
            var windowType = typeof(TWindow);

            if (!_openedWindows.TryGetValue(windowType, out var window))
                return;

            _openedWindows.Remove(windowType);

            CloseWindowEvent(window);
            
            Destroy(window.gameObject);
        }
    }
}
