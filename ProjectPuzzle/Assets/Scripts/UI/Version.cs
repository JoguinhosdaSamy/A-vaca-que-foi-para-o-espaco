using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Version : MonoBehaviour
    {
        void Start()
        {
            string version = PlayerSettings.bundleVersion;
            Text versionText = GetComponentInChildren<Text>();
            versionText.text = version;
        }
    
    }
}
