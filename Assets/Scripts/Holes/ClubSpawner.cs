using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SimpleClubSpawner : MonoBehaviour
{
    public GameObject realClubPrefab;
    private XRInteractionManager interactionManager;
    private XRGrabInteractable myInteractable;

    void Awake()
    {
        interactionManager = FindFirstObjectByType<XRInteractionManager>();
        myInteractable = GetComponent<XRGrabInteractable>();
    }

    public void SpawnAndEquip(SelectEnterEventArgs args)
    {
        if (realClubPrefab == null)
        {
            DropSelf(args.interactorObject);
            return;
        }

        var handInteractor = args.interactorObject;
        GameObject newClub = Instantiate(realClubPrefab, handInteractor.transform.position, handInteractor.transform.rotation);
        newClub.tag = "Club";

        var newClubInteractable = newClub.GetComponent<IXRSelectInteractable>();

        if (newClubInteractable != null && interactionManager != null)
        {
            interactionManager.SelectEnter(handInteractor, newClubInteractable);
            interactionManager.SelectExit(handInteractor, myInteractable);
        }
    }

    private void DropSelf(IXRSelectInteractor hand)
    {
        if (interactionManager != null && myInteractable != null)
        {
            interactionManager.SelectExit(hand, myInteractable);
        }
    }
}