using UnityEngine;

public class ReloadLayoutOnExitGame : MonoBehaviour
{
#if UNITY_EDITOR
    private bool hadSaveOnRunTime = false;
    private bool isRunningGame = false;

    public bool SetHadSaveOnRunTime(bool value)
    {
        if (isRunningGame)
            hadSaveOnRunTime = value;

        return hadSaveOnRunTime;
    }

    private void Start()
    {
        hadSaveOnRunTime = false;
        isRunningGame = true;
        //Debug.Log("ReloadLayoutOnExitGame Start()");
    }

    //after exit game from unity editor, reload layouts which has been saved during the run
    private void OnApplicationQuit()
    {
        //Debug.Log("ReloadLayoutOnExitGame OnApplicationQuit()"+ hadSaveOnRunTime.ToString());
        if (hadSaveOnRunTime && U3DExtends.UIEditorConfigure.ReloadLayoutOnExitGame)
        {
            U3DExtends.UIEditorHelper.DelayReLoadLayout(gameObject, true);
            hadSaveOnRunTime = false;
        }

        isRunningGame = false;
    }
#endif
}