using UnityEngine;

public class TapSpawnFlyText : ElementUI
{
    public GameObject flyTextPrefab;
    public Transform parent;

    public float speedFlyText = 50f;
    public float duration = 0.5f;
    public void SpawnFlyText()
    {
        GameObject flyText = Instantiate(flyTextPrefab, parent);
        flyText.transform.position = transform.position + Vector3.up;
        flyText.GetComponent<FlyingText>().Init(Random.Range(-1, -99).ToString(), speedFlyText, duration);
    }
}
