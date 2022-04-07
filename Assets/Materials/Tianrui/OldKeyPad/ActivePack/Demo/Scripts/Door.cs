using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BigBlit.ActivePack;


public class Door : ActiveObject, IPositionable
{
    [SerializeField] bool _isOpen;

    public float Position => _isOpen ? 1.0f : 0.0f;

    public event PositionChangeEvent positionChangedEvent;

    protected override void OnValidate() {
        base.OnValidate();
        positionChangedEvent?.Invoke(this);
    }

    public bool IsOpen {
        get => _isOpen;
        set {
            if (_isOpen != value) {
                _isOpen = value;
                positionChangedEvent?.Invoke(this);
            }
        }
    }
}
