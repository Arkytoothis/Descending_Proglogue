using System.Collections;
using System.Collections.Generic;
using DarkTonic.MasterAudio;
using UnityEngine;

namespace Descending.Gui
{
    public enum GameWindows { Menu, Party, Treasure, Armory, Stronghold, Library, Number, None }
    
    public abstract class GameWindow : MonoBehaviour
    {
        [SerializeField] protected GameObject _container = null;
        [SerializeField, SoundGroup] protected string _openSound = "";
        [SerializeField, SoundGroup] protected string _closeSound = "";
        
        [SerializeField] protected bool _isOpen = false;

        public bool IsOpen => _isOpen;

        public abstract void Setup();
        public abstract void Open();
        public abstract void Close();
    }
}
