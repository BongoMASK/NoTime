using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level_Selector : MonoBehaviour
{
   public int level;
   
   
    public void OpenScene( )
    {
        SceneManager.LoadScene(level);
    }
}
