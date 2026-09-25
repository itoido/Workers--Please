using System;

[Serializable]
public class ApplicantResultData
{
    public ApplicantData applicant;
    public StampMenuUI.StampType playerDecision;
    public bool isCorrect;

    public ApplicantResultData(
        ApplicantData applicant,
        StampMenuUI.StampType playerDecision
    )
    {
        this.applicant = applicant;
        this.playerDecision = playerDecision;

        if (applicant == null)
        {
            isCorrect = false;
            return;
        }

        bool playerRejected =
            playerDecision == StampMenuUI.StampType.Reject;

        isCorrect =
            playerRejected == applicant.isFraud;
    }
}