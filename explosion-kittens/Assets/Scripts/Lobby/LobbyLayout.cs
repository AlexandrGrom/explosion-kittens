using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby
{
    public class LobbyLayout : MonoBehaviour
    {
        [SerializeField] private Button createRoom;
        [SerializeField] private Button back;
        [SerializeField] private RectTransform menuRT;
        [SerializeField] private RectTransform createRoomRT;
        [SerializeField] private RectTransform currentRT;
        
        private void Awake()
        {
            createRoomRT.anchoredPosition = new Vector2(currentRT.sizeDelta.x,0);
            createRoom.onClick.AddListener(OpenCreateRoom);
            back.onClick.AddListener(CloseCreateRoom);
        }

        private void OpenCreateRoom()
        {
            menuRT.DOAnchorPosX(-currentRT.sizeDelta.x, 0.3f).SetEase(Ease.InOutSine);
            createRoomRT.DOAnchorPosX(0, 0.3f).SetEase(Ease.InOutSine);
        }

        private void CloseCreateRoom()
        {
            menuRT.DOAnchorPosX(0, 0.3f).SetEase(Ease.InOutSine);
            createRoomRT.DOAnchorPosX(currentRT.sizeDelta.x, 0.3f).SetEase(Ease.InOutSine);
        }
    }
}
