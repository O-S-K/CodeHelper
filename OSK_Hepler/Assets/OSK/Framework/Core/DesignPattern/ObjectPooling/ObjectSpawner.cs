using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OSK
{
	public class ObjectSpawner : UIMonoBehaviour
	{
		[SerializeField] protected SpawnObject	spawnObjectPrefab;
		[SerializeField] protected float		spawnRate;

		protected ObjectPool	spawnObjectPool;
		protected float			timer;

		protected virtual void Start()
		{
			spawnObjectPool = new ObjectPool(spawnObjectPrefab.gameObject, 0, transform, ObjectPool.PoolBehaviour.CanvasGroup);
		}

		protected virtual void Update()
		{
			timer -= Time.deltaTime;

			if (timer <= 0)
			{
				SpawnObject();

				timer = spawnRate;
			}
		}

		protected virtual void SpawnObject()
		{
			spawnObjectPool.GetObject<SpawnObject>().Spawned();
		}
	}
}
