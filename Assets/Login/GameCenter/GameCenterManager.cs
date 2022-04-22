using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.iOS;
using UnityEngine.SocialPlatforms.GameCenter;

public class GameCenterManager : MonoBehaviour
{

    public void OnClickButtonLogin()
    {
#if UNITY_IOS
        //Apple GameCenter Login
        GameCenterLogin();
#endif
    }
    /// <summary>
    /// Apple GameCenter Login
    /// </summary>
    public void GameCenterLogin()
    {
        if (Social.localUser.authenticated == true)
        {
            Debug.Log("Success to true");
            Debug.Log($"Social.localUser.id : {Social.localUser.id}");
        }
        else
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    Debug.Log("Success to authenticate");
                    Debug.Log($"Social.localUser.id : {Social.localUser.id}");
                }
                else
                {
                    Debug.Log("Faile to login");
                }
            });
        }
    }
}
