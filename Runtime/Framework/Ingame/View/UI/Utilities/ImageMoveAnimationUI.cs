using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class ImageMoveAnimationUI : OSK.SingletonMono<ImageMoveAnimationUI>
{
    public Canvas canvas;
    public GameObject[] iconClones;

    public int indexIcon { get; set; }
    public Vector2 randomX { get; set; }
    public Vector2 randomY { get; set; }


    public float timeDrop { get; set; }
    public float speedDrop { get; set; }

    public float timeFly { get; set; }
    public float speedFly { get; set; }

    public int numberOfCoins { get; set; }
    public float delayMove { get; set; }

    public float timeDestroyed { get; set; }
    public float delaySetOnCompleted { get; set; }

    public System.Action onCompleted;
    private GameObject coinParent;

    public Transform pointSpawn;
    public Vector3 startPosition;
    public Transform target;

    /* example of usage:
        new SpawnImageBuilder()
            .SetIndexIcon(0)
            .SetStartPosition(convertPos)
            .SetEndPoint(gold.GetPosIcon())
            .SetRandomX(new Vector2(-30f, 30f))
            .SetRandomY(new Vector2(-20f, -30f))
            .SetTimeDrop(.1f)
            .SetSpeedDrop(1)
            .SetTimeFly(1)
            .SetSpeedFly(3)
            .SetNumberOfCoins(10)
            .SetDelayMove(0.15f)
            .SetTimeDestroyed(1.25f)
            .SetOnCompleted(() =>
            {
                DialogManager.Hide<GoldUI>();
                Destroy(gameObject);
            }).BuildWithPosition();
     */


    private void Start()
    {
        for (int i = 0; i < iconClones.Length; i++)
        {
            iconClones[i].SetActive(false);
        }
    }

    public void SpawnImageWithPosition()
    {
        if (coinParent == null)
            StartCoroutine(DelaySpawnCoins(startPosition));
    }


    public void SpawnImagesTransform()
    {
        if (coinParent == null)
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


    public SpawnImageBuilder SetStartPosition(Vector3 startPosition)
    {
        _img.startPosition = startPosition;
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