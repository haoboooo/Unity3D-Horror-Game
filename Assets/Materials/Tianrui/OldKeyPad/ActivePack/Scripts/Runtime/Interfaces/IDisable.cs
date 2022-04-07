using System.Collections;
using UnityEngine;

namespace BigBlit.ActivePack
{
    public delegate void DisableEvent(IDisable disable);

    public interface IDisable {

        event DisableEvent disableChangedEvent;

        bool IsDisabled { get; set; }
    }
}