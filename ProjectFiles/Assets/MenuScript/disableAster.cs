using UnityEngine;
using System.Collections;
using VRStandardAssets.Utils;

namespace VRStandardAssets.Menu
{
    public class disableAster : MonoBehaviour
    {
        [SerializeField]
        private VRInteractiveItem m_Item;

        [SerializeField]
        private Animator SkyboxAnimator;
        // Update is called once per frame
        void Update()
        {
            if (!m_Item.IsOver && !m_Item.gameObject.GetComponent<MenuItemPopout>().overrideBool)
            {
                gameObject.SetActive(false);

            }
        }
    }
}
