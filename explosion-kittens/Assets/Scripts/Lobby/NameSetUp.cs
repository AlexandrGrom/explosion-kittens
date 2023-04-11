using TMPro;
using UnityEngine;

namespace UI
{
    public class NameSetUp : MonoBehaviour
    {
        private const string Name = "name";
        
        [SerializeField] private TMP_InputField nameInput;

        public string PlayerName { get; private set; }
    

        private void Awake()
        {
            string defaultValue = "Player";
#if UNITY_EDITOR
            defaultValue = "Editor";
#endif
            
            PlayerName = PlayerPrefs.GetString(Name, defaultValue);
            
            nameInput.placeholder.GetComponent<TextMeshProUGUI>().text = PlayerName;
            nameInput.text = PlayerName;
            
            nameInput.onSelect.AddListener(f =>
            {
                nameInput.placeholder.GetComponent<TextMeshProUGUI>().text = "Enter new name";
            });
            
            nameInput.onEndEdit.AddListener(data =>
            {
                PlayerName = data;
                nameInput.placeholder.GetComponent<TextMeshProUGUI>().text = PlayerName;
                PlayerPrefs.SetString(Name, PlayerName);
            });
        }
    }
}
