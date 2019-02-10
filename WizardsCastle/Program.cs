using System;
using WizardsCastle.Logic;
using WizardsCastle.Logic.Data;

namespace WizardsCastle
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var game = new Game(GameConfig.Standard);
                game.Play();
            }
            catch (Exception ex)
            {
                Console.WriteLine("==========================================");
                Console.WriteLine("*************CRASH************************");
                Console.WriteLine(ex.ToString());
                Environment.Exit(101);
            }

            
        }
    }
}
