using UnityEngine;

public class BecomeChild : MonoBehaviour
{
    [SerializeField]
    string[] tags = {
        "Move",
        "Cube"
    };

    bool CheckNormal(Collision collision) {
        if (collision.contacts[0].normal.y > 0.5f)
            return true;

        return false;
    }

    bool IsValidObject(GameObject obj) {
        foreach (string item in tags)
            if(obj.CompareTag(item))
                return true;

        return false;
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

    private void OnCollisionEnter(Collision collision) {
        HandleCollision(collision);
    }
}
