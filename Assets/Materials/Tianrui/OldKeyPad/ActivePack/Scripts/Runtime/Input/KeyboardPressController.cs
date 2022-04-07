// ActivePack Library
// Copyright (C) BigBlit Assets Michal Kalinowski
// http://bigblit.fun

using UnityEngine;

namespace BigBlit.ActivePack
{
    /// <summary>
    /// Keyboard controller for all pressable objects.
    /// </summary>
    public class KeyboardPressController : PressControllerBase
    {
        #region FIELDS AND PROPERTIES

        [SerializeField] KeyCode keyCode = KeyCode.None;
        #endregion

        #region UNITY EVENTS
        private void Update() {
            if (Input.GetKeyDown(keyCode)) {
                Target.Press();
            }  else if(Input.GetKeyUp(keyCode)) {
                Target.Normal();
            }
        }

        #endregion
    }
}
