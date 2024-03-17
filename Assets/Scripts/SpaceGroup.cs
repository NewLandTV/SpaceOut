using System.Collections.Generic;
using UnityEngine;

public class SpaceGroup : MonoBehaviour
{
    [SerializeField]
    private Space spacePrefab;

    private Space[] spaces;

    [SerializeField]
    private ushort width;
    public ushort Width => width;
    [SerializeField]
    private ushort height;
    public ushort Height => height;

    [SerializeField]
    private Sprite[] characterSprite;
    public Sprite[] CharacterSprite => characterSprite;

    [SerializeField]
    private bool useOptimize;
    [SerializeField]
    private ushort renderSpaces;
    [SerializeField]
    private Transform player;

    private List<GameObject> renderSpaceGameObjects = new List<GameObject>();

    private void Awake()
    {
        spaces = new Space[width * height];

        for (ushort w = 0; w < width; w++)
        {
            for (ushort h = 0; h < height; h++)
            {
                spaces[w + h * width] = Instantiate(spacePrefab, new Vector3((w - (width >> 1)) * 5f, (h - (height >> 1)) * 5f), Quaternion.identity, transform);

                spaces[w + h * width].gameObject.SetActive(!useOptimize);

                spaces[w + h * width].name = $"Space [{w}, {h}]";

                spaces[w + h * width].Setup(this);
            }
        }
    }

    private void Start()
    {
        string str = "HELLO WORLD";

        Character[] characterArray = new Character[str.Length];

        for (int i = 0; i < str.Length; i++)
        {
            characterArray[i] = Space.Data.CharToCharacter(str[i]);
        }

        SetCharacters(8, 8, 0, 0, characterArray);
        Render();
    }

    public void Render()
    {
        if (!useOptimize)
        {
            return;
        }

        ushort offsetX = (ushort)(width >> 1);
        ushort offsetY = (ushort)(height >> 1);
        ushort playerX = (ushort)(Mathf.FloorToInt(player.position.x / 5f) + offsetX);
        ushort playerY = (ushort)(Mathf.FloorToInt(player.position.y / 5f) + offsetY);
        
        for (int i = 0; i < renderSpaceGameObjects.Count; i++)
        {
            renderSpaceGameObjects[i].SetActive(false);
        }

        renderSpaceGameObjects.Clear();

        for (int x = playerX - renderSpaces; x < playerX + renderSpaces; x++)
        {
            for (int y = playerY - renderSpaces; y < playerY + renderSpaces; y++)
            {
                if (x < 0 || x >= width || y < 0 || y >= height)
                {
                    continue;
                }

                renderSpaceGameObjects.Add(spaces[x + y * width].gameObject);

                spaces[x + y * width].gameObject.SetActive(true);
            }
        }
    }

    public Character GetCharacter(ushort x, ushort y, ushort innerY, ushort innerX) => spaces[x + y * width].GetCharacter(innerX, innerY);

    public void SetCharacter(ushort x, ushort y, ushort innerY, ushort innerX, Character character)
    {
        spaces[x + y * width].SetCharacter(innerX, innerY, character);
    }

    public void SetCharacters(ushort x, ushort y, ushort innerX, ushort innerY, Character[] characters)
    {
        ushort xIndex = x;
        ushort innerXIndex = innerX;

        for (ushort i = 0; i < characters.Length; i++)
        {
            if (innerXIndex >= 5)
            {
                xIndex++;
                innerXIndex = 0;
            }

            SetCharacter(xIndex, y, innerXIndex++, innerY, characters[i]);
        }
    }
}
