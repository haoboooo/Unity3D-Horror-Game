using UnityEngine.Events;

namespace BigBlit.Keypads
{
    public interface INumPad
    {

        void RegisterValueChangedListener(UnityAction<INumPad> call);
        void RemoveValueChangedListener(UnityAction<INumPad> call);
        void RegisterEnterListener(UnityAction<INumPad> call);
        void RemoveEnterListener(UnityAction<INumPad> call);

        string Value { get; set; }
        int MaxLength { get; set; }

        void PressKey(string key);
        void PressBack();
        void PressClear();
        void PressEnter();
    }
}