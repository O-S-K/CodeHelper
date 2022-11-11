using UnityEngine;
using UnityEngine.UI;
using TMPro;
public static class Extensions
{
    public static float WrapAngle(this float angle)
    {
        angle %= 360;
        if (angle > 180)
            return angle - 360;

        return angle;
    }

    public static T ChangeAlpha<T>(this T g, float newAlpha) where T : Graphic
    {
        var color = g.color;
        color.a = newAlpha;
        g.color = color;
        return g;
    }

    public static void SetTextFade(this Text text, float value)
    {
        Color color = text.color;
        color.a = value;
        text.color = color;
    }
    /// <summary>
    /// SetAlpha: 0 -> 1
    /// </summary>
    /// <param name="text"></param>
    /// <param name="value"></param>
    public static void SetTextFade(this TextMeshProUGUI text, float value)
    {
        Color color = text.color;
        color.a = value;
        text.color = color;
    }

    public static void AnulateRotationExceptY(this Transform transform)
    {
        Vector3 rotation = transform.eulerAngles;
        rotation.x = 0;
        rotation.z = 0;
        transform.eulerAngles = rotation;
    }

    /// <summary>
    /// SetAlpha: 0 -> 1
    /// </summary>
    /// <param name="color"></param>
    /// <param name="alpha"></param>
    /// <returns></returns>
    public static Color SetAlpha(this Color color, float alpha)
    {
        color.a = alpha;
        return color;
    }
    public static void SetAlpha(UnityEngine.UI.Graphic graphic, float alpha)
    {
        Color color = graphic.color;
        color.a = alpha;
        graphic.color = color;
    }


    public static void SetColorMaterial(this Material material, string nameID, Color color)
    {
        var nameId = Shader.PropertyToID(nameID);
        material.SetColor(nameId, color);
    }

    public static void SetColorMaterials(this Material[] materials, string nameID, Color color)
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].SetColor(Shader.PropertyToID(nameID), color);
        }
    }

    // LAYER
    public static void SetLayerAllChildren(this GameObject gameobject, string nameLayer)
    {
        foreach (GameObject child in gameobject.GetComponentsInChildren<GameObject>())
        {
            child.layer = LayerMask.NameToLayer(nameLayer);
        }
    }

    public static void SetLayer(this GameObject gameObject, int layer, bool applyToChildren = false)
    {
        gameObject.layer = layer;
        if (applyToChildren)
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                SetLayer(gameObject.transform.GetChild(i).gameObject, layer, true);
            }
        }
    }

    public static void SetLayer(this GameObject gameObject, string nameLayer)
    {
        gameObject.layer = LayerMask.NameToLayer(nameLayer);
    }


    public static void DestroyAllChildren(this Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            GameObject.Destroy(parent.GetChild(i).gameObject);
        }
    }
}
