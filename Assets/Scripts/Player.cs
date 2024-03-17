using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private SpaceGroup spaceGroup;

    private Character originCharacter;

    private int positionX;
    public int PositionX => positionX;
    private int positionY;
    public int PositionY => positionY;
    private ushort innerPositionX;
    public ushort InnerPositionX => innerPositionX;
    private ushort innerPositionY;
    public ushort InnerPositionY => innerPositionY;

    private bool moveable = true;

    private IEnumerator Start()
    {
        positionX = (ushort)(spaceGroup.Width >> 1);
        positionY = (ushort)(spaceGroup.Height >> 1);
        innerPositionX = 2;
        innerPositionY = 2;
        originCharacter = spaceGroup.GetCharacter((ushort)positionX, (ushort)positionY, innerPositionX, innerPositionY);

        SetPosition(Vector2.zero);

        while (true)
        {
            Move();

            yield return null;
        }
    }

    private void Move()
    {
        if (!moveable)
        {
            return;
        }

        moveable = false;

        Invoke(nameof(EndMove), 0.1f);

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector2 direction = new Vector2(x, y);

        if (direction == Vector2.zero)
        {
            return;
        }

        if (direction.x != 0f && direction.y != 0f)
        {
            direction.x = 0f;
        }

        SetPosition(direction);
    }

    private void EndMove()
    {
        moveable = true;
    }

    private void SetPosition(Vector2 direction)
    {
        transform.position = new Vector3(positionX * 5f - (spaceGroup.Width >> 1) * 5f, positionY * 5f - (spaceGroup.Height >> 1) * 5f, 0f);

        spaceGroup.SetCharacter((ushort)positionX, (ushort)positionY, innerPositionX, innerPositionY, originCharacter);

        if (direction == Vector2.up)
        {
            if (innerPositionY == 4)
            {
                positionY++;
                innerPositionY = 0;

                spaceGroup.Render();
            }
            else
            {
                innerPositionY++;
            }
        }
        else if (direction == Vector2.left)
        {
            if (innerPositionX == 0)
            {
                positionX--;
                innerPositionX = 4;

                spaceGroup.Render();
            }
            else
            {
                innerPositionX--;
            }
        }
        else if (direction == Vector2.down)
        {
            if (innerPositionY == 0)
            {
                positionY--;
                innerPositionY = 4;

                spaceGroup.Render();
            }
            else
            {
                innerPositionY--;
            }
        }
        else if (direction == Vector2.right)
        {
            if (innerPositionX == 4)
            {
                positionX++;
                innerPositionX = 0;

                spaceGroup.Render();
            }
            else
            {
                innerPositionX++;
            }
        }

        if (positionX < 0)
        {
            positionX = 0;
            innerPositionX = 0;

            spaceGroup.Render();
        }
        else if (positionX >= spaceGroup.Width)
        {
            positionX = spaceGroup.Width - 1;
            innerPositionX = 4;

            spaceGroup.Render();
        }

        if (positionY < 0)
        {
            positionY = 0;
            innerPositionY = 0;

            spaceGroup.Render();
        }
        else if (positionY >= spaceGroup.Height)
        {
            positionY = spaceGroup.Height - 1;
            innerPositionY = 4;

            spaceGroup.Render();
        }

        originCharacter = spaceGroup.GetCharacter((ushort)positionX, (ushort)positionY, innerPositionX, innerPositionY);

        spaceGroup.SetCharacter((ushort)positionX, (ushort)positionY, innerPositionX, innerPositionY, Character.AtSign);
    }
}
