using System.Collections;
using UnityEngine;

public class CubeLogic : MonoBehaviour
{
    [SerializeField] private ManageScore scoreManager;

    private CubeThrow cubeThrow;
    private Rigidbody rb;
    private FindDiceSide finder;
    private bool hasScored;
    private const float velocityThreshold = 0.00001f;
    private static bool hasResetScoreThisThrow;

    public int Score { get; private set; }

    private void Awake()
    {
        cubeThrow = GetComponent<CubeThrow>();
        rb = GetComponent<Rigidbody>();
        finder = new FindDiceSide();
        scoreManager = Object.FindFirstObjectByType<ManageScore>();
    }

    public void OnThrow()
    {
        if (!hasResetScoreThisThrow)
        {
            scoreManager.ClearScore();
            hasResetScoreThisThrow = true;
        }
        hasScored = false;
        Score = 0;
        cubeThrow.ThrowCube();
        StartCoroutine(WaitForRollEnd());
    }

    private IEnumerator WaitForRollEnd()
    {
        yield return new WaitForFixedUpdate();
        yield return new WaitUntil(() =>
            rb.linearVelocity.magnitude < velocityThreshold &&
            rb.angularVelocity.magnitude < velocityThreshold);
        Score = finder.GetTopFace(transform);
        if (!hasScored && Score > 0)
        {
            scoreManager.IncreaseTotalScore(Score);
            hasScored = true;
        }
        if (transform.parent != null && transform.GetSiblingIndex() == transform.parent.childCount - 1)
        {
            hasResetScoreThisThrow = false;
        }
    }
}
