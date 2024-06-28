using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class ImageMoveAnimationUI : OSK.SingletonMono<ImageMoveAnimationUI>
{
    public Canvas canvas;
    public GameObject[] iconClones;

    public int indexIcon = 0;
    public Vector2 randomX = new Vector2(-30f, 30f);
    public Vector2 randomY = new Vector2(-20f, -30f);

    public float timeDrop = 0.15f;
    public float speedDrop = 1.5f;

    public float timeFly = 1;
    public float speedFly = 3;

    public int numberOfCoins = 10;
    public float delayMove = 0.15f;

    public float timeDestroyed = 1.25f;
    public float delaySetOnCompleted = 1;

    public System.Action onCompleted;
    private GameObject coinParent;

    public Transform pointSpawn;
    public Vector3 startPosition;
    public Transform target;
 


    private void Start()
    {
        for (int i = 0; i < iconClones.Length; i++)
        {
            iconClones[i].SetActive(false);
        }
    }
    
    public void SpawnImageWithTransform(int idx , Transform posInit, Transform posIcon, System.Action onCompleted = null)
    {
        new SpawnImageBuilder()
            .SetIndexIcon(idx)
            .SetStartTransform(posInit)
            .SetEndPoint(posIcon)
            .SetRandomX(new Vector2(-30f, 30f))
            .SetRandomY(new Vector2(-20f, -30f))
            .SetTimeDrop(.1f)
            .SetSpeedDrop(1)
            .SetTimeFly(1)
            .SetSpeedFly(3)
            .SetNumberOfCoins(10)
            .SetDelayMove(0.15f)
            .SetTimeDestroyed(1.25f)
            .SetOnCompleted(1,() =>
            {
                onCompleted?.Invoke();
            }).BuildWithTransform();
    } 
    
    public void SpawnImageWithPosition(int idx, Transform posInit, Transform posIcon, System.Action onCompleted = null)
    {
        new SpawnImageBuilder()
            .SetIndexIcon(idx)
            .SetStartTransform(posInit)
            .SetEndPoint(posIcon)
            .SetRandomX(new Vector2(-30f, 30f))
            .SetRandomY(new Vector2(-20f, -30f))
            .SetTimeDrop(.1f)
            .SetSpeedDrop(1)
            .SetTimeFly(1)
            .SetSpeedFly(3)
            .SetNumberOfCoins(10)
            .SetDelayMove(0.15f)
            .SetTimeDestroyed(1.25f)
            .SetOnCompleted(1,() =>
            {
                onCompleted?.Invoke();
            }).BuildWithPosition();
    }

    public void SpawnImageWithPosition()
    {
            StartCoroutine(DelaySpawnCoins(startPosition));
    }


    public void SpawnImagesTransform()
    {
            StartCoroutine(DelaySpawnCoins(pointSpawn.position));
    }

    private IEnumerator DelaySpawnCoins(Vector3 startPoint)
    {
        coinParent = new GameObject("iconMoveParent");
        coinParent.transform.parent = canvas.transform;
        coinParent.transform.localPosition = Vector3.zero;
        coinParent.transform.localScale = Vector3.one;
        Destroy(coinParent, timeDestroyed);

        float timeDlaySpawn = 0;

        for (int i = 0; i < numberOfCoins; i++)
        {
            var coinClone = Instantiate(iconClones[indexIcon], startPoint, Quaternion.identity, coinParent.transform);
            coinClone.gameObject.SetActive(true);
            StartCoroutine(DropCoins(coinClone, startPoint));

            float randomTime = Random.Range(0.01f, 0.05f);
            timeDlaySpawn += randomTime;
            yield return new WaitForSeconds(randomTime);
            StartCoroutine(MoveCoinToTarget(coinClone.transform));
        }

        yield return new WaitForSeconds(timeDlaySpawn + delaySetOnCompleted);
        onCompleted?.Invoke();
    }


    private IEnumerator DropCoins(GameObject image, Vector3 startPoint)
    {
        float timer = 0f;
        Vector3 randomOffset = new Vector2(Random.Range(randomX.x, randomX.y), Random.Range(randomY.x, randomY.y));
        Vector3 spawnPos = startPoint + randomOffset;
        while (timer < timeDrop)
        {
            float t = timer / speedDrop;
            image.transform.position = Vector3.MoveTowards(image.transform.position, spawnPos, t);
            timer += Time.deltaTime;
            yield return null;
        }
        // AudioManager.Instance.PlayOneShot("coin_collect_appear", 1, 1);   
    }

    private IEnumerator MoveCoinToTarget(Transform imageTransform)
    {
        float timer = 0f;
        yield return new WaitForSeconds(delayMove + Random.Range(0.01f, 0.05f));
        while (timer < timeFly)
        {
            float t = timer / speedFly;
            if (imageTransform != null && imageTransform.gameObject.activeInHierarchy)
                imageTransform.position = Vector3.MoveTowards(imageTransform.position, target.position, t);
            timer += Time.deltaTime;
            yield return null;
        }

        if (imageTransform != null && imageTransform.gameObject.activeInHierarchy)
        {
            //AudioManager.Instance.PlayOneShot("coin_collect_appear", 1, 1);   
            imageTransform.position = target.position;
        }
    }
}

public class SpawnImageBuilder
{
    private ImageMoveAnimationUI _img;

    public SpawnImageBuilder()
    {
        _img = ImageMoveAnimationUI.Instance;
    }

    public SpawnImageBuilder SetIndexIcon(int indexIcon)
    {
        _img.indexIcon = indexIcon;
        return this;
    }


    public SpawnImageBuilder SetStartTransform(Transform startPoint)
    {
        _img.pointSpawn = startPoint;
        return this;
    }


    public SpawnImageBuilder SetStartPosition(Transform transform, RectTransform rectTransformCanvas)
    {
        var convertPos = OSK.Utils.CanvasUtils.WorldToCanvasPosition(rectTransformCanvas, Camera.main, transform);
        _img.startPosition = convertPos;
        return this;
    }

    public SpawnImageBuilder SetEndPoint(Transform endPoint)
    {
        _img.target = endPoint;
        return this;
    }

    public SpawnImageBuilder SetOnCompleted(float delay, System.Action onCompleted)
    {
        _img.delaySetOnCompleted = delay;
        _img.onCompleted = onCompleted;
        return this;
    }

    public SpawnImageBuilder SetRandomX(Vector2 randomX)
    {
        _img.randomX = randomX;
        return this;
    }

    public SpawnImageBuilder SetRandomY(Vector2 randomY)
    {
        _img.randomY = randomY;
        return this;
    }

    public SpawnImageBuilder SetTimeDrop(float timeDrop)
    {
        _img.timeDrop = timeDrop;
        return this;
    }

    public SpawnImageBuilder SetSpeedDrop(float speedDrop)
    {
        _img.speedDrop = speedDrop;
        return this;
    }

    public SpawnImageBuilder SetTimeFly(float timeFly)
    {
        _img.timeFly = timeFly;
        return this;
    }

    public SpawnImageBuilder SetSpeedFly(float speedFly)
    {
        _img.speedFly = speedFly;
        return this;
    }

    public SpawnImageBuilder SetNumberOfCoins(int numberOfCoins)
    {
        _img.numberOfCoins = numberOfCoins;
        return this;
    }

    public SpawnImageBuilder SetDelayMove(float delayMove)
    {
        _img.delayMove = delayMove;
        return this;
    }

    public SpawnImageBuilder SetTimeDestroyed(float timeDestroyed)
    {
        _img.timeDestroyed = timeDestroyed;
        return this;
    }


    public void BuildWithTransform()
    {
        _img.SpawnImagesTransform();
    }

    public void BuildWithPosition()
    {
        _img.SpawnImageWithPosition();
    }
}