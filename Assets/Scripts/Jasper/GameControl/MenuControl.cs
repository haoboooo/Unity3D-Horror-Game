using UnityEngine;

public class MenuControl : MonoBehaviour
{
    public GameObject mainPage;
    public GameObject guidePage;

    public void MainPageGuideButton()
    {
        mainPage.SetActive(false);
        guidePage.SetActive(true);
    }

    public void GuidePageBackButton()
    {
        mainPage.SetActive(true);
        guidePage.SetActive(false);
    }
}
