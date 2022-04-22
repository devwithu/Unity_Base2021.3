using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseGameCenterManager : MonoBehaviour
{
    
    public void OnClickButton()
    {
        Debug.Log("LoginProcess_GameCenter");
        SignInWithGameCenterAsync();
    }

    public Task SignInWithGameCenterAsync()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        var credentialTask = Firebase.Auth.GameCenterAuthProvider.GetCredentialAsync();
        var continueTask = credentialTask.ContinueWithOnMainThread(task =>
        {
            if (!task.IsCompleted)
                return null;

            if (task.Exception != null)
                Debug.Log("GC Credential Task - Exception: " + task.Exception.Message);

            var credential = task.Result;

            var loginTask = auth.SignInWithCredentialAsync(credential);
            return loginTask.ContinueWithOnMainThread(HandleSignInWithUser);
        });

        return continueTask;
    }


    // Called when a sign-in without fetching profile data completes.
    void HandleSignInWithUser(Task<Firebase.Auth.FirebaseUser> task)
    {
        //EnableUI();
        if (LogTaskCompletion(task, "Sign-in"))
        {
            Debug.Log(String.Format("{0} signed in", task.Result.DisplayName));
            Debug.Log(String.Format("{0} UserId", task.Result.UserId));
        }
    }

    // Log the result of the specified task, returning true if the task
    // completed successfully, false otherwise.
    protected bool LogTaskCompletion(Task task, string operation)
    {
        bool complete = false;
        if (task.IsCanceled)
        {
            Debug.Log(operation + " canceled.");
        }
        else if (task.IsFaulted)
        {
            Debug.Log(operation + " encounted an error.");
            foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
            {
                string authErrorCode = "";
                Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
                if (firebaseEx != null)
                {
                    authErrorCode = String.Format("AuthError.{0}: ",
                        ((Firebase.Auth.AuthError)firebaseEx.ErrorCode).ToString());
                }

                Debug.Log(authErrorCode + exception.ToString());
            }
        }
        else if (task.IsCompleted)
        {
            Debug.Log(operation + " completed");
            complete = true;
        }

        return complete;
    }
}