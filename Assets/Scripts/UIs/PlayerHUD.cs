using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Team;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private TeamController TeamController;
    [SerializeField] private TextMeshProUGUI ScoreLabel;
    [SerializeField] private TextMeshProUGUI NectarLabel;
    [SerializeField] private TextMeshProUGUI AntsLabel;
    [SerializeField] private Image NectarFillMask;

    private void Update()
    {
        ScoreLabel.text = TeamController.Score.ToString("0");
        NectarLabel.text = TeamController.Nectar.ToString("0");
        AntsLabel.text = TeamController.workers.Count.ToString("0");

        if (NectarFillMask)
            NectarFillMask.fillAmount = TeamController.Nectar / TeamController.AntNectarCost;
    }
}
