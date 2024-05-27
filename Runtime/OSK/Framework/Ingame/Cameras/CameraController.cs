using UnityEngine;


public class CameraController : OSK.SingletonMono<CameraController>
{

    /* ------------------------ Inspector Assigned Fields ----------------------- */

    public Camera cameraMain;
    //private Tweener camShake;

    /* ------------------------------ Unity Events ------------------------------ */

    protected void Start()
    {
        OSK.Observer.Add("ShakeCamera", ShakeCamera);
    }

    protected void OnDestroy()
    {
        OSK.Observer.Remove("ShakeCamera", ShakeCamera);
    }


    public void ShakeCamera(object data)
    {
        // if (camShake != null) camShake.Kill();
        // camShake = cameraMain.transform.DOShakePosition(_data.shakeData.duration, _data.shakeData.strenght, _data.shakeData.vibrato)
        //     .SetEase(_data.shakeData.ease).OnComplete(() =>
        //     {
        //         cameraMain.transform.DOLocalMove(Vector3.zero, _data.shakeData.fadeOutTime).SetEase(_data.shakeData.ease);
        //     });
    }
}
