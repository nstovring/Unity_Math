using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ComputeShaderTest : MonoBehaviour
{
    public ComputeShader compute_shader;
    public RenderTexture render_texture;
    public RenderTexture dye_texture;

    public RenderTexture velocity_texture;


    int SetColorWhiteKernelIndex = 1;
    int MainKernelIndex = 0;

    [Range(-20f,20f)]
    public float diffusion_factor = 1;

    public float sinPow = 1;

    // Start is called before the first frame update
    void Start()
    {
        InitTexture(out render_texture);
        InitTexture(out velocity_texture);
        InitTexture(out dye_texture);


        compute_shader.SetTexture(SetColorWhiteKernelIndex, "Result", render_texture);
        compute_shader.SetTexture(SetColorWhiteKernelIndex, "Dye", dye_texture);

        compute_shader.SetTexture(MainKernelIndex, "Result", render_texture);
        compute_shader.SetTexture(MainKernelIndex, "Dye", dye_texture);


        compute_shader.Dispatch(SetColorWhiteKernelIndex,render_texture.width/8, render_texture.height/8, 1);


    }



    void InitTexture(out RenderTexture r)
    {
        r = new RenderTexture(256, 256, 1);
        r.enableRandomWrite = true;
        r.Create();
    }

    // Update is called once per frame
    void Update()
    {
        //diffusion_factor += Mathf.Sin(Time.time) * sinPow;

        if (Input.GetMouseButton(0))
        {
            compute_shader.Dispatch(SetColorWhiteKernelIndex, render_texture.width / 8, render_texture.height / 8, 1);
        }
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest){
        if(render_texture == null){
             render_texture = new RenderTexture(256,256,1);
            render_texture.enableRandomWrite = true;
            render_texture.Create();
        }

        //compute_shader.SetTexture(MainKernelIndex, "Result", render_texture);
        compute_shader.SetFloat("deltaTime", Time.deltaTime);
        compute_shader.SetFloat("diffusion_factor", diffusion_factor);

        compute_shader.Dispatch(MainKernelIndex,render_texture.width/8, render_texture.height/8, 1);

        Graphics.Blit(render_texture, dest);
    }

    void OnDisable(){
        render_texture.Release();
        dye_texture.Release();
        velocity_texture.Release();
    }


}
