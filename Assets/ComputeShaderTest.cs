using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ComputeShaderTest : MonoBehaviour
{
    public ComputeShader compute_shader;
    public RenderTexture render_texture;
    // Start is called before the first frame update
    void Start()
    {
        render_texture = new RenderTexture(256,256,1);
        render_texture.enableRandomWrite = true;
        render_texture.Create();

        compute_shader.SetTexture(0, "Result", render_texture);
        compute_shader.Dispatch(0,render_texture.width/8, render_texture.height/8, 1);


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest){
        if(render_texture == null){
             render_texture = new RenderTexture(256,256,1);
            render_texture.enableRandomWrite = true;
            render_texture.Create();
        }

        compute_shader.SetTexture(0, "Result", render_texture);
        compute_shader.Dispatch(0,render_texture.width/8, render_texture.height/8, 1);

        Graphics.Blit(render_texture, dest);
    }

}
