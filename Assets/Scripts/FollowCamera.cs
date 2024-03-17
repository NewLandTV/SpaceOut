using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField]
    private SpaceGroup spaceGroup;

    [SerializeField]
    private Player targetPlayer;

    private void Update()
    {
        float x = (targetPlayer.PositionX - (spaceGroup.Width >> 1)) * 5f + targetPlayer.InnerPositionX;
        float y = (targetPlayer.PositionY - (spaceGroup.Height >> 1)) * 5f + targetPlayer.InnerPositionY;

        Vector3 position = new Vector3(x, y, transform.position.z);

        transform.position = position;
    }
}
