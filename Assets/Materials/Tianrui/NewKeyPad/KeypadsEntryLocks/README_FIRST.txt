Thank You for using "Keypads: Entry Locks" package :-)
Please send any questions to bigblit@bigblit.fun

To start:

1. If you are using HDRP please install BigBlit_KEL_HDRP.unityPackage or BigBlit_KEL_HDRP_Preview.unityPackage (for preview versions of HDRP) first.
2. Load the demo scene from BigBlit/ActivePack/KeypadsEntryLocks/Demo/Scenes and make sure that lighting data and probes are baked.
or
3. Add PhysicsRaycaster component to your scene camera.
4. In the PhysicsRaycaster component set Event Mask to UI layer.
5. Drag one of the prefabs from the project view from BigBlit/ActivePack/KeypadsEntryLocks/Prefabs folder into the scene or hierarchy view and make sure that each button has UI layer set.
6. Make sure that Unity Event System is on the scene. 
If not, add new game object, call it EventSystem and add "Standalone Input Module" component to the object.
7. Go and play with the asset :-)

The components included in the package:

Main Keypads Components (BigBlit/ActivePack/KeypadsEntryLocks/Scripts):
NumPad - Implements simple numeric pad behaviours (PressKey, PressCancel, PressEnter, etc.)
SimpleLocker - Implements simple locking mechanism. Works with NumPad.
NumpadText - Text renderer component.
NumPadTextController - Controlls NumpadText.
NumPadTimer - Timer widget. Works with NumpadText.

ActivePack Library Components (BigBlit/ActivePack/Scripts):
Input components:
Input components are used to send events to NumPad.

PressButton - Pressable Button. Implements press behaviour and events.
ClickButton - Clickable Button. Implements click and long click behaviour and events.
ToggleButton - Toggleable Button. Implements toggle on/toggle of behaviour and events.
ButtonSwitch - Add swtich behaviour to group of ToggleButtons.
Lever - Implements Lever behaviours and events (used for doorpad door handles).

UnityEvents Triggers:
PressableButtonEventTrigger - Converts native IPressable interface events to Unity Events.
ClickableEventTrigger -  Converts native IClickable (ex. click button) events to Unity Events.
ToggleableEventTrigger - Converts native IToggleable interface events to Unity Events.
PositionableEventTrigger -  Converts native IPositionable interface events to Unity Events.
DraggableEventTrigger - Converts native IDraggable interface events to Unity Events.

Input:
KeyboardPressController - Keyboard input controller for all pressable objects.
PointerPressController - Pointer controller for all pressable objects.
PointerDragController - Pointer controller for drag events
CircularPointerDragController - Pointer controller for circular drag gesture.

NOTICE: The PointerPressController, PointerDragController and CircularPointerDragController  is based on Unity Event System. 
For it to work please make sure that:
 - You have unity Event System configured.
 - Camera has PhysicsRaycaster component added.
 - PhysicsRaycaster EventMask layer and ActiveObjects layers are properly set.

Misc:
ColorChanger - A color changer that uses PropertyBlock to change material colors efficiently.
EmissionController - Emission controller that react on ActiveObject events and uses PropertyBlock to change material emission efficiently. Used to add lighting to the buttons decals.
PositionAnimator - Animates GameObject that implements IPositionable interface by using Playables and AnimationClips.
