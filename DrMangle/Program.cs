namespace DrMangle
{
    public class Program
    {

        public static void Main(string[] args)
        {
            GameController gc = new GameController();            
            bool activeGame = true;

            while (activeGame)
            {
                //try
                //{
                    activeGame = gc.RunGame();
                //}
                //catch (System.Exception ex)
                //{
                //    string currentFile = new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName();
                //    int currentLine = new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileLineNumber();
                //    gc.Repo.LogException(gc.Data, $"General exception {currentFile} line {currentLine}", ex, true);
                //    activeGame = false;
                //}
            }
        }
    }
}
