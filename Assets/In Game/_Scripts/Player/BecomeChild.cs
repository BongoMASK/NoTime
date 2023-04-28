using UnityEngine;

public class BecomeChild : MonoBehaviour {
    [SerializeField] LayerMask whatIsParent;

    bool CheckNormal(Collision collision) {
        float normal = collision.contacts[0].normal.y;
        if (normal > 0.5f)
            return true;

        return false;
    }

    bool IsValidObject(GameObject obj) {
        return whatIsParent == (whatIsParent | (1 << obj.layer));
    }

    void BecomeChildOfGameObject(GameObject obj) {
        transform.parent = obj.transform;
    }

    void HandleCollision(Collision collision) {
        if (CheckNormal(collision)) {
            if (IsValidObject(collision.gameObject))
                BecomeChildOfGameObject(collision.gameObject);
            else
                transform.parent = null;
        }
        else
            transform.parent = null;
    }

    private void OnCollisionStay(Collision collision) {
        HandleCollision(collision);
    }

    private void OnCollisionExit(Collision collision) {
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }
}
