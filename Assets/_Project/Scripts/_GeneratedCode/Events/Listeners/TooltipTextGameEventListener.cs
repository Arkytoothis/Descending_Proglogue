using Descending.Gui;
using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[AddComponentMenu(SOArchitecture_Utility.EVENT_LISTENER_SUBMENU + "TooltipText")]
	public sealed class TooltipTextGameEventListener : BaseGameEventListener<TooltipText, TooltipTextGameEvent, TooltipTextUnityEvent>
	{
	}
}