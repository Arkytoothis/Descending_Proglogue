using System.Collections;
using System.Collections.Generic;
using System.Text;
using DarkTonic.MasterAudio;
using Descending.Core;
using UnityEngine;

namespace Descending.Equipment
{
    [System.Serializable]
    public class WeaponData
    {
        [SerializeField] private bool _hasData = true;
        [SerializeField] private WeaponTypes _weaponType = WeaponTypes.None;
        [SerializeField] private DamageClasses _damageClass = DamageClasses.None;
        [SerializeField] private int _range = 1;
        [SerializeField] private float _projectileDelay = 1f;
        [SerializeField] private DamageTypeDefinition _damageType = null;
        [SerializeField] private int _minDamage = 0;
        [SerializeField] private int _maxDamage = 0;
        [SerializeField] private ProjectileDefinition _projectile = null;
        [SerializeField] private GameObject _attackEffectPrefab = null;
        [SerializeField] private AnimatorOverrideController _animatorOverride = null;

        [SoundGroup] public List<string> HitSounds;
        [SoundGroup] public List<string> MissSounds;
        
        public bool HasData => _hasData;
        public ProjectileDefinition Projectile => _projectile;
        public WeaponTypes WeaponType => _weaponType;
        public DamageClasses DamageClass => _damageClass;
        public int Range => _range;
        public float ProjectileDelay => _projectileDelay;
        public DamageTypeDefinition DamageType => _damageType;
        public int MinDamage => _minDamage;
        public int MaxDamage => _maxDamage;
        public AnimatorOverrideController AnimatorOverride => _animatorOverride;
        public GameObject AttackEffectPrefab => _attackEffectPrefab;

        public WeaponData(WeaponData weaponData)
        {
            _hasData = weaponData._hasData;
            _projectileDelay = weaponData._projectileDelay;
            _range = weaponData._range;
            _weaponType = weaponData._weaponType;
            _animatorOverride = weaponData.AnimatorOverride;
            _damageType = weaponData._damageType;
            _minDamage = weaponData._minDamage;
            _maxDamage = weaponData._maxDamage;
            _damageType = weaponData._damageType;
        }

        public string GetTooltipText()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Range: ");
            sb.Append(_range);
            sb.AppendLine();

            sb.Append(_minDamage);
            sb.Append("-");
            sb.Append(_maxDamage);
            sb.Append(" ");

            if (_damageType != null)
            {
                sb.Append(_damageType.Name);
                sb.Append(" damage");
            }

            return sb.ToString();
        }
        
        public string GetItemWidgetText()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Range ");
            sb.Append(_range);
            sb.AppendLine();
            sb.Append(_minDamage);
            sb.Append("-");
            sb.Append(_maxDamage);
            sb.Append(" ");

            if (_damageType != null)
            {
                sb.Append(_damageType.Name);
                sb.Append(" damage");
            }

            return sb.ToString();
        }
    }
}