//using UnityEditor;
//using UnityEditor.SceneManagement;
//using UnityEngine;
//using UnityEngine.UI;

//[CustomEditor(typeof(UIButtonHandler))]
//public class UIButtonHandlerEditor : Editor {
//    UIButtonHandler uIButtonHandler;

//    public override void OnInspectorGUI() {
//        base.OnInspectorGUI();

//        if (GUILayout.Button("Apply Theme")) {
//            uIButtonHandler = (UIButtonHandler)target;
//            ApplyTheme();
//        }
//    }

//    public void ApplyTheme() {

//        ButtonStyle[] buttonStyles = FindObjectsOfType<ButtonStyle>();
//        foreach (var item in buttonStyles) {
//            item.ApplyButtonStyle(uIButtonHandler);

//            EditorUtility.SetDirty(item.gameObject.GetComponent<Image>());
//        }
//        EditorSceneManager.MarkSceneDirty(uIButtonHandler.gameObject.scene);
//    }
//}
