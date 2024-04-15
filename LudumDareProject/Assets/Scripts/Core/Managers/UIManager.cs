using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public UIPauseMenu pauseMenu_;

    public void OpenPauseMenu()
    {
        pauseMenu_.EnablePauseMenu();
    }


}
