namespace Features.UserLogin.Requests
{
    public record InformationDetailRequest
    (
        int Age,
        string Gender,
        float Height,
        float Weight,
        float R,
        float TargetWeight
    );
}
