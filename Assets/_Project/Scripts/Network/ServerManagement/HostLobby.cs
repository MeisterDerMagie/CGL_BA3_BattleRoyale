using System.Collections;
using System.Collections.Generic;
using Doodlenite;
using UnityEngine;
using UnityEngine.SceneManagement;
using Wichtel.SceneManagement;

namespace Doodlenite {
public class HostLobby : MonoBehaviour
{
    [SerializeField] private SceneReference loadingScreen;

    public void Host()
    {
        ServerProviderCommunication.Instance.HostRequest();
        SceneManager.LoadSceneAsync(loadingScreen);
    }
}
}