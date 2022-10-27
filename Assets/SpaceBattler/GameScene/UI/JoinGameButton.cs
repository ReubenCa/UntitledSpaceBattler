using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SpaceBattler
{
    public class JoinGameButton : MonoBehaviour
    {
        public void OnClick()
        {
            ClientGameManager.instance.PlayerObj.JoinGame();
            gameObject.SetActive(false);
        }
        public void Enable()
        {
            gameObject.SetActive(true);
        }
    }
}