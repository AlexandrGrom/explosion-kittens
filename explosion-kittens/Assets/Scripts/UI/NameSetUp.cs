using TMPro;
using UnityEngine;

namespace UI
{
    public class NameSetUp : MonoBehaviour
    {
        public const string Name = "name";
        [SerializeField] private TMP_InputField nameInput;

        public string PlayerName { get; private set; }
    

        private void Awake()
        {
            PlayerName = PlayerPrefs.GetString(Name, "Player");
            nameInput.placeholder.GetComponent<TextMeshProUGUI>().text = PlayerName;

            nameInput.onEndEdit.AddListener(data =>
            {
                PlayerName = data;
                nameInput.placeholder.GetComponent<TextMeshProUGUI>().text = PlayerName;
                PlayerPrefs.SetString(Name, PlayerName);
            });
        }
    }
}
