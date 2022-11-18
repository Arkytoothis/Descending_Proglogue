using System.Collections.Generic;
using System.Text;
using Descending.Abilities;
using Descending.Units;
using UnityEngine;

namespace Descending.Equipment
{
    [System.Serializable]
    public class UsableData
    {
        [SerializeField] private bool _hasData = true;
        [SerializeField] private int _maxUses = 0;
        [SerializeField] private float _cooldown = 0f;
        [SerializeReference] private AbilityEffects _effects = null;

        public bool HasData { get => _hasData; }
        public int MaxUses => _maxUses;
        public AbilityEffects Effects { get => _effects; }
        public float Cooldown => _cooldown;

        public UsableData(UsableData usableData)
        {
            _hasData = usableData._hasData;
            _maxUses = usableData.MaxUses;
            _effects = usableData.Effects;
            _cooldown = usableData.Cooldown;
        }

        public void Use(Unit user, List<Unit> targets)
        {
            for (int i = 0; i < targets.Count; i++)
            {
                for (int j = 0; j < _effects.Data.Count; j++)
                {
                    _effects.Data[i].Process(user, targets);
                }
            }
        }

        public string GetTooltipText()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(_effects.GetTooltipText());
            
            return sb.ToString();
        }

        public string GetItemWidgetText()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Max Uses: ");
            sb.Append(_maxUses);

            return sb.ToString();
        }
    }
}