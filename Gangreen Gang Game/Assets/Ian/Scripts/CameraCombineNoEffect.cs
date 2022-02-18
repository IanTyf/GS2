using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCombineNoEffect : MonoBehaviour
{
    public Material combineMat;

    private Camera cam;
    public RenderTexture _rt;

    // Use this for initialization
    void Start()
    {
        //material = new Material(Shader.Find("Custom/GlitchEffectShader"));
        cam = GetComponent<Camera>();
        //cam.targetTexture.width = Screen.width;
        //cam.targetTexture.height = Screen.height;
    }

    private void Update()
    {
        int resWidth = Screen.width;
        int resHeight = Screen.height;

        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        cam.targetTexture = rt; //Create new renderTexture and assign to camera
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false); //Create new texture

        cam.Render();

        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0); //Apply pixels from camera onto Texture2D

        cam.targetTexture = null;
        RenderTexture.active = null; //Clean
        Destroy(rt); //Free memory
        cam.targetTexture = _rt;

        combineMat.SetTexture("_OverlayTex2", screenShot);
    }
}
