using UnityEngine;
using System.Collections;

//Flat Mesh Generator API is here ↓
using VacuumShaders.FlatMeshGenerator;


[AddComponentMenu("VacuumShaders/Flat Mesh Generator/Example/Runtime Flat Terrain")]
public class Runtime_FlatTerrain : MonoBehaviour 
{
    //////////////////////////////////////////////////////////////////////////////
    //                                                                          // 
    //Variables                                                                 //                
    //                                                                          //               
    //////////////////////////////////////////////////////////////////////////////

    //
    public Terrain targetTerrain;

    
    //Terrain To Mesh options
    public FlatTerrainOptions flatOptions;


    //This material will be used on final mesh
    public Material vertexColorMaterial;

    //////////////////////////////////////////////////////////////////////////////
    //                                                                          // 
    //Unity Functions                                                           //                
    //                                                                          //               
    //////////////////////////////////////////////////////////////////////////////
	void Start () 
    {
        if (targetTerrain == null)
            return;


      
        //Will contain bake results 
        //Need - array - as FlatMeshGenerator returns mesh array depending on chunk count
        Mesh[] newMesh = null;

        //Will contain baking reports, will help if something goes wrong
        FlatMeshGenerator.CONVERTION_INFO[] convertionInfo;

        //Same as above but with more detail info
        string[] convertionInfoString;



        //Generating flat terrain       
        newMesh = FlatMeshGenerator.GenerateFlatTerrain(targetTerrain, out convertionInfo, out convertionInfoString, flatOptions);

        //Check reports
        if (convertionInfoString != null)
            for (int i = 0; i < convertionInfoString.Length; i++)
            {
                Debug.LogWarning(convertionInfoString[i]);
            }


        //Successful conversation
        if (newMesh != null)
        {
            for (int i = 0; i < newMesh.Length; i++)
            {
                //Create new gameobject for each chunk
                GameObject chunk = new GameObject(newMesh[i].name);
                chunk.AddComponent<MeshFilter>().sharedMesh = newMesh[i];
                chunk.AddComponent<MeshRenderer>().sharedMaterial = vertexColorMaterial;

                
                //Move to parent
                chunk.transform.parent = this.gameObject.transform;
                chunk.transform.localPosition = Vector3.zero;
            }
        }
	}
}
