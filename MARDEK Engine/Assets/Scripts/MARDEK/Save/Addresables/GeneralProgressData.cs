using MARDEK.Inventory;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MARDEK.Save
{
    public class GeneralProgressData : AddressableMonoBehaviour
    {
        [SerializeField] string currentScene = default;
        [SerializeField] string _gameName = string.Empty;
        [SerializeField] List<PlotItem> _obtainedPlotItems;

        public List<PlotItem> obtainedPlotItems { get { return _obtainedPlotItems; } }
        public string GameName
        {
            get
            {
                return _gameName;
            }
            set
            {
                if (string.IsNullOrEmpty(_gameName))
                    _gameName = value;
                return;
            }
        }
        
        public override void Save()
        {
            currentScene = SceneManager.GetActiveScene().path;
            base.Save();
        }
        public void LoadScene()
        {
            if (string.IsNullOrEmpty(currentScene))
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            else
                SceneManager.LoadScene(currentScene);
        }
    }
}