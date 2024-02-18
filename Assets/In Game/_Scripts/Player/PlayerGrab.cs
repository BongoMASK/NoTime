//using UnityEngine;

//public class PlayerGrab : MonoBehaviour
//{
//    [SerializeField] PlayerObjectInteraction playerObjectInteraction;

//    [SerializeField, Tooltip("Transform of game object where the object should be after interaction")]
//    private Transform interactedObjectPos;

//    private void Grabbing() {
//        if (!playerObjectInteraction.isCurrentlyInteracting)
//            return;

//        Transform t = playerObjectInteraction.currentInteractedObject.transform;

//        // Puts "object" in "hand" position by moving the "parent"
//        Vector3 dist = t.GetChild(0).position - interactedObjectPos.position;
//        Vector3 targetDist = t.position - dist;
//        t.position = Vector3.Lerp(t.position, targetDist, 0.4f);
//    }

//    public void MakeCurrentInteractedObject() {
//        if()
//    }

//    private void ResetInteractedObject() {
//        if (playerObjectInteraction != null)
//            playerObjectInteraction.currentInteractedObject = null;
//    }
//}
