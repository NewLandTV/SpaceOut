using System.Collections.Generic;
using UnityEngine;

public enum Character : byte
{
    Null = 0,
    Number_0 = 1,
    Number_1 = 2,
    Number_2 = 3,
    Number_3 = 4,
    Number_4 = 5,
    Number_5 = 6,
    Number_6 = 7,
    Number_7 = 8,
    Number_8 = 9,
    Number_9 = 10,
    A = 11,
    B = 12,
    C = 13,
    D = 14,
    E = 15,
    F = 16,
    G = 17,
    H = 18,
    I = 19,
    J = 20,
    K = 21,
    L = 22,
    M = 23,
    N = 24,
    O = 25,
    P = 26,
    Q = 27,
    R = 28,
    S = 29,
    T = 30,
    U = 31,
    V = 32,
    W = 33,
    X = 34,
    Y = 35,
    Z = 36,
    a = 37,
    b = 38,
    c = 39,
    d = 40,
    e = 41,
    f = 42,
    g = 43,
    h = 44,
    i = 45,
    j = 46,
    k = 47,
    l = 48,
    m = 49,
    n = 50,
    o = 51,
    p = 52,
    q = 53,
    r = 54,
    s = 55,
    t = 56,
    u = 57,
    v = 58,
    w = 59,
    x = 60,
    y = 61,
    z = 62,
    AtSign = 63
}

public class Space : MonoBehaviour
{
    private SpaceGroup spaceGroup;

    [SerializeField]
    private SpriteRenderer[] spriteRenderer;

    public struct Data
    {
        public Character character;

        public bool passable;

        public Data(Character character, bool passable)
        {
            this.character = character;
            this.passable = passable;
        }

        public static Character CharToCharacter(char ch)
        {
            if (ch == ' ')
            {
                return Character.Null;
            }
            else if (ch >= 48 && ch <= 57)
            {
                return (Character)ch - 47;
            }
            else if (ch >= 65 && ch <= 90)
            {
                return (Character)ch - 54;
            }
            else if (ch >= 97 && ch <= 122)
            {
                return (Character)ch - 61;
            }

            return Character.Null;
        }

        public static Character[] StringToCharacters(string str)
        {
            List<Character> result = new List<Character>();

            for (ushort i = 0; i < str.Length; i++)
            {
                result.Add(CharToCharacter(str[i]));
            }

            return result.ToArray();
        }

        public static Color CharacterToColor(Character character)
        {
            if (character == Character.Null)
            {
                return Color.black;
            }

            if (character >= Character.Number_0 && character <= Character.Number_9)
            {
                return Color.green;
            }

            if (character >= Character.A && character <= Character.z)
            {
                return Color.blue;
            }

            if (character == Character.AtSign)
            {
                return Color.yellow;
            }

            return Color.white;
        }
    }

    private Data[] datas;

    private ushort width;
    private ushort height;

    public void Setup(SpaceGroup spaceGroup)
    {
        this.spaceGroup = spaceGroup;

        GenerateSpaceDatas(5, 5);
        GenerateCharacters();
    }

    public Character GetCharacter(ushort x, ushort y) => datas[x + y * width].character;

    public void SetCharacter(ushort x, ushort y, Character character)
    {
        datas[x + y * width].character = character;

        GenerateCharacter((uint)(x + y * width));
    }

    private void GenerateSpaceDatas(ushort width, ushort height)
    {
        datas = new Data[width * height];

        this.width = width;
        this.height = height;

        const string chars = " 0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        for (uint i = 0; i < width * height; i++)
        {
            datas[i] = new Data(Data.CharToCharacter(chars[Random.Range(0, chars.Length)]), true);
        }
    }

    private void GenerateCharacter(uint index)
    {
        spriteRenderer[index].sprite = spaceGroup.CharacterSprite[(int)datas[index].character];
        spriteRenderer[index].color = Data.CharacterToColor(datas[index].character);
    }

    private void GenerateCharacters()
    {
        for (uint i = 0; i < width * height; i++)
        {
            GenerateCharacter(i);
        }
    }
}
