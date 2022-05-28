using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The controller for growing vines out of the ground in Mokosh's area.
/// </summary>
public class GrowVines : MonoBehaviour
{
    /// <summary>
    /// A list of all the vine objects that will be grown out.
    /// </summary>
    [SerializeField]
    private List<MeshRenderer> vineMeshes;

    /// <summary>
    /// How long it takes for the vine mesh to grow out until its max growth.
    /// </summary>
    [SerializeField]
    private float timeToGrow = 5f;

    /// <summary>
    /// The delay for the vine to grow incrementally.
    /// </summary>
    [SerializeField]
    private float refreshRate = 0.05f;

    /// <summary>
    /// The minimum growth value of the vine mesh.
    /// </summary>
    [SerializeField]
    [Range(0, 1)]
    private float minGrowth = 0.2f;

    /// <summary>
    /// The maximum growth value of the vine mesh.
    /// </summary>
    [SerializeField]
    [Range(0, 1)]
    private float maxGrowth = 0.97f;

    /// <summary>
    /// A list of all the GrowVine materials that will be adjusted to affect each of the vine meshes.
    /// </summary>
    [SerializeField]
    private List<Material> vineGrowMaterials = new List<Material>();

    /// <summary>
    /// Whether the vine has fully grown out or not.
    /// </summary>
    [SerializeField]
    private bool isFullyGrown;


    void Start()
    {
        //For each vine mesh that has to be grown, add its GrowVines material to the vineGrowMaterials array if the _Grow property exists
        for (int i = 0; i <  vineMeshes.Count; i++)
        {
            for (int j = 0; j < vineMeshes[i].materials.Length; j++)
            {
                Material vineMeshMaterial = vineMeshes[i].materials[j];
                if (vineMeshMaterial.HasProperty("_Grow"))
                {
                    vineMeshMaterial.SetFloat("_Grow", minGrowth);
                    vineGrowMaterials.Add(vineMeshMaterial);
                }
                else
                {
                    Debug.LogError("No reference to the _Grow property exists on the current material.");
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            for (int i = 0; i < vineGrowMaterials.Count; i++)
            {
                StartCoroutine(GrowVinesCoroutine(vineGrowMaterials[i]));
            }
        }
    }

    /// <summary>
    /// Make the vine either grow to its maximum growth or shrink to its minimum growth depending on whether it is fully grown or not.
    /// </summary>
    /// <param name="material"></param>
    /// <returns></returns>
    private IEnumerator GrowVinesCoroutine(Material material)
    {
        float currentGrowValue = material.GetFloat("_Grow");

        if (!isFullyGrown)
        {
            while (currentGrowValue < maxGrowth)
            {
                currentGrowValue += 1 / (timeToGrow / refreshRate);
                material.SetFloat("_Grow", currentGrowValue);

                yield return new WaitForSeconds(refreshRate);
            }
        }
        else
        {
            while (currentGrowValue > minGrowth)
            {
                currentGrowValue -= 1 / (timeToGrow / refreshRate);
                material.SetFloat("_Grow", currentGrowValue);

                yield return new WaitForSeconds(refreshRate);
            }
        }

        isFullyGrown = currentGrowValue >= maxGrowth ? true : false;
    }

}
