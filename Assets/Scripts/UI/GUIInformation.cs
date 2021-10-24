using System.Collections.Generic;
using System.Globalization;
using Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIInformation : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private TMP_Text healthVal;
    
    [SerializeField] private TMP_Text textEvent;

    [SerializeField] private TMP_Text healPotionCount;
    [SerializeField] private TMP_Text coinsCount;
    
    [SerializeField] private GameObject loadingScreen;
    
    [Header("Stats")]
    [SerializeField] private TMP_Text damageVal;
    [SerializeField] private TMP_Text attackSpeedVal;
    [SerializeField] private TMP_Text speedVal;
    [SerializeField] private TMP_Text luckVal;
    [SerializeField] private TMP_Text powerVal;
    
    private Player _player;

    public void ShowTextEvent(string text)
    {
        textEvent.text = text;
        textEvent.gameObject.SetActive(false);
        textEvent.gameObject.SetActive(true);
    }
    
    public void ShowLoadingScreen()
    {
        loadingScreen.SetActive(true);
    }
    
    private void OnEnable()
    {
        _player = FindObjectOfType<Player>();
        _player.changeHealth += OnChangeHealthValue;
        _player.changeItems += OnChangeItems;
        _player.changeStats += OnChangeStats;
    }

    private void OnDisable()
    {
        _player.changeHealth -= OnChangeHealthValue;
        _player.changeItems -= OnChangeItems;
        _player.changeStats -= OnChangeStats;
    }

    private void OnChangeHealthValue(float currentVal, float maxVal)
    {
        healthBar.maxValue = maxVal;
        healthBar.value = currentVal;
        healthVal.text = currentVal.ToString(CultureInfo.InvariantCulture);
    }

    private void OnChangeItems(Dictionary<DropItemType, int> items)
    {
        healPotionCount.text = items[DropItemType.HealPotion].ToString();
        coinsCount.text = items[DropItemType.Coin].ToString();
    }
    
    private void OnChangeStats(PlayerStats stats)
    {
        damageVal.text = stats.Damage.ToString();
        attackSpeedVal.text = stats.AttackSpeed.ToString();
        speedVal.text = stats.Speed.ToString();
        luckVal.text = stats.Luck.ToString();
        powerVal.text = stats.FireAspect.ToString();
    }

}
