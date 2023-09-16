using System.Globalization;
using TMPro;
using UnityEngine;

namespace GamePlay.Code.Scripts
{
    public class CombatText : MonoBehaviour
    {
        [SerializeField] public TMP_Text hpText;
        
        public void OnInit(float hp)
        {
            hpText.text = hp.ToString();
            Invoke(nameof(OnDespawn), 1f);
        }

        public void OnDespawn()
        {
            if (gameObject != null)
            {
                Destroy(gameObject);
            }
        }
    }
}