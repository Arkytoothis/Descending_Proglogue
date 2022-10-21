using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Descending.Treasure
{
    public enum CoinTypes { Copper, Silver, Gold, Mithril }
    public enum GemTypes { Sapphire, Emerald, Ruby, Diamond }
    
    public class TreasureManager : MonoBehaviour
    {
        public static TreasureManager Instance { get; private set; }

        [SerializeField] private List<GameObject> _coinPrefabs = null;
        [SerializeField] private List<GameObject> _gemPrefabs = null;
        [SerializeField] private Transform _coinsParent = null;
        [SerializeField] private Transform _gemsParent = null;
        [SerializeField] private float _torqueModifier = 10f;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Multiple Treasure Managers " + transform + " - " + Instance);
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
        }

        public void SpawnCoins(Vector3 spawnPosition, int amount, CoinTypes coinType)
        {
            StartCoroutine(SpawnCoins_Coroutine(spawnPosition, amount, coinType));
        }

        public void SpawnGems(Vector3 spawnPosition, int amount, GemTypes gemType)
        {
            StartCoroutine(SpawnGems_Coroutine(spawnPosition, amount, gemType));
        }
        
        private IEnumerator SpawnCoins_Coroutine(Vector3 spawnPosition, int amount, CoinTypes coinType)
        {
            yield return new WaitForSeconds(0.2f);
            
            for (int i = 0; i < amount; i++)
            {
                float xOffset = Random.Range(-0.1f, 0.1f);
                float yOffset = Random.Range(2f, 2.2f);
                float zOffset = Random.Range(-0.1f, 0.1f);
                GameObject clone = Instantiate(_coinPrefabs[(int) coinType], _coinsParent);
                clone.transform.position = new Vector3(spawnPosition.x + xOffset, spawnPosition.y + yOffset, spawnPosition.z + zOffset);
                
                Rigidbody rigidbody = clone.GetComponent<Rigidbody>();
                rigidbody.AddTorque(new Vector3(Random.Range(-_torqueModifier, _torqueModifier), Random.Range(-_torqueModifier, _torqueModifier), Random.Range(-_torqueModifier, _torqueModifier)));
                //rigidbody.AddExplosionForce(50f, transform.position, 10f, 5f);
            }
        }
        
        private IEnumerator SpawnGems_Coroutine(Vector3 spawnPosition, int amount, GemTypes gemTypes)
        {
            yield return new WaitForSeconds(0.2f);
            
            for (int i = 0; i < amount; i++)
            {
                float xOffset = Random.Range(-0.1f, 0.1f);
                float yOffset = Random.Range(2f, 2.2f);
                float zOffset = Random.Range(-0.1f, 0.1f);
                GameObject clone = Instantiate(_gemPrefabs[(int) gemTypes], _coinsParent);
                clone.transform.position = new Vector3(spawnPosition.x + xOffset, spawnPosition.y + yOffset, spawnPosition.z + zOffset);
                
                Rigidbody rigidbody = clone.GetComponent<Rigidbody>();
                rigidbody.AddTorque(new Vector3(Random.Range(-_torqueModifier, _torqueModifier), Random.Range(-_torqueModifier, _torqueModifier), Random.Range(-_torqueModifier, _torqueModifier)));
                //rigidbody.AddExplosionForce(50f, transform.position, 10f, 5f);
            }
        }
    }
}