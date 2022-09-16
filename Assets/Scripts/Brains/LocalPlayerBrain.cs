public class LocalPlayerBrain : PaddleBrain
{
    private readonly UIController uiController;

    public LocalPlayerBrain(UIController uiController)
    {
        this.uiController = uiController;
    }

    public InputParams Act(float deltaTime)
    {
        return uiController.InputParams;
    }
}