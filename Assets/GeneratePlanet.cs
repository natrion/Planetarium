using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]


public class GeneratePlanet : MonoBehaviour
{
    public Material[] materials;
    public int textureSize;
    public Transform player;
    public GameObject OreFolder;
    private bool s;
    private float BiomNoisewas2;
    private float BiomNoisewas;
    private bool k;
    public Player playerScript;
    // public float[] atmosphereDensityOfPlanets;
    // public float atmosphereDensity;
    //public float DegenerativePerlinNoiseDestortion;
    //public float DegenerativePerlinNoiseIntensity;
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
    private float PerlinNoisewas2 = 1;
    public float whitness;
    public Color PlanetColor;
    public Color MountainColor;
    public float snowHight;
    public float PerlinoiseIntensityBiom;
    public float PerlinoiseDestortionBiom;
    public Color BiomColor;
    public float BiomColorIntensity;
    public float BiomGenerateHight;
    public GameObject PlanetCanvas;
    private GameObject plantcopy;
    private GameObject plantcopyRock;
    public float PerlinoiseIntensityRock;
    public float PerlinoiseDestortionRock;
    private GameObject plantcopyRock2;
    public float PerlinoiseIntensityRock2;
    public float PerlinoiseDestortionRock2;
    public bool generateRock;
    public Material sunMaterial;
    private Vector3[] PlanetsPositions;
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
                //float DegenerativePerlinNoise = Mathf.PerlinNoise((a2 - (a2 * 2)) * DegenerativePerlinNoiseDestortion, (aPerlin - (aPerlin * 2)) * DegenerativePerlinNoiseDestortion) * (DegenerativePerlinNoiseIntensity / 10);

                float PerlinNoyse =  PerlinNoyse1 * PerlinNoyse2* PerlinNoyse3  - Terrein ;
                 

                float maxPerlinNoyse = ((PerlinoiseIntensity / 10) * (PerlinoiseIntensity2 / 10) * (PerlinoiseIntensity3 / 10) ) ;
                

                if (i == Complexity | i == 0)
                {
                    PerlinNoyse = maxPerlinNoyse / 4;
                }else if(d == 0)
                {
                    PerlinNoisewas = PerlinNoyse;
                }else if(d> Complexity * 0.98f)
                {

                    if (k == false)
                    {
                        PerlinNoisewas2 = PerlinNoyse;
                    }
                    float a = (d - Complexity * 0.98f) / (Complexity - Complexity * 0.98f);
                    PerlinNoyse = PerlinNoisewas2 + (a *  (PerlinNoisewas - PerlinNoisewas2)) - Terrein ;
                    // PerlinNoyse += (PerlinNoisewas - PerlinNoyse) /  ( ( Complexity * 0.10f ) - ((float)d  - Complexity * 0.40f) )   ;
                    k = true;
                }
                else
                {
                    k = false;
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
                   // if (d > Complexity * 0.95f)
                   // {

                   //     if (s == false)
                    //    {
                   //         BiomNoisewas2 = PerlinNoyseBiom;
                   //     }
                     //   float a = (d - Complexity * 0.95f) / (Complexity - Complexity * 0.95f);
                    //    PerlinNoyseBiom = BiomNoisewas + (a * (BiomNoisewas - BiomNoisewas2)) ;
                        // PerlinNoyse += (PerlinNoisewas - PerlinNoyse) /  ( ( Complexity * 0.10f ) - ((float)d  - Complexity * 0.40f) )   ;
                    //    s = true;
                        
                   // }
                    //else
                   // {
                   //     s = false;
                   //     if (d == 0)
                   //     {
                   //         BiomNoisewas = PerlinNoyseBiom;
                   //     }
                   // }
                    colors[i * Complexity + d] = new Color(colors[i * Complexity + d].r + PerlinNoyseBiom * (BiomColor.r - colors[i * Complexity + d].r) * BiomColorIntensity, colors[i * Complexity + d].g + PerlinNoyseBiom * (BiomColor.g - colors[i * Complexity + d].g) * BiomColorIntensity, colors[i * Complexity + d].b + PerlinNoyseBiom * (BiomColor.b - colors[i * Complexity + d].b) * BiomColorIntensity, PlanetColor.a);

                }











                float PerlinNoyseRock = Mathf.PerlinNoise((a2 - (a2 * 2)) * PerlinoiseDestortionRock + 10000, (aPerlin - (aPerlin * 2)) * PerlinoiseDestortionRock + 10000) ;
                if (generateRock == true)
                {
                    float rockGenerate = Random.Range(0, PerlinoiseIntensityRock * (1-PerlinNoyseBiom)  );
                    if (rockGenerate > PerlinoiseIntensityRock * (1 - PerlinNoyseBiom) - 1)
                    {
                        GameObject plantCopyRock = Instantiate(plantcopyRock);
                        plantCopyRock.transform.position = new Vector3 (vertices[i * Complexity + d].x * plantcopy.transform.localScale.x + plantcopy.transform.position.x,
                                                                        vertices[i * Complexity + d].y * plantcopy.transform.localScale.y + plantcopy.transform.position.y,
                                                                        vertices[i * Complexity + d].z * plantcopy.transform.localScale.z + plantcopy.transform.position.z);
                        plantCopyRock.transform.parent = plantcopy.transform;
                        plantCopyRock.tag = "Rock";
                        plantCopyRock.transform.eulerAngles = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
                    }
                }

                float PerlinNoyseRock2 = Mathf.PerlinNoise((a2 - (a2 * 2)) * PerlinoiseDestortionRock2 + 10000, (aPerlin - (aPerlin * 2)) * PerlinoiseDestortionRock2 + 10000);
                if (generateRock == true)
                {
                    float rockGenerate = Random.Range(0, PerlinoiseIntensityRock2 * (1 - PerlinNoyseBiom));
                    if (rockGenerate > PerlinoiseIntensityRock2 * (1 - PerlinNoyseBiom) - 1)
                    {
                        GameObject plantCopyRock2 = Instantiate(plantcopyRock2);
                        plantCopyRock2.transform.position = new Vector3(vertices[i * Complexity + d].x * plantcopy.transform.localScale.x + plantcopy.transform.position.x,
                                                                        vertices[i * Complexity + d].y * plantcopy.transform.localScale.y + plantcopy.transform.position.y,
                                                                        vertices[i * Complexity + d].z * plantcopy.transform.localScale.z + plantcopy.transform.position.z);
                        plantCopyRock2.transform.parent = plantcopy.transform;
                        plantCopyRock2.tag = "Rock";
                        plantCopyRock2.transform.eulerAngles = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
                    }
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
        for (int k = 0; k < vertices.Length; k ++)
        {

            int U = ((k )  % Complexity) % (textureSize * 2);
            int Y = ((k ) / Complexity) % (textureSize * 2);
            if (U > textureSize)
            {
                U = textureSize - (((k) % Complexity) % (textureSize * 2) - textureSize);
            }
            if (Y > textureSize)
            {
                Y = textureSize - (((k) / Complexity) % (textureSize * 2) - textureSize);
            }

            uvs[k] = new Vector2((float)U / textureSize, (float)Y / textureSize); 
            
        }




        mesh = plantcopy.GetComponent<MeshFilter>().mesh;
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.vertices = vertices;
        mesh.colors = colors;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        MeshCollider MeshCollider = plantcopy.gameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;

        
        //atmosphereDensityOfPlanets[MeshNumber] = atmosphereDensity;


        // transform.GetChild(0).position += new Vector3(size / 2 , 0);
        // transform.GetChild(1).position -= new Vector3(size / 2 , 0);
        // transform.GetChild(2).position += new Vector3(0, size / 2 , 0);
        // transform.GetChild(3).position -= new Vector3(0, size / 2 , 0);
        // transform.GetChild(4).position += new Vector3(0, 0, size / 2 );
        // transform.GetChild(5).position -= new Vector3(0, 0, size / 2 );

    }
    void Start()
    {
        ///////////////////////////////////////////////Generate Actual Planet
        //plantcopy = Instantiate(PlanetCanvas);

        //OneGeneratePlanet();
        ///////////////////////////////////////////////////////////////////////
        
        int PlanetNumber = Random.Range(0, 10);
        
        PlanetsPositions = new Vector3[PlanetNumber];


        //atmosphereDensityOfPlanets = new float[transform.childCount];

        // atmosphereDensity = 0.02f;

        generateRock = false;

        plantcopy = Instantiate(PlanetCanvas);
        plantcopy.transform.parent = transform;

        Complexity = 150;
        
        PerlinoiseIntensity = 0;
        PerlinoiseIntensity2 = 0;
        PerlinoiseIntensity3 = 0;

        PlanetColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        BiomColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        MountainColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

        plantcopy.transform.localScale = new Vector3(1, 1, 1) * Random.Range(7, 20);

        TerrainDestortion = Random.Range(20, 40);
        TerrainIntensity = Random.Range(0, 0.2f);
        snowHight = 0.1f;

        Light StarLight = plantcopy.AddComponent(typeof(Light)) as Light;
        StarLight.range =   plantcopy.transform.localScale.x * 4800;
        StarLight.intensity =   plantcopy.transform.localScale.x * 20000000;
        StarLight.color = PlanetColor;
        StarLight.shadows = LightShadows.Hard;

        plantcopy.GetComponent<MeshRenderer>().material = sunMaterial ;

        plantcopy.transform.eulerAngles = new Vector3(0, 90, 0);
        

        plantcopy.transform.eulerAngles = new Vector3(90, 0, 0);
        plantcopy.GetComponent<ThingData>().PlanetComplexity = Complexity;

        OneGeneratePlanet();

        
        for (int i = 0; i != PlanetNumber; i++)
        {
            
            GenerateParametersForPlanet(1200 , true);
            PlanetsPositions[i] = plantcopy.transform.position;
            if (i == 0)
            {
                player.position = plantcopy.transform.position + new Vector3(0, plantcopy.GetComponent<ThingData>().PlanetComplexity / 3, 0);
            }
            if (Complexity > 250)
            {
                float MoonNumber = Random.Range(0, 5 * ((float)Complexity / 1200 * 2 ));

                GameObject Bigplantcopy = plantcopy;
                for (int g = 0; g < MoonNumber; g++)
                {
                    GenerateParametersForPlanet((float)Complexity / 2 , false);
                    plantcopy.transform.parent = Bigplantcopy.transform;
                    float a;
                    if (Random.Range(-1, 2) == 1) { a = 1; } else { a = -1; }
                    float b;
                    if (Random.Range(-1, 2) == 1) { b = 1; } else { b = -1; }

                    plantcopy.transform.position = new Vector3(Random.Range(500, 1000) * a + Bigplantcopy.transform.position.x, 0, Random.Range(500, 1000) * b + Bigplantcopy.transform.position.z);

                    
                }
            }
            
        }

        
        //plantcopyRock = null;
        //plantcopyRock2 = null;

        /////////////////////////////////////////////////////////GENERATING PLANET

      //  MeshNumber = 1;

       // plantcopy = Instantiate(PlanetCanvas);
       // plantcopy.transform.parent = transform;

      //  Complexity = 200;

       // plantcopy.transform.position = new Vector3(0, 1000, 0);
       // OneGeneratePlanet();



        //atmosphereDensity = 0.01f;
        //Complexity = 300;
        //MeshNumber = 1;
        //OneGeneratePlanet();
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
    void GenerateParametersForPlanet(float MaxComplexity , bool DoPosition)
    {
        snowHight = 0.0001f;
        //////////////////////////////////////////////////////////GENERATING ROCK FOR PLANET
        generateRock = false;


        plantcopy = Instantiate(PlanetCanvas);
        plantcopyRock = plantcopy;
        plantcopyRock.SetActive(false);
        plantcopy.transform.parent = transform;

        Complexity = Random.Range(40, 80);

        PerlinoiseIntensity = Random.Range(15f, 40f);
        PerlinoiseIntensity2 = Random.Range(5f, 20f);
        PerlinoiseIntensity3 = Random.Range(5f, 10f);
        PerlinoiseDestortion = Random.Range(0.5f, 2f);
        PerlinoiseDestortion2 = Random.Range(1f, 3f);
        PerlinoiseDestortion3 = Random.Range(2f, 5f);

        PlanetColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        BiomColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        PerlinoiseIntensityBiom = Random.Range(0f, 10f);
        PerlinoiseDestortionBiom = Random.Range(0f, 10f);
        MountainColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

        plantcopy.transform.localScale = new Vector3(1, 1, 1) * Random.Range(0.05f, 0.2f);
        plantcopy.GetComponent<ThingData>().Health = Complexity;
        plantcopy.GetComponent<ThingData>().MaxHealth = Complexity;

        plantcopy.GetComponent<ThingData>().StartRockSize = plantcopy.transform.localScale.x * 2.8f;

        plantcopy.GetComponent<ThingData>().Ore = OreFolder.transform.GetChild(Random.Range(0, OreFolder.transform.childCount)).gameObject;

        plantcopy.GetComponent<ThingData>().MaxHealthToNewOre = Random.Range(2f, 5f); ;
        plantcopy.GetComponent<ThingData>().HealthToNewOre = plantcopy.GetComponent<ThingData>().MaxHealthToNewOre;
        OneGeneratePlanet();

        

        Destroy(plantcopyRock);


        //////////////////////////////////////////////////////////GENERATING ROCK FOR PLANET



        plantcopy = Instantiate(PlanetCanvas);
        plantcopyRock2 = plantcopy;
        plantcopyRock2.SetActive(false);
        plantcopy.transform.parent = transform;

        Complexity = Random.Range(20, 60);

        PerlinoiseIntensity = Random.Range(0f, 40f);
        PerlinoiseIntensity2 = Random.Range(0f, 20f);
        PerlinoiseIntensity3 = Random.Range(0f, 10f);
        PerlinoiseDestortion = Random.Range(0f, 2f);
        PerlinoiseDestortion2 = Random.Range(0f, 3f);
        PerlinoiseDestortion3 = Random.Range(0f, 5f);

        PlanetColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        BiomColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        PerlinoiseIntensityBiom = Random.Range(0f, 10f);
        PerlinoiseDestortionBiom = Random.Range(0f, 10f);
        MountainColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

        plantcopy.transform.localScale = new Vector3(1, 1, 1) * Random.Range(0.05f, 0.2f);
        plantcopy.GetComponent<ThingData>().Health =Complexity;
        plantcopy.GetComponent<ThingData>().MaxHealth = Complexity;

        plantcopy.GetComponent<ThingData>().StartRockSize = plantcopy.transform.localScale.x *2.8f ;

        plantcopy.GetComponent<ThingData>().Ore = OreFolder.transform.GetChild(Random.Range(0, OreFolder.transform.childCount)).gameObject;

        plantcopy.GetComponent<ThingData>().MaxHealthToNewOre = Random.Range(2f, 5f); 
        plantcopy.GetComponent<ThingData>().HealthToNewOre = plantcopy.GetComponent<ThingData>().MaxHealthToNewOre;


        OneGeneratePlanet();

        Destroy(plantcopyRock2);
        //////////////////////////////////////////////////////////GENERATING PLANET
        generateRock = true;
        

        plantcopy = Instantiate(PlanetCanvas);
        plantcopy.transform.parent = transform;
        if (DoPosition == true)
        {
            bool GoodDistance = false;
            while (GoodDistance == false)
            {
                float a;
                if (Random.Range(-1, 2) == 1) { a = 1; } else { a = -1; }
                float b;
                if (Random.Range(-1, 2) == 1) { b = 1; } else { b = -1; }

                plantcopy.transform.position = new Vector3(Random.Range(2000, 15000) * a, 0, Random.Range(2000, 15000) * b);
                GoodDistance = true;

                for (int d = 0; d < PlanetsPositions.Length; d++)
                {
                    float Distance = Mathf.Abs(PlanetsPositions[d].x - plantcopy.transform.position.x) + Mathf.Abs(PlanetsPositions[d].y - plantcopy.transform.position.y) + Mathf.Abs(PlanetsPositions[d].z - plantcopy.transform.position.z);
                    if (Distance < 7000)
                    {
                        GoodDistance = false;
                    }
                }

            }
        }
        

        Complexity = Random.Range(80, (int)MaxComplexity);

        PerlinoiseIntensity = Random.Range(3f, 13f);
        PerlinoiseIntensity2 = Random.Range(7f, 13f);
        PerlinoiseIntensity3 = Random.Range(2f / ((float)Complexity / 1100), 8f / ((float)Complexity / 1100));

        PerlinoiseDestortion = Random.Range(0.05f, 0.9f);
        PerlinoiseDestortion2 = Random.Range(0.5f, 3.75f);
        PerlinoiseDestortion3 = Random.Range(0.75f * ((float)Complexity / 1100), 7.5f * ((float)Complexity / 1100));

        TerrainDestortion = Random.Range(70f * ((float)Complexity / 1100), 100f * ((float)Complexity / 1100));
        TerrainIntensity = Random.Range(0.01f / ((float)Complexity / 1100), 0.025f / ((float)Complexity / 1100));

        //DegenerativePerlinNoiseDestortion = Random.Range(14f * ((float)Complexity / 1100), 20f * ((float)Complexity / 1100));
        //DegenerativePerlinNoiseIntensity = Random.Range(1f / ((float)Complexity / 1100), 1f / ((float)Complexity / 1100));

        whitness = Random.Range(1.5f, 4.5f);

        PlanetColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        BiomColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        PerlinoiseIntensityBiom = Random.Range(1f, 2f);
        PerlinoiseDestortionBiom = Random.Range(1f, 3f);
        MountainColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

        PerlinoiseDestortionRock = Random.Range(0f, 1f);
        PerlinoiseDestortionRock2 = Random.Range(0.5f, 2f);
        PerlinoiseIntensityRock = Random.Range(2800, 6000);
        PerlinoiseIntensityRock2 = Random.Range(800, 1600);

        plantcopy.GetComponent<MeshRenderer>().material = materials[Random.Range(0, 6)];

        //Complexity = 1050;

        OneGeneratePlanet();

        plantcopy.transform.eulerAngles = new Vector3(90, 0, 0);
        plantcopy.GetComponent<ThingData>().PlanetComplexity = Complexity;

        plantcopy.GetComponent<ThingData>().Intensity = PerlinoiseIntensity * PerlinoiseIntensity2 * PerlinoiseIntensity3 + TerrainIntensity;

        plantcopy.GetComponent<ThingData>().PlanetOre1 = OreFolder.transform.GetChild(Random.Range(0, OreFolder.transform.childCount)).gameObject;
        plantcopy.GetComponent<ThingData>().PlanetOre2 = OreFolder.transform.GetChild(Random.Range(0, OreFolder.transform.childCount)).gameObject;

        plantcopy.GetComponent<ThingData>().PlanetMaxHealthToNewOre1 = Random.Range(15f, 30f);
        plantcopy.GetComponent<ThingData>().PlanetMaxHealthToNewOre2 = Random.Range(15f, 30f);
        plantcopy.GetComponent<ThingData>().HealthToNewOre1 = plantcopy.GetComponent<ThingData>().PlanetMaxHealthToNewOre1;
        plantcopy.GetComponent<ThingData>().HealthToNewOre2 = plantcopy.GetComponent<ThingData>().PlanetMaxHealthToNewOre2;

    }

    
}
