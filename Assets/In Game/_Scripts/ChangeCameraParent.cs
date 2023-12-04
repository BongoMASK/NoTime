using UnityEngine;


public class ChangeCameraParent : MonoBehaviour
{

    public enum axis {
        xAxis,
        yAxis,
        zAxis
    }


    [Header("Which Direction")]

    [SerializeField] axis whichAxis = axis.zAxis;

    [SerializeField] bool reverse = false;

    [Header("Parents info")]

    [SerializeField] Transform defaultParent;
    [SerializeField] Transform otherParent;

    void ChangeTheCameraParent(GameObject other) {

        if(otherParent == null) {
            Debug.Log("No parent assigned");
            return;
        }

        switch (whichAxis) {
            case axis.xAxis:
                if (other.transform.position.x > transform.position.x)
                    CameraManager.instance.ChangeCameraParent(otherParent);
                else if (other.transform.position.x < transform.position.x)
                    CameraManager.instance.ChangeCameraParent(defaultParent);
                break;

            case axis.yAxis:
                if (other.transform.position.y > transform.position.y)
                    CameraManager.instance.ChangeCameraParent(otherParent);
                else if (other.transform.position.y < transform.position.y)
                    CameraManager.instance.ChangeCameraParent(defaultParent);
                break;

            case axis.zAxis:
                if (other.transform.position.z > transform.position.z)
                    CameraManager.instance.ChangeCameraParent(otherParent);
                else if (other.transform.position.z < transform.position.z)
                    CameraManager.instance.ChangeCameraParent(defaultParent);
                break;

            default:
                Debug.Log("No change in parent.");
                return;
                break;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player")) {
            ChangeTheCameraParent(other.gameObject);
        }
    }

    private void OnDrawGizmosSelected() {

        Vector3 direction = Vector3.zero;

        switch (whichAxis) {
            case axis.xAxis:
                direction = Vector3.right;
                break;
         
            case axis.yAxis:
                direction = Vector3.up;
                break;
            
            case axis.zAxis:
                direction = Vector3.forward;
                break;
        }

        int pos = 1;
        if (reverse)
            pos = -1;

        Gizmos.DrawRay(transform.position, 3 * pos * direction);
    }
}
