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
                activeGame = gc.RunGame();

            }
        }
    }
}
