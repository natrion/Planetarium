using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]


public class GeneratePlanet : MonoBehaviour
{
    //Seting Variables
    public GameObject PlanetOrbit; 
    public GameObject RockFolder;
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

    public float PlanetShapeDestortion;
    public float PlanetShapeIntensity;

    private Vector2[] uvs;
    //private float PerlinNoisewas = 1;
    //private float PerlinNoisewas2 = 1;
    public float whitness;
    public Color PlanetColor;
    public Color MountainColor;
    public float snowHight;

    public float PerlinoiseIntensityBiom;
    public float PerlinoiseDestortionBiom;
    public float PerlinoiseIntensityBiom2;
    public float PerlinoiseDestortionBiom2;

    public float BiomGenerateHight;

    public Color BiomColor;
    public Color BiomColor2;

    public GameObject PlanetCanvas;
    public GameObject starCanvas;

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

    public static float Noise3D(float x, float y, float z, float frequency, float amplitude, float persistence, int octave, int seed)
	{
		float noise = 0.0f;

		for (int i = 0; i < octave; ++i)
		{
			// Get all permutations of noise for each individual axis
			float noiseXY = Mathf.PerlinNoise(x * frequency + seed, y * frequency + seed) * amplitude;
			float noiseXZ = Mathf.PerlinNoise(x * frequency + seed, z * frequency + seed) * amplitude;
			float noiseYZ = Mathf.PerlinNoise(y * frequency + seed, z * frequency + seed) * amplitude;

			// Reverse of the permutations of noise for each individual axis
			float noiseYX = Mathf.PerlinNoise(y * frequency + seed, x * frequency + seed) * amplitude;
			float noiseZX = Mathf.PerlinNoise(z * frequency + seed, x * frequency + seed) * amplitude;
			float noiseZY = Mathf.PerlinNoise(z * frequency + seed, y * frequency + seed) * amplitude;

			// Use the average of the noise functions
			noise += (noiseXY + noiseXZ + noiseYZ + noiseYX + noiseZX + noiseZY) / 6.0f;

			amplitude *= persistence;
			frequency *= 2.0f;
		}

		// Use the average of all octaves
		return noise / octave;
	}

    void OneGeneratePlanet()
    {
        
        Color colorDiference = new Color(MountainColor.r -PlanetColor.r  ,  MountainColor.g - PlanetColor.g ,   MountainColor.b - PlanetColor.b, 1);
        vertices = new Vector3[(Complexity * Complexity + 1 )* 2];
        colors = new Color[vertices.Length];
        uvs = new Vector2[vertices.Length];

        triangles = new int[(Complexity * Complexity * 6 )* 2];

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //whitness = whitness / 10;
        float r1 = (0.3f * Complexity);////seting the radius of planet it never changes
        float z = 0f;////seting z how high is the script
        a2 = 0;////seting a2 z cordinat for perlin noise
        for (int i = 0; i < Complexity+ 1; i++)
        {
            if (2 * r1 * z - z * z <0)
            {
                print("s");
            }
            float r = Mathf.Sqrt(2 * r1 * z - z * z);////seting radius of one of the crircles on the sphear 
            
            for (int d = 0; d < Complexity; d++)
            {

                a+= (2*Mathf.PI / Complexity);////
                if(i == Complexity)
                {
                    r = 0;
                }
                float x = r *Mathf.Sin(a);/////seting x direction
                float y = r *Mathf.Cos(a);/////seting z direction
                //PerlinoiseIntensity = PerlinoiseIntensity /(z - r1);

                

                //float ZbetweenOne = (float)i / ((float)Complexity);
                //float ZCalculated = Mathf.Abs(Mathf.Abs((ZbetweenOne * 2) - 1) - 1);
                //print(ZCalculated + "= ZCalculated");


                //float northeastnumber = 1 - Mathf.Abs( (float)i / (float)Complexity - 0.5f);////detect how far is this round to pole
                //float unroal = Mathf.Abs(( (float)i / (float)Complexity) - 0.5f);

                //float aPerlin = a  ;

                //print(northeastnumber);

                float PerlinNoyse1 = perlinNoise.get3DPerlinNoise(new Vector3(x / 75 , y / 75  , (z - r1) / 75 ), PerlinoiseDestortion/2) * (PerlinoiseIntensity /5);
                float PerlinNoyse2 = perlinNoise.get3DPerlinNoise(new Vector3(x / 75, y / 75, (z - r1) / 75), PerlinoiseDestortion2 /2  ) * (PerlinoiseIntensity2 /5);
                float PerlinNoyse3Original  = perlinNoise.get3DPerlinNoise(new Vector3(x / 75, y / 75, (z - r1) / 75), PerlinoiseDestortion3 / 2);
                float PlanetShape = perlinNoise.get3DPerlinNoise(new Vector3(x / 75, y / 75, (z - r1) / 75), PlanetShapeDestortion / 2) * (PlanetShapeIntensity / 5);

                float PerlinNoyse3 = 1 - Mathf.Abs(PerlinNoyse3Original);

                float Terrein = perlinNoise.get3DPerlinNoise(new Vector3(x / 75, y / 75, (z - r1) / 75), TerrainDestortion * 50) * (TerrainIntensity / 5);

                //float PerlinNoyse1 = Noise3D(x / 100, y / 100, (z - r1) / 100, PerlinoiseDestortion, PerlinoiseIntensity / 5, 1, 1,1000);
                //float PerlinNoyse2 = Noise3D(x / 100, y / 100, (z - r1) / 100, PerlinoiseDestortion2, PerlinoiseIntensity2 / 5, 1, 1, 1000);
                //float PerlinNoyse3 = Noise3D(x / 100, y / 100, (z - r1) / 100, PerlinoiseDestortion3, PerlinoiseIntensity3/5 , 1, 1, 1000);
                //float Terrein = Noise3D(x / 100, y / 100, z / 100, TerrainDestortion, TerrainIntensity / 5, 1, 1, 0);

                //float PerlinNoyse1 = Mathf.PerlinNoise((a2 ) * PerlinoiseDestortion , (aPerlin) * PerlinoiseDestortion  ) * (PerlinoiseIntensity / 10);
                //float PerlinNoyse2 = Mathf.PerlinNoise((a2) * PerlinoiseDestortion2 , (aPerlin) * PerlinoiseDestortion2) * (PerlinoiseIntensity2 / 10);
                //float PerlinNoyse3 = Mathf.PerlinNoise((a2) * PerlinoiseDestortion3 , (aPerlin) * PerlinoiseDestortion3) * (PerlinoiseIntensity3 / 10);
                //float Terrein = Mathf.PerlinNoise((a2) * TerrainDestortion, (aPerlin) * TerrainDestortion ) * (TerrainIntensity / 10);

                //float DegenerativePerlinNoise = Mathf.PerlinNoise((a2 - (a2 * 2)) * DegenerativePerlinNoiseDestortion, (aPerlin - (aPerlin * 2)) * DegenerativePerlinNoiseDestortion) * (DegenerativePerlinNoiseIntensity / 10);

                float PerlinNoyse =  PlanetShape+PerlinNoyse1 * PerlinNoyse2 * PerlinNoyse3 - Terrein ;



                float maxPerlinNoyse = ((PerlinoiseIntensity ) * (PerlinoiseIntensity2 ) + Terrein) ;
                

                //if (i == Complexity | i == 0)
                //{
                //    PerlinNoyse = maxPerlinNoyse / 4;
                //if(d == 0)
               // {
               //     PerlinNoisewas = PerlinNoyse;
               // }else if(d> Complexity * 0.98f)
               // {

               //     if (k == false)
               //     {
               //         PerlinNoisewas2 = PerlinNoyse;
               //     }
               //     float a = (d - Complexity * 0.98f) / (Complexity - Complexity * 0.98f);
              //      PerlinNoyse = PerlinNoisewas2 + (a *  (PerlinNoisewas - PerlinNoisewas2)) - Terrein ;
              //      // PerlinNoyse += (PerlinNoisewas - PerlinNoyse) /  ( ( Complexity * 0.10f ) - ((float)d  - Complexity * 0.40f) )   ;
              //      k = true;
              //  }
              //  else
              //  {
              //      k = false;
              //  }


                //float z = r * Mathf.Sin(a2);
                //float y2 = r * Mathf.Cos(a2);
                vertices[i * Complexity + d ] = new Vector3(x + x *PerlinNoyse, y +  y *PerlinNoyse, (z - r1) +  (z- r1 )* PerlinNoyse  );





                /////////////////////////////////////////////////////////////////generating mountain colors
                //if (snowHight > PerlinNoyse)
                //{
                 //   colors[i * Complexity + d] = new Color(PlanetColor.r  , PlanetColor.g , PlanetColor.b , PlanetColor.a);
               // }
               // else
               // {
                //    colors[i * Complexity + d] = new Color(PlanetColor.r + (PerlinNoyse / maxPerlinNoyse) * colorDiference.r*whitness, PlanetColor.g + (PerlinNoyse / maxPerlinNoyse) * colorDiference.g*whitness, PlanetColor.b + (PerlinNoyse / maxPerlinNoyse) * colorDiference.b * whitness, PlanetColor.a);
                    //colors[i * Complexity + d] = new Color(PlanetColor.r * (PerlinNoyse / whitness * colorDiference.r), PlanetColor.g * (PerlinNoyse / whitness * colorDiference.g), PlanetColor.b * (PerlinNoyse / whitness * colorDiference.b), PlanetColor.a);
                    //colors[i * Complexity + d] = new Color(PlanetColor.r * (PerlinNoyse / whitness), PlanetColor.g * (PerlinNoyse / whitness), PlanetColor.b * (PerlinNoyse / whitness), PlanetColor.a);
               // }
                float PerlinNoyseBiom1 = perlinNoise.get3DPerlinNoise(new Vector3(x / 75, y / 75, (z - r1) / 75), PerlinoiseDestortionBiom / 2) * (PerlinoiseIntensityBiom / 7);
                float PerlinNoyseBiom2 = perlinNoise.get3DPerlinNoise(new Vector3(x / 75, y / 75, (z - r1) / 75), PerlinoiseDestortionBiom2 / 2) * (PerlinoiseIntensityBiom2 / 7);

                Color finalBiomColor = PerlinNoyseBiom1 * (BiomColor) + PerlinNoyseBiom2 * (BiomColor2);



                /////////////////////////////////////////////////////////////////generating biom colors
                // if ((PerlinNoyse / maxPerlinNoyse) < BiomGenerateHight)
                // {
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
                // }high

                Color highColor = MountainColor * whitness * ((PerlinNoyse- PlanetShape) / maxPerlinNoyse);
                

                if (PerlinoiseIntensity == 0) { colors[i * Complexity + d] = PlanetColor + finalBiomColor; }//sun color
                else { colors[i * Complexity + d] = PlanetColor + finalBiomColor + highColor; }//planet color 

                // }











                float PerlinNoyseRock = Mathf.PerlinNoise((a2 - (a2 * 2)) * PerlinoiseDestortionRock + 10000, (a - (a * 2)) * PerlinoiseDestortionRock + 10000) ;
                if (generateRock == true)
                {
                    float rockGenerate = Random.Range(PerlinoiseIntensityRock, PerlinNoyseRock);
                    if (rockGenerate > 0.8 )
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

                float PerlinNoyseRock2 = Mathf.PerlinNoise((a2 - (a2 * 2)) * PerlinoiseDestortionRock2 + 10000, (a - (a * 2)) * PerlinoiseDestortionRock2 + 10000);
                if (generateRock == true)
                {
                    float rockGenerate = Random.Range(PerlinoiseIntensityRock2, PerlinNoyseRock2);
                    if (rockGenerate > 0.8)
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
                if (i * Complexity + d == 15300)
                {
                    print("Bug");
                }
            }
            //PerlinNoisewas = 1;
            if (r !=0 )
            {
                a2 += ((Mathf.PI / Complexity) / r) * r1;
            }
            else
            {
                a2 += Mathf.PI / Complexity;
            }
            
            a = 0;

            // if ( i < 2)
            // {
            //     z += (2 * r1 / Complexity /2);
            // } else if (i > Complexity -2)
            // {
            //    z += (2 * r1 / Complexity / 2);
            // } else
            //S {

            //}
            

            float IbetweenOne = (float)i / ((float)Complexity ) ;
            float ICalculated = Mathf.Abs(Mathf.Abs((IbetweenOne*2)-1)-1);
            z += ICalculated* 1.2f;

            //z += (2 * r1 / Complexity);

            //z = (Mathf.Sin(Mathf.PI / Complexity * i - Mathf.PI / 2) + 1) * (2 * r1 / Complexity) * i;


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
               
        ///////////////////////////////////////////////////////////////////////

        int PlanetNumber = Random.Range(1, 10);////generating planet number

        PlanetsPositions = new Vector3[PlanetNumber];//Seting Variables for positions of planets

        ///////////////////////////////////////////////////////////////////////////////////////generating star
        generateRock = false;//rock will not generate

        plantcopy = Instantiate(starCanvas);////seting parent and craeting star
        plantcopy.transform.parent = transform;

        Complexity = 150;

        PerlinoiseIntensity = 0;//making no hills on the star
        PerlinoiseIntensity2 = 0;
        PerlinoiseIntensity3 = 0;

        PlanetColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));//generating star color
        BiomColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        MountainColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

        plantcopy.transform.localScale = new Vector3(1, 1, 1) * Random.Range(7, 20);////generating star size

        TerrainDestortion = Random.Range(20, 40);////hill size on the star
        TerrainIntensity = Random.Range(0, 0.2f);////hill height on the star
        snowHight = 0.1f;

        Light StarLight = plantcopy.transform.GetChild(0).GetComponent<Light>(); ////generating light on the star
        StarLight.intensity =   plantcopy.transform.localScale.x  /2f;
        StarLight.color = PlanetColor;

        plantcopy.transform.eulerAngles = new Vector3(0, 90, 0);//seting star rotation
       
        plantcopy.transform.eulerAngles = new Vector3(90, 0, 0);
        plantcopy.GetComponent<ThingData>().PlanetComplexity = Complexity;//setting mass of the star

        OneGeneratePlanet();////generating star

        
        for (int i = 0; i != PlanetNumber; i++)////making X number of planets            ( X = PlanetNumber)
        {
            
            GenerateParametersForPlanet(1200 , true);///generating parameters for planet
            PlanetsPositions[i] = plantcopy.transform.position;//// adding position to ather planet positions
            if (i == 0)/////planet can not have moons if it is too small
            {
                player.position = plantcopy.transform.position + new Vector3(0, plantcopy.GetComponent<ThingData>().PlanetComplexity / 3, 0);
            }
            if (Complexity > 250)
            {
                float MoonNumber = Random.Range(0, 5 * ((float)Complexity / 1200 * 2 ));
                
                GameObject Bigplantcopy = plantcopy;////seting orbiting planet parameters
                for (int g = 0; g < MoonNumber; g++)////making X number of moons 
                {                               // max complexity       //anabling rocks to spawn
                    GenerateParametersForPlanet((float)Complexity / 2 , false);

                    GameObject MoonOrbit = Instantiate(PlanetOrbit);
                    MoonOrbit.transform.position = Bigplantcopy.transform.position;
                    MoonOrbit.transform.parent = Bigplantcopy.transform;

                    plantcopy.transform.parent = MoonOrbit.transform;
                    float a;////calculating moon position (and making it not spawn is planet)
                    if (Random.Range(-1, 2) == 1) { a = 1; } else { a = -1; }
                    float b;
                    if (Random.Range(-1, 2) == 1) { b = 1; } else { b = -1; }

                    plantcopy.transform.localPosition = new Vector3(Random.Range(500, 1000) * a , 0, Random.Range(500, 1000) * b );

                    
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
        //generateRock = false;


        //plantcopy = Instantiate(PlanetCanvas);
        //plantcopyRock = plantcopy;
        //plantcopyRock.SetActive(false);
        //plantcopy.transform.parent = transform;

        //Complexity = Random.Range(40, 80);

        //PerlinoiseIntensity = Random.Range(15f, 40f);
        //PerlinoiseIntensity2 = Random.Range(5f, 20f);
        //PerlinoiseIntensity3 = Random.Range(5f, 10f);
        //PerlinoiseDestortion = Random.Range(0.5f, 2f);
        //PerlinoiseDestortion2 = Random.Range(1f, 3f);
        //PerlinoiseDestortion3 = Random.Range(2f, 5f);

        //PlanetColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        //BiomColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        //PerlinoiseIntensityBiom = Random.Range(0f, 10f);
        //PerlinoiseDestortionBiom = Random.Range(0f, 10f);
        //MountainColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

        //plantcopy.transform.localScale = new Vector3(1, 1, 1) * Random.Range(0.05f, 0.2f);
        for (int i = 0; i < 2; i++)/////making 2 rocks
        {
            
            if (Random.Range(1,50) == 40)
            {
                plantcopy = Instantiate(RockFolder.transform.GetChild(Random.Range(0, RockFolder.transform.childCount)).gameObject);////coping randomli selected rock
            }
            else
            {
                generateRock = false;
                textureSize = 70;
                plantcopy = Instantiate(PlanetCanvas);
                Complexity = Random.Range(50, 80);

                PlanetShapeIntensity = Random.Range(0, 0);
                PerlinoiseIntensity = Random.Range(4f, 8f);
                PerlinoiseIntensity2 = Random.Range(10f, 14f);
                PerlinoiseIntensity3 = Random.Range(16f, 24f);

                PlanetShapeDestortion = Random.Range(2f, 3f);
                PerlinoiseDestortion = Random.Range(4f, 8f);
                PerlinoiseDestortion2 = Random.Range(6f, 8f);
                PerlinoiseDestortion3 = Random.Range(7f, 10f);

                TerrainDestortion = Random.Range(15, 17);
                TerrainIntensity = Random.Range(0.2f, 0.8f);

                //DegenerativePerlinNoiseDestortion = Random.Range(14f * ((float)Complexity / 1100), 20f * ((float)Complexity / 1100));
                //DegenerativePerlinNoiseIntensity = Random.Range(1f / ((float)Complexity / 1100), 1f / ((float)Complexity / 1100));
                PlanetColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

                whitness = Random.Range(140, 200);
                MountainColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

                BiomColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                PerlinoiseIntensityBiom = Random.Range(5.4f, 6.6f);
                PerlinoiseDestortionBiom = Random.Range(1f, 2f);

                BiomColor2 = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                PerlinoiseIntensityBiom2 = Random.Range(1.2f, 1.6f);
                PerlinoiseDestortionBiom2 = Random.Range(6f, 10f);

                OneGeneratePlanet();
            }
            
            if (i==0)////efecting rock1 on tur 1     and rock2 on turn 2
            {
                plantcopyRock = plantcopy;/////making copy of the rock
                plantcopyRock.SetActive(false);//seting it not active 
            }
            else
            {
                plantcopyRock2 = plantcopy;////the same but for rock2
                plantcopyRock2.SetActive(false);
            }
            

            plantcopy.transform.localScale = new Vector3(1, 1, 1) * Random.Range(0.02f, 0.05f);////randomly scaling rock

            plantcopy.GetComponent<ThingData>().Health = plantcopy.transform.localScale.x * 10 * (float)Complexity;////seting rock health 
            plantcopy.GetComponent<ThingData>().MaxHealth = plantcopy.transform.localScale.x * 10 * (float)Complexity;////seting rock  max health so the script can scale it down when mining corectly

            plantcopy.GetComponent<ThingData>().StartRockSize = plantcopy.transform.localScale.x * 2.8f;////seting rock scale so the script can scale it down when mining corectly

            plantcopy.GetComponent<ThingData>().Ore = OreFolder.transform.GetChild(Random.Range(0, OreFolder.transform.childCount)).gameObject;////seting rocks ore

            plantcopy.GetComponent<ThingData>().MaxHealthToNewOre = Random.Range(2f, 6f);// seting MaxHealthToNewOre
            plantcopy.GetComponent<ThingData>().HealthToNewOre = plantcopy.GetComponent<ThingData>().MaxHealthToNewOre;// seting HealthToNewOre

            plantcopy.GetComponent<ThingData>().ThingsExploreData = Random.Range(0, 1000000000000000.000f);
            plantcopy.GetComponent<ThingData>().thingExplordataType = "Planet Data";
            plantcopy.GetComponent<ThingData>().rotate = false;
            plantcopy.GetComponent<ThingData>().DataAmount = Random.Range(3, 10);

            
            if (i == 0)////efecting rock1 on tur 1     and rock2 on turn 2
            {
                Destroy(plantcopyRock);////Destroing rock1 so there is not any not wanted rock in the scene
            }
            else
            {
                Destroy(plantcopyRock2);////Destroing rock2 so there is not any not wanted rock in the scene
            }
        }
        


        //////////////////////////////////////////////////////////GENERATING ROCK FOR PLANET



        //plantcopy = Instantiate(PlanetCanvas);
        //plantcopyRock2 = plantcopy;
        ///plantcopyRock2.SetActive(false);
        //plantcopy.transform.parent = transform;

        //Complexity = Random.Range(20, 60);

        //PerlinoiseIntensity = Random.Range(0f, 40f);
        //PerlinoiseIntensity2 = Random.Range(0f, 20f);
        //PerlinoiseIntensity3 = Random.Range(0f, 10f);
        //PerlinoiseDestortion = Random.Range(0f, 2f);
        //PerlinoiseDestortion2 = Random.Range(0f, 3f);
        //PerlinoiseDestortion3 = Random.Range(0f, 5f);

        //PlanetColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        //BiomColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        //PerlinoiseIntensityBiom = Random.Range(0f, 10f);
        //PerlinoiseDestortionBiom = Random.Range(0f, 10f);
        //MountainColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

        //plantcopy.transform.localScale = new Vector3(1, 1, 1) * Random.Range(0.05f, 0.2f);
        //plantcopy.GetComponent<ThingData>().Health =Complexity;
        //plantcopy.GetComponent<ThingData>().MaxHealth = Complexity;

        //plantcopy.GetComponent<ThingData>().StartRockSize = plantcopy.transform.localScale.x *2.8f ;

        //plantcopy.GetComponent<ThingData>().Ore = OreFolder.transform.GetChild(Random.Range(0, OreFolder.transform.childCount)).gameObject;

        //plantcopy.GetComponent<ThingData>().MaxHealthToNewOre = Random.Range(2f, 5f); 
        //plantcopy.GetComponent<ThingData>().HealthToNewOre = plantcopy.GetComponent<ThingData>().MaxHealthToNewOre;


        //OneGeneratePlanet();

        //Destroy(plantcopyRock2);
        //////////////////////////////////////////////////////////GENERATING PLANET
        generateRock = true;
        textureSize = 10;

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

                plantcopy.transform.position = new Vector3(Random.Range(2000, 10000) * a, 0, Random.Range(2000, 10000) * b);
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

        PlanetShapeIntensity = Random.Range(0f, 3f) * Mathf.Clamp((1200f / (float)Complexity) / 4f, 1, 10f);
        PerlinoiseIntensity = Random.Range(1f, 2f);
        PerlinoiseIntensity2 = Random.Range(5f, 7f)  ;
        PerlinoiseIntensity3 = Random.Range(8f, 12f) ;

        PlanetShapeDestortion = Random.Range(0.7f, 1f) * Mathf.Clamp(( 1200f / (float)Complexity) / 4f, 1, 10f);
        PerlinoiseDestortion = Random.Range(0.7f, 2f) * Mathf.Clamp((1200f / (float)Complexity) / 4f, 1, 10f); 
        PerlinoiseDestortion2 = Random.Range(2.5f, 3.5f) * Mathf.Clamp((1200f / (float)Complexity) / 4f, 1, 10f); 
        PerlinoiseDestortion3 = Random.Range(7f, 10f) * Mathf.Clamp((1200f / (float)Complexity) / 4f, 1, 10f); 

        TerrainDestortion = Random.Range(60f , 80f );
        TerrainIntensity = Random.Range(0.004f , 0.006f );

        //DegenerativePerlinNoiseDestortion = Random.Range(14f * ((float)Complexity / 1100), 20f * ((float)Complexity / 1100));
        //DegenerativePerlinNoiseIntensity = Random.Range(1f / ((float)Complexity / 1100), 1f / ((float)Complexity / 1100));
        PlanetColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

        whitness = Random.Range(70, 100);
        MountainColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

        BiomColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        PerlinoiseIntensityBiom = Random.Range(5.4f, 6.6f);
        PerlinoiseDestortionBiom = Random.Range(1f, 2f);

        BiomColor2 = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        PerlinoiseIntensityBiom2= Random.Range(1.2f, 1.6f);
        PerlinoiseDestortionBiom2 = Random.Range(6f, 10f);


        PerlinoiseDestortionRock = Random.Range(3f, 6f);
        PerlinoiseDestortionRock2 = Random.Range(3f, 6f);
        PerlinoiseIntensityRock = Random.Range(-5, -10) ;
        PerlinoiseIntensityRock2 = Random.Range(-5, -10) * Mathf.Clamp(( (float)Complexity / 1200) *2 , 0.1f, 1f);

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
        plantcopy.GetComponent<ThingData>().ThingsExploreData = Random.Range(0, 1000000000000000.000f);
        plantcopy.GetComponent<ThingData>().thingExplordataType = "Planet Data";
        plantcopy.GetComponent<ThingData>().DataAmount = Random.Range(3, 10);
    }
    void Updatee()
    {
        
        if (!plantcopy )
        {
            plantcopy = Instantiate(PlanetCanvas);
        }

        OneGeneratePlanet();
    }

    
}
