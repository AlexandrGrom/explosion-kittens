using TMPro;
using UnityEngine;

namespace Lobby
{
    [RequireComponent(typeof(TMP_InputField))]
    public class InputFieldSetUp : MonoBehaviour
    {
        [SerializeField] protected string key = "name";
        [SerializeField] protected string placeholderText = "Enter new name";
        [SerializeField] protected string defaultValue = "Player";
        
        [SerializeField] protected TMP_InputField nameInput;
        [SerializeField] protected TextMeshProUGUI placeholder;
        
        public string Name { get; protected set; }
        

        private void Reset()
        {
            nameInput = GetComponent<TMP_InputField>();
            placeholder = nameInput.placeholder.GetComponent<TextMeshProUGUI>();
        }
        
        private void Awake()
        {
            
#if UNITY_EDITOR
            defaultValue += "Editor";
#endif
            
            Name = PlayerPrefs.GetString(key, defaultValue);
            
            placeholder.text = Name;
            nameInput.text = Name;
            
            nameInput.onSelect.AddListener(f =>
            {
                placeholder.text = placeholderText;
            });
            
            nameInput.onEndEdit.AddListener(data =>
            {
                Name = data;
                placeholder.text = Name;
                PlayerPrefs.SetString(Name, Name);
            });
        }
    }
}
