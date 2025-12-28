using UnityEngine;
using UnityEngine.UI;

public class ImageDigitDisplay : MonoBehaviour
{
    [Header("Resources")]
    public Sprite[] numberSprites;
    public Sprite dashSprite;

    [Header("UI Slots (Order: Hundreds -> Tens -> Units)")]
    public Image[] digitSlots;

    public void SetValue(int value, bool leadingZeros)
    {
        value = Mathf.Clamp(value, 0, 999);

        string numString = value.ToString("D" + digitSlots.Length);
        
        bool encounteredNonZero = false;

        for (int i = 0; i < digitSlots.Length; i++)
        {
            int digit = int.Parse(numString[i].ToString());

            if (digit != 0)
            {
                encounteredNonZero = true;
            }

            bool shouldShow = false;

            if (leadingZeros)
            {
                shouldShow = true;
            }
            else
            {
                if (encounteredNonZero || i == digitSlots.Length - 1)
                {
                    shouldShow = true;
                }
            }

            if (shouldShow)
            {
                digitSlots[i].enabled = true;
                digitSlots[i].sprite = numberSprites[digit];
            }
            else
            {
                digitSlots[i].enabled = false;
            }
        }
    }
}