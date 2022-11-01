using System.Collections;
using System.Collections.Generic;
using Descending.Units;
using UnityEngine;

namespace Descending.Gui
{
    public class HeroListPanel : MonoBehaviour
    {
        [SerializeField] private GameObject _heroWidgetPrefab = null;
        [SerializeField] private Transform _heroWidgetsParent = null;

        private PartyWindow _partyWindow = null;
        private List<HeroListWidget> _heroWidgets = null;

        public void Setup(PartyWindow partyWindow)
        {
            _partyWindow = partyWindow;
            _heroWidgets = new List<HeroListWidget>();

            for (int i = 0; i < UnitManager.Instance.HeroUnits.Count; i++)
            {
                GameObject clone = Instantiate(_heroWidgetPrefab, _heroWidgetsParent);
                HeroListWidget widget = clone.GetComponent<HeroListWidget>();
                widget.Setup(_partyWindow, UnitManager.Instance.HeroUnits[i]);
                
                _heroWidgets.Add(widget);
            }
        }
    }
}