using UnityEngine;

public class CameraDetection : MonoBehaviour
{
    public Camera CCTV;
    public GameObject[] puzzleObj;

    void Update()
    {
        foreach (GameObject x in puzzleObj)
        {

            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(CCTV);
            if (GeometryUtility.TestPlanesAABB(planes, x.GetComponent<Collider>().bounds))
            {
                print("The object " + x.name + " has appeared");
            }
            else
            {
                print("The object " + x.name + " has disappeared");
            }

        }
    }
}