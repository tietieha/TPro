using HUDUI;
using UnityEngine;

public class TestHud : MonoBehaviour
{
    public Camera SceneCamera;
    public HUDBehaviour eCSRenderManager;
    public GameObject target;
    int title1 = 0;
    int title2 = 0;

    public GameObject target2;

    // Start is called before the first frame update
    void Start()
    {
        eCSRenderManager_Init();
    }

    void eCSRenderManager_Init()
    {
        eCSRenderManager.Init(SceneCamera);
        title1 = eCSRenderManager.CreateHpTitle(1, 1, 0, 0, 1, target.transform);
        title2 = eCSRenderManager.CreateHpTitle(1, 2, 0, 0, 1, target2.transform);
    }

    void eCSRenderManager_HP()
    {
        //     eCSRenderManager.ShowJumpWorld(1,(int)JumpWorldType.Damage, target.transform, -1000, 1);
        //     eCSRenderManager.ShowJumpWorld(1, (int)JumpWorldType.MagicDamage, target.transform, -999, 1);
        // eCSRenderManager.ShowJumpWorld(1, (int)JumpWorldType.MagicDeny, target2.transform, -888, 1, 20);
        // eCSRenderManager.ShowJumpWorld(2, (int)JumpWorldType.MagicImmune, target.transform, -777, -4, 1);
        // eCSRenderManager.ShowJumpWorld(2, (int)JumpWorldType.MagicResist, target.transform, -666, 1, 5);
        // eCSRenderManager.ShowJumpWorld(2, (int)JumpWorldType.MagicUnEffect, target.transform, -666, 1);
        // eCSRenderManager.ShowJumpWorld(1, (int)JumpWorldType.Cure, target.transform, 568, 1);
        // eCSRenderManager.ShowJumpWorld(1, (int)JumpWorldType.Miss, target.transform, -555, 1);
        // eCSRenderManager.ShowJumpWorld(1, (int)JumpWorldType.ShieldMinus, target.transform, -444, 1);
        // eCSRenderManager.ShowJumpWorld(1, (int)JumpWorldType.ShieldAdd, target.transform, -333, 1);
        // eCSRenderManager.ShowJumpWorld(1, (int)JumpWorldType.AttAdd, target.transform, -222, 1);
        // eCSRenderManager.ShowJumpWorld(1, (int)JumpWorldType.AttMinus, target.transform, -111, 1);

        eCSRenderManager.ShowHpTitle(1, true,20);
        // eCSRenderManager.ShowHpTitle(2, true,5);
        eCSRenderManager.SetHpRate(1, 0.5f,1,10);
        //eCSRenderManager.ShowHpTitle(1, false);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    eCSRenderManager_Init();
        //}
        if (Input.GetKeyDown(KeyCode.B))
        {
            eCSRenderManager_HP();
        }
    }
}