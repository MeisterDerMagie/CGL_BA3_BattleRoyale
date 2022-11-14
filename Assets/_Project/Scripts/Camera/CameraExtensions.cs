using UnityEngine;


namespace Wichtel.Extensions {
public static class CameraExtensions
{
    /// <summary>
    /// Returns the orthographic camera bounds
    /// </summary>
    /// <param name="_camera"></param>
    /// <returns></returns>
    public static Bounds OrthographicBounds(this Camera _camera)
    {
        if (!_camera.orthographic)
        {
            Debug.LogError($"The camera {_camera.name} is not Orthographic!", _camera);
            return new Bounds();
        }
 
        Transform camTrans = _camera.transform;
        float scaleWidthFactor = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;
        float height = _camera.orthographicSize * 2f;
        float width = height * scaleWidthFactor;
        float viewDepth = _camera.farClipPlane - _camera.nearClipPlane;

        Vector3 camPosition = camTrans.position;
        return new Bounds(new Vector3(camPosition.x, camPosition.y, camPosition.z), new Vector3(width, height, viewDepth));
    }
 
    /// <summary>
    /// Returns the orthographic camera rect (without z)
    /// </summary>
    /// <param name="_camera"></param>
    /// <returns></returns>
    public static Rect OrthographicRect(this Camera _camera)
    {
        if (!_camera.orthographic)
        {
            Debug.LogError($"The camera {_camera.name} is not Orthographic!", _camera);
            return new Rect();
        }
 
        Transform camTrans = _camera.transform;
        float scaleWidthFactor = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;
        float height = _camera.orthographicSize * 2f;
        float width = height * scaleWidthFactor;

        Vector3 camPosition = camTrans.position;
        return new Rect(new Vector2(camPosition.x - (width*0.5f), camPosition.y - (height * 0.5f)), new Vector2(width, height));
    }
}
}