namespace LR2_QuizMaster
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new LR2_QuizMaster.Forms.MainMenuForm());
        }
    }
}
