using System.Collections;
using UnityEngine;
using TMPro;

public class StatisticsSystem : MonoBehaviour {

    public static StatisticsSystem statisticsSystemInstance;
    public GameObject statisticsPanel;
    public ObjectStatistics playerStatistics;
    private Animator animator;
    private TextMeshProUGUI playerStatisticsText;

    private void Awake() {
        if(statisticsSystemInstance != null && statisticsSystemInstance != this) {
            Destroy(gameObject);
        }
        else {
            statisticsSystemInstance = this;
            animator = statisticsPanel.GetComponent<Animator>();
            playerStatisticsText = statisticsPanel.GetComponentInChildren<TextMeshProUGUI>();
            statisticsPanel.SetActive(false);
        }
    }

    private void Update() {
        if (Input.GetKey(KeyCode.Tab)) {
            ShowStatisticsPanel();
        }
        else {
            if (statisticsPanel.activeSelf) {
                StartCoroutine(HideStatisticsPanel());
            }
        }
    }

    private void StatisticsUpdate() {
        playerStatisticsText.text = "";
        for (int i=0; i<playerStatistics.statistic.Length; i++) {
            if (playerStatistics.statistic[i].name == "Health" /* || playerStatistics.statistic[i].name == "Stamina" || playerStatistics.statistic[i].name == "Mana"*/) {
                playerStatisticsText.text += playerStatistics.statistic[i].name + ": " + playerStatistics.statistic[i].currentValue + "/" + playerStatistics.statistic[i].baseValue + "\n";
            }
            else {
                playerStatisticsText.text += playerStatistics.statistic[i].name + ": " + playerStatistics.statistic[i].baseValue + "\n";
            }
        }
        /*if (playerStatistics.GetComponentInChildren<Weapon>()) {
            playerStatisticsText.text += "Equipped weapon: " + GetComponentInChildren<Weapon>().name;
        }
        else {
            playerStatisticsText.text += "Equipped weapon: none";
        }*/
        playerStatisticsText.text += "Equipped weapon: none";
    }

    private void ShowStatisticsPanel() {
        statisticsPanel.SetActive(true);
        StatisticsUpdate();
        animator.SetBool("IsVisible", true);
    }

    IEnumerator HideStatisticsPanel() {
        animator.SetBool("IsVisible", false);
        yield return new WaitForSeconds(0.25f);
        statisticsPanel.SetActive(false);
    }
}
