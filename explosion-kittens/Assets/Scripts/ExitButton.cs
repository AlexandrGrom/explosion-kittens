using Photon.Pun;

public class ExitButton : ButtonAnimation
{
    protected override void Awake()
    {
        base.Awake();
        button.onClick.AddListener(()=>PhotonNetwork.LeaveRoom());
    }
}
