using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class GeneratePlanet : MonoBehaviour
{
    public int[] CompelxityOfPlanets;
    private int MeshNumber;
    private Mesh mesh;
    private Vector3[] vertices;
    private Color[] colors;
    private int[] triangles;
    public int Complexity = 6;
    private int howManyPoints;
    private float a;
    private float a2;
    public float PerlinoiseIntensity;
    public float PerlinoiseDestortion;
    public float PerlinoiseIntensity2;
    public float PerlinoiseDestortion2;
    public float PerlinoiseIntensity3;
    public float PerlinoiseDestortion3;
    public float TerrainIntensity;
    public float TerrainDestortion;
    private Vector2[] uvs;
    private float PerlinNoisewas = 1;
    public float whitness;
    public Color PlanetColor;
    public Color MountainColor;
    public float snowHight;
    public float PerlinoiseIntensityBiom;
    public float PerlinoiseDestortionBiom;
    public Color BiomColor;
    public float BiomColorIntensity;
    public float BiomGenerateHight;


    void OneGeneratePlanet()
    {
        
        Color colorDiference = new Color(MountainColor.r -PlanetColor.r  ,  MountainColor.g - PlanetColor.g ,   MountainColor.b - PlanetColor.b, 1);
        vertices = new Vector3[(Complexity * Complexity + 1 )* 2];
        colors = new Color[vertices.Length];
        uvs = new Vector2[vertices.Length];

        triangles = new int[(Complexity * Complexity * 6 )* 2];

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //whitness = whitness / 10;
        float r1 = (0.3f * Complexity);
        float z = 0f;
        a2 = 0;
        for (int i = 0; i < Complexity+1; i++)
        {

            float r = Mathf.Sqrt(2 * r1 * z - z * z);

            for (int d = 0; d < Complexity; d++)
            {

                a+= (2*Mathf.PI / Complexity);
                if(i == Complexity)
                {
                    r = 0;
                }
                float x = r *Mathf.Sin(a);
                float y = r *Mathf.Cos(a);
                //PerlinoiseIntensity = PerlinoiseIntensity /(z - r1);

                float aPerlin  =  a ;

                float PerlinNoyse1 = Mathf.PerlinNoise((a2 - (a2 * 2)) * PerlinoiseDestortion, (aPerlin - (aPerlin * 2)) * PerlinoiseDestortion) * (PerlinoiseIntensity / 10);
                float PerlinNoyse2 = Mathf.PerlinNoise((a2 - (a2 * 2)) * PerlinoiseDestortion2, (aPerlin - (aPerlin * 2)) * PerlinoiseDestortion2) * (PerlinoiseIntensity2 / 10);
                float PerlinNoyse3 = Mathf.PerlinNoise((a2 - (a2 * 2)) * PerlinoiseDestortion3, (aPerlin - (aPerlin * 2)) * PerlinoiseDestortion3) * (PerlinoiseIntensity3 / 10);
                float Terrein = Mathf.PerlinNoise((a2 - (a2 * 2)) * TerrainDestortion, (aPerlin - (aPerlin * 2)) * TerrainDestortion) * (TerrainIntensity / 10);

                float PerlinNoyse = PerlinNoyse1 * PerlinNoyse2* PerlinNoyse3 - Terrein;
                float maxPerlinNoyse = ((PerlinoiseIntensity / 10) * (PerlinoiseIntensity2 / 10) * (PerlinoiseIntensity3 / 10)) ;

                if (i == Complexity | i == 0)
                {
                    PerlinNoyse = maxPerlinNoyse / 4;
                }else if(d == 0)
                {
                    PerlinNoisewas = PerlinNoyse;
                }else if(d> Complexity *0.90)
                {
                    PerlinNoyse += (PerlinNoisewas - PerlinNoyse) /  ( ( Complexity * 0.10f ) - ((float)d - Complexity * 0.90f) )  ;
                }
                //float z = r * Mathf.Sin(a2);
                //float y2 = r * Mathf.Cos(a2);
                vertices[i * Complexity + d ] = new Vector3(x + x *PerlinNoyse, y +  y *PerlinNoyse, (z - r1) +  (z- r1 )* PerlinNoyse  );
                if (snowHight > PerlinNoyse)
                {
                    colors[i * Complexity + d] = new Color(PlanetColor.r  , PlanetColor.g , PlanetColor.b , PlanetColor.a);
                }
                else
                {
                    colors[i * Complexity + d] = new Color(PlanetColor.r + (PerlinNoyse / maxPerlinNoyse) * colorDiference.r*whitness, PlanetColor.g + (PerlinNoyse / maxPerlinNoyse) * colorDiference.g*whitness, PlanetColor.b + (PerlinNoyse / maxPerlinNoyse) * colorDiference.b * whitness, PlanetColor.a);
                    //colors[i * Complexity + d] = new Color(PlanetColor.r * (PerlinNoyse / whitness * colorDiference.r), PlanetColor.g * (PerlinNoyse / whitness * colorDiference.g), PlanetColor.b * (PerlinNoyse / whitness * colorDiference.b), PlanetColor.a);
                    //colors[i * Complexity + d] = new Color(PlanetColor.r * (PerlinNoyse / whitness), PlanetColor.g * (PerlinNoyse / whitness), PlanetColor.b * (PerlinNoyse / whitness), PlanetColor.a);
                }
                float PerlinNoyseBiom = Mathf.PerlinNoise((a2 - (a2 * 2)) * PerlinoiseDestortionBiom+10000, (aPerlin - (aPerlin * 2)) * PerlinoiseDestortionBiom + 10000) * (PerlinoiseIntensityBiom / 10);

                if ((PerlinNoyse / maxPerlinNoyse) < BiomGenerateHight)
                {
                    colors[i * Complexity + d] = new Color(colors[i * Complexity + d].r + PerlinNoyseBiom * (BiomColor.r - colors[i * Complexity + d].r ) * BiomColorIntensity, colors[i * Complexity + d].g + PerlinNoyseBiom * (  BiomColor.g - colors[i * Complexity + d].g) * BiomColorIntensity, colors[i * Complexity + d].b + PerlinNoyseBiom * (  BiomColor.b - colors[i * Complexity + d].b) * BiomColorIntensity, PlanetColor.a);
                }
                //colors[i * Complexity + d] = new Color(1 + PlanetColor.r / ((maxPerlinNoyse-PerlinNoyse) / whitness), 1 + PlanetColor.g  / ((maxPerlinNoyse - PerlinNoyse) / whitness), 1 + PlanetColor.b / ((maxPerlinNoyse - PerlinNoyse) / whitness), PlanetColor.a);
                //vertices[i * Complexity + d + howManyPointsHad] = new Vector3(i* VertexSpacing - size / 2  , d* VertexSpacing - size / 2 , 0 );
                howManyPoints = i * Complexity + d;

            }
            PerlinNoisewas = 1;
            if (r !=0 )
            {
                a2 += ((Mathf.PI / Complexity) / r) * r1;
            }
            else
            {
                a2 += Mathf.PI / Complexity;
            }
            
            a = 0;
            z += (2 * r1 / Complexity);
            // z = (Mathf.Sin(Mathf.PI / Complexity * i - Mathf.PI / 2) + 1) * (2 * r1 / Complexity) * i;

        }


        int j = 0;
        for (int k = 0; k < Complexity * Complexity * 6; k += 6)
        {
            if (howManyPoints > (j / Complexity) + Complexity)
            {
                triangles[  k+1] =   j / Complexity;
                triangles[  k ] =   (j / Complexity) + 1;
                triangles[  k + 2] =   (j / Complexity) + Complexity;

                triangles[  k + 3] =   (j / Complexity) + Complexity;
                triangles[  k + 5] =   (j / Complexity) + 1;
                triangles[  k + 4] =   (j / Complexity) + Complexity + 1;
                j += Complexity;
            }

        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // for (int i = 0; i < colors.Length; i++)
        // {
        //colors[i] = new Color(0.45f, 0.3491f, 0.228f, 1);
        //}



        

        mesh = transform.GetChild(MeshNumber).GetComponent<MeshFilter>().mesh;
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.vertices = vertices;
        mesh.colors = colors;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        MeshCollider MeshCollider = transform.GetChild(MeshNumber).gameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;

        CompelxityOfPlanets[MeshNumber] = Complexity;


        // transform.GetChild(0).position += new Vector3(size / 2 , 0);
        // transform.GetChild(1).position -= new Vector3(size / 2 , 0);
        // transform.GetChild(2).position += new Vector3(0, size / 2 , 0);
        // transform.GetChild(3).position -= new Vector3(0, size / 2 , 0);
        // transform.GetChild(4).position += new Vector3(0, 0, size / 2 );
        // transform.GetChild(5).position -= new Vector3(0, 0, size / 2 );

    }
    void Start()
    {
        CompelxityOfPlanets = new int[transform.childCount];

        Complexity = 1000;
        MeshNumber = 0;
        OneGeneratePlanet();

        Complexity = 300;
        MeshNumber = 1;
        OneGeneratePlanet();
    }
    //for (int i = 0; i < 25; i++)
    // {
    // print("vertex " + i + " " + vertices[i].x + " " + vertices[i].y + " " + vertices[i].z);
    //// }

    /*       for (int i = 0; i < Complexity-1 ; i++)
           { 
               for (int d = 0  ; d  < Complexity   ; d++)
               {
                   if(d != Complexity)
                   {
                       triangles[(i * 4 + d) * 6] = i + d;
                       triangles[(i * 4 + d) * 6 + 1] = i + d + 1;
                       triangles[(i * 4 + d) * 6 + 2] = i + d + Complexity;

                       triangles[(i * 4 + d) * 6 + 3] = i + d + Complexity + 1;
                       triangles[(i * 4 + d) * 6 + 4] = i + d + Complexity;
                       triangles[(i * 4 + d) * 6 + 5] = i + d + 1;
                   }                       
               }
           }
    */
}
