using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

	

    public void ChangeScene0 ()
    {
        SceneManager.LoadScene(0);

        // Set bool back to false so that is does not interfer with new scenes
        HouseController.house1Added = false;
        ExtHouseController.house1Added = false;
    }

    public void ChangeScene1()
    {
        SceneManager.LoadScene(1);
    }

    public void ChangeScene2()
    {
        SceneManager.LoadScene(2);
    }

    public void ChangeScene3()
    {
        SceneManager.LoadScene(3);
    }

    public void ChangeScene4()
    {
        SceneManager.LoadScene(4);
    }
}
