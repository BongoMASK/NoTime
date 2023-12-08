using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimulatePhysicsScene : MonoBehaviour
{

    private Scene simulationScene;
    private PhysicsScene physicsScene;

    Collider[] obstaclesParent;

    private Dictionary<Transform, Transform> spawnedObjects = new Dictionary<Transform, Transform>();

    private void Start() {
        CreatePhysicsScene();
    }

    private void Update() {

        foreach (var item in spawnedObjects) {
            item.Value.position = item.Key.position;
            item.Value.rotation = item.Key.rotation;
        }
    }

    void CreatePhysicsScene() {

        simulationScene = SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics3D));
        physicsScene = simulationScene.GetPhysicsScene();

        obstaclesParent = FindObjectsOfType<Collider>();

        Debug.Log(obstaclesParent.Length);

        foreach(Collider col in obstaclesParent) {

            if(col.isTrigger) continue;

            Transform obj = col.transform;

            var ghostObj = Instantiate(obj.gameObject, obj.position, obj.rotation);
            Renderer renderer = ghostObj.GetComponent<Renderer>();
            if (renderer != null) {
                renderer.enabled = false;
            }
            SceneManager.MoveGameObjectToScene(ghostObj, simulationScene);

            if (!ghostObj.isStatic)
                spawnedObjects.TryAdd(obj, ghostObj.transform);
        }
    }

    public Playback GetNextPos(Recorder obj, Vector3 pos, Vector3 velocity) {
        var ghostObj = Instantiate(obj, pos, Quaternion.identity);
        SceneManager.MoveGameObjectToScene(ghostObj.gameObject, simulationScene);

        ghostObj.Init(velocity);

        physicsScene.Simulate(Time.fixedDeltaTime);

        Playback playback = new Playback(ghostObj.transform.position, ghostObj.transform.rotation);

        Destroy(ghostObj);
        return playback;
    }
}
