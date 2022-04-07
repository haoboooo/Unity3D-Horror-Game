// ActivePack Library
// Copyright (C) BigBlit Assets Michal Kalinowski
// http://bigblit.fun

namespace BigBlit.ActivePack
{
    public delegate void PositionChangeEvent(IPositionable positionable);

    /// <summary>
    /// Interface for all objects that have position-aware element like buttons heads or levers handles.
    /// </summary>
    public interface IPositionable
    {
        /// <summary> Gets actual position of the element.</summary>
        float Position { get; }

        /// <summary> Triggered each time when the position-aware element is changing position</summary>
        event PositionChangeEvent positionChangedEvent;
    }
}
