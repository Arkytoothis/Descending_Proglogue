using System;
using System.Collections;
using System.Collections.Generic;
using Descending.Core;
using Descending.Equipment;
using Descending.Tiles;
using Descending.Treasure;
using DG.Tweening;
using UnityEditor.Experimental;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Descending.Interactables
{
    public class TreasureChest : MonoBehaviour, IInteractable
    {
        [SerializeField] private bool _isActive = false;
        [SerializeField] private GameObject _gameObjectToHide = null;
        [SerializeField] private Collider _collider = null;
        [SerializeField] private Transform _lidTransform = null;
        [SerializeField] private Transform _itemSpawnTransform = null;
        [SerializeField] private float _activateDuration = 1f;
        [SerializeField] private float _activateAngle = 50f;
        [SerializeField] private List<DropData> _coinData;
        [SerializeField] private List<DropData> _gemData;
        [SerializeField] private ItemDefinition _itemDropDefinition;
        
        private bool _treasureDropped = false;
        private MapPosition _mapPosition;
        private float _timer;
        private bool _isInteracting;
        private List<ItemDrop> _itemDrops = null;
        
        private Action onComplete;

        private void Start()
        {
            InteractableManager.Instance.RegisterInteractable(this);
        }

        public void Setup()
        {
            _mapPosition = MapManager.Instance.GetGridPosition(transform.position);
            MapManager.Instance.SetInteractableAtGridPosition(_mapPosition, this);
            PathfindingManager.Instance.SetIsGridPositionWalkable(_mapPosition, false);
            _isInteracting = false;
            _itemDrops = new List<ItemDrop>();
        }

        private void Update()
        {
            if (!_isInteracting) return;
            
            _timer -= Time.deltaTime;

            if (_timer <= 0f)
            {
                _isInteracting = false;
                onComplete();
            }
        }

        public void Interact(Action onInteractionComplete)
        {
            //Debug.Log("Interacting with Treasure Chest");
            onComplete = onInteractionComplete;
            _isInteracting = true;
            _timer = 0.5f;
            
            if(_isActive == false)
            {
                Activate();
            }
        }

        public string GetName()
        {
            return "Treasure Chest";
        }

        public void Activate()
        {
            _isActive = true;
            MapManager.Instance.SetInteractableAtGridPosition(_mapPosition, null);
            PathfindingManager.Instance.SetIsGridPositionWalkable(_mapPosition, true);
            _lidTransform.DORotate(new Vector3(_activateAngle, 0f, 0f), _activateDuration, RotateMode.LocalAxisAdd);

            StartCoroutine(HideAndDestroy_Coroutine(1f, 3f));
        }
        
        public void DropTreasure()
        {
            if (_treasureDropped == true) return;
            
            _treasureDropped = true;
            
            TryDropCoins(CoinTypes.Copper);
            TryDropCoins(CoinTypes.Silver);
            TryDropCoins(CoinTypes.Gold);
            TryDropCoins(CoinTypes.Mithril);

            TryDropGems(GemTypes.Sapphire);
            TryDropGems(GemTypes.Ruby);
            TryDropGems(GemTypes.Emerald);
            TryDropGems(GemTypes.Diamond);
            
            TryDropItems();
        }

        private void TryDropCoins(CoinTypes coinType)
        {
            DropData dropData = _coinData[(int) coinType];
            
            if (Random.Range(0, 100) < dropData.Chance)
            {
                TreasureManager.Instance.SpawnCoins(transform.position, Random.Range(dropData.Minimum, dropData.Maximum), coinType, 0f);
            }
        }

        private void TryDropGems(GemTypes gemType)
        {
            DropData dropData = _gemData[(int) gemType];
            
            if (Random.Range(0, 100) < dropData.Chance)
            {
                TreasureManager.Instance.SpawnGems(transform.position, Random.Range(dropData.Minimum, dropData.Maximum), gemType, 0f);
            }
        }
        
        private void TryDropItems()
        {
            string itemKey = ItemGenerator.GetRandomKeyByType(GenerateItemType.Any);
            ItemDefinition itemDefinition = Database.instance.Items.GetItem(itemKey);
            ItemDrop itemDrop = TreasureManager.Instance.SpawnItemDrop(itemDefinition, _itemSpawnTransform.position);
            
            _itemDrops.Add(itemDrop);
        }

        private IEnumerator HideAndDestroy_Coroutine(float delay, float destroyAfter)
        {
            yield return new WaitForSeconds(delay);
            
            DropTreasure();
            _gameObjectToHide.SetActive(false);
            _collider.enabled = false;
            Destroy(gameObject, destroyAfter);
        }
    }
}