using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraCombine : MonoBehaviour
{

    public Material material;

    // Start is called before the first frame update
    void Start()
    {
        //material = new Material(Shader.Find("Hidden/CameraCombine"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {

        //material.SetFloat("_ChromAberrAmountX", redOffset.x);
        //material.SetVector("u_greenOffset", greenOffset);
        //material.SetVector("u_blueOffset", blueOffset);
        Graphics.Blit(source, destination, material);
    }
}
