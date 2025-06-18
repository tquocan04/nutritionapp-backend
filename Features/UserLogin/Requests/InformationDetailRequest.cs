namespace Features.UserLogin.Requests
{
    public record InformationDetailRequest
    (
        int Age,
        string Gender,
        float Height, // cm
        float Weight, // kg
        float R,
        float TargetWeight // kg
    );
}
