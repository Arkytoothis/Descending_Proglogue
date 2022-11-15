using Descending.Gui;
using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[AddComponentMenu(SOArchitecture_Utility.EVENT_LISTENER_SUBMENU + "CombatText")]
	public sealed class CombatTextGameEventListener : BaseGameEventListener<CombatText, CombatTextGameEvent, CombatTextUnityEvent>
	{
	}
}