using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceBattler
{
    public class PlayerPanel : MonoBehaviour
    {
        [SerializeField]
        Text text;
        [SerializeField]
        Image image;

        public void UpdateInfo(GlobalPlayerInfo info)
        {
            string newtext = (info.AI ? "AI" : "HUMAN") + " ID:" + info.PlayerID;
            text.text = newtext;
            image.color = info.color;
        }
    }
}