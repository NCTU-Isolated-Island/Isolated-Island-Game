using UnityEngine;
using System.Collections;

//Flat Mesh Generator API is here ↓
using VacuumShaders.FlatMeshGenerator;


[AddComponentMenu("VacuumShaders/Flat Mesh Generator/Example/Runtime Mesh Combine")]
public class Runtime_MeshCombine : MonoBehaviour 
{
    //////////////////////////////////////////////////////////////////////////////
    //                                                                          // 
    //Variables                                                                 //                
    //                                                                          //               
    //////////////////////////////////////////////////////////////////////////////

    //All meshes inside 'meshfilterCollection' will be combined into one mesh
    public Transform meshfilterCollection;


    //Describes which textures and colors will be baked and how
    public FlatMeshOptions flatOptions;



    //This material will be used on final mesh
    public Material vertexColorMaterial;


    //////////////////////////////////////////////////////////////////////////////
    //                                                                          // 
    //Unity Functions                                                           //                
    //                                                                          //               
    //////////////////////////////////////////////////////////////////////////////
	void Start () 
    {
        //First check if meshes inside 'meshfilterCollection' can be combined
        FlatMeshGenerator.COMBINE_INFO combineInfo;

        combineInfo = FlatMeshGenerator.CanBeMeshesCombined(meshfilterCollection);
        if (combineInfo != FlatMeshGenerator.COMBINE_INFO.OK)
        {
            //Houston we have a problem
            Debug.LogError(combineInfo.ToString());

            return;
        }


        
        //Will contain bake results 
        Mesh newMesh = null;

        //Will contain baking reports, will help if something goes wrong
        FlatMeshGenerator.CONVERTION_INFO[] convertionInfo;

        //Same as above but with more detail info
        string[] convertionInfoString;



        //Generating flat meshes and then combining  
       newMesh = FlatMeshGenerator.GenerateFlatMeshesAndThenCombine(meshfilterCollection, out convertionInfo, out convertionInfoString, flatOptions);


        //Check reports
        if (convertionInfoString != null)
            for (int i = 0; i < convertionInfoString.Length; i++)
            {
                if (convertionInfo[i] != FlatMeshGenerator.CONVERTION_INFO.Ok)
                    Debug.LogWarning(convertionInfoString[i]);
            }


        //Successful conversation
        if (newMesh != null)
        {
            gameObject.AddComponent<MeshFilter>().sharedMesh = newMesh;
            gameObject.AddComponent<MeshRenderer>().sharedMaterial = vertexColorMaterial;
        }
	}
}
