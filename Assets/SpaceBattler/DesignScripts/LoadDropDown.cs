using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace SpaceBattler
{
    public class LoadDropDown : MonoBehaviour
    {
        private TMP_Dropdown dropdown;
        private void Awake()
        {
            dropdown = GetComponent<TMP_Dropdown>();
        }

        void Start()
        {
            UpdateOptions();
            ShipSaveSystem.SavesUpdated += new ShipSaveSystem.TestDelegate(UpdateOptions);
        }

        void UpdateOptions()
        {
            Debug.Log("Updating Options");
            dropdown.options.Clear();
            HashSet<string> saves = ShipSaveSystem.Saves;
            foreach (string s in saves)
            {
                dropdown.options.Add(new TMP_Dropdown.OptionData(s));
            }
        }
    }
}