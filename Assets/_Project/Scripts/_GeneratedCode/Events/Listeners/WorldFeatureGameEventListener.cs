using Descending.Features;
using UnityEngine;

namespace ScriptableObjectArchitecture
{
	[AddComponentMenu(SOArchitecture_Utility.EVENT_LISTENER_SUBMENU + "WorldFeature")]
	public sealed class WorldFeatureGameEventListener : BaseGameEventListener<WorldFeature, WorldFeatureGameEvent, WorldFeatureUnityEvent>
	{
	}
}