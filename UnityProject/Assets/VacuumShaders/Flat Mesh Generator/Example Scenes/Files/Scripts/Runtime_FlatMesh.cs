using UnityEngine;
using System.Collections;

//Flat Mesh Generator API is here ↓
using VacuumShaders.FlatMeshGenerator;


[AddComponentMenu("VacuumShaders/Flat Mesh Generator/Example/Runtime Flat Mesh")]
public class Runtime_FlatMesh : MonoBehaviour 
{
    //////////////////////////////////////////////////////////////////////////////
    //                                                                          // 
    //Variables                                                                 //                
    //                                                                          //               
    //////////////////////////////////////////////////////////////////////////////

    //Describes surface type
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
        Mesh origianlMesh = null;

        //Get original mesh from MeshFilter or SkinnedMeshRenderer
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        SkinnedMeshRenderer skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        if (meshFilter != null)
            origianlMesh = meshFilter.sharedMesh;

        if (origianlMesh == null && skinnedMeshRenderer != null)
            origianlMesh = skinnedMeshRenderer.sharedMesh;


        //Oops, no mesh found
        if (origianlMesh == null)
            return;



        //Will contain bake results 
        Mesh newMesh = null;

        //Will contain baking reports, will help if something goes wrong
        FlatMeshGenerator.CONVERTION_INFO[] convertionInfo;

        //Same as above but with more detail info
        string[] convertionInfoString;
        


        //Generating flat mesh     
        newMesh = FlatMeshGenerator.GenerateFlatMesh(GetComponent<Renderer>(), out convertionInfo, out convertionInfoString, flatOptions);


        //Check reports
        if (convertionInfoString != null)
            for (int i = 0; i < convertionInfoString.Length; i++)
            {
                Debug.Log(convertionInfoString[i]);
            }


        //Successful conversation
        if (newMesh != null)
        {
            //Replace old mesh with new one
            if (meshFilter != null)
                meshFilter.sharedMesh = newMesh;
            else if (skinnedMeshRenderer != null)
                skinnedMeshRenderer.sharedMesh = newMesh;


            //Replace material to make baked data visible
            GetComponent<Renderer>().sharedMaterials = new Material[] { vertexColorMaterial };
        }
	}
}
