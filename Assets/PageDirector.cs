// PageDirector.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartDirector : MonoBehaviour
{
    // Scene을 변경하는 메서드
    public void SceneChange()
    {
        // "start_track_play"라는 Scene으로 전환
        SceneManager.LoadScene("start_track_play");
    }
}