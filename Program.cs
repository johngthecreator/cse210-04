using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unit04.Game.Casting;
using Unit04.Game.Directing;
using Unit04.Game.Services;


namespace Unit04
{
    /// <summary>
    /// The program's entry point.
    /// </summary>
    class Program
    {
        private static int FRAME_RATE = 12;
        private static int MAX_X = 900;
        private static int MAX_Y = 600;
        private static int CELL_SIZE = 15;
        private static int FONT_SIZE = 20;
        private static int COLS = 60;
        private static int ROWS = 40;
        private static string CAPTION = "Robot Finds Kitten";
        private static string DATA_PATH = "Data/messages.txt";
        private static Color WHITE = new Color(255, 255, 255);
        private static int DEFAULT_ARTIFACTS = 20;


        /// <summary>
        /// Starts the program using the given arguments.
        /// </summary>
        /// <param name="args">The given arguments.</param>
        static void Main(string[] args)
        {
            // create the cast
            Cast cast = new Cast();

            // create the banner
            Actor banner = new Actor();
            banner.SetText("");
            banner.SetFontSize(FONT_SIZE);
            banner.SetColor(WHITE);
            banner.SetPosition(new Point(CELL_SIZE, 0));
            cast.AddActor("banner", banner);

            // create the robot
            Actor robot = new Actor();
            robot.SetText("#");
            robot.SetFontSize(FONT_SIZE);
            robot.SetColor(WHITE);
            robot.SetPosition(new Point(MAX_X / 2, MAX_Y / -40));
            cast.AddActor("robot", robot);

            // load the messages
            List<string> messages = File.ReadAllLines(DATA_PATH).ToList<string>();

            // create the artifacts
            Random random = new Random();
            for (int i = 0; i < DEFAULT_ARTIFACTS; i++)
            {
                string text = ((char)random.Next(33, 126)).ToString();
                string message = messages[i];

                int x = random.Next(1, COLS);
                int y = random.Next(1, ROWS);

                int r = random.Next(0, 256);
                int g = random.Next(0, 256);
                int b = random.Next(0, 256);
                Color rockColor = new Color(50 ,90 ,150 );
                Color gemColor = new Color(50 ,170 ,210 );
                for(int rockCount = 0; rockCount <= 5; rockCount++){
                    Point position_rock = new Point(x*rockCount, 4);
                    position_rock = position_rock.Scale(CELL_SIZE);
                    FallingObject rock = new FallingObject(-5);
                    rock.SetText("O");
                    rock.SetFontSize(FONT_SIZE);
                    rock.SetColor(rockColor);
                    rock.SetPosition(position_rock);
                    cast.AddActor("rocks", rock);
                }
                for(int gemCount = 0; gemCount <= 5; gemCount++){
                    FallingObject gem = new FallingObject(10);
                    Point position_gem = new Point(x+3*gemCount, 5);
                    position_gem = position_gem.Scale(CELL_SIZE);
                    gem.SetText("*");
                    gem.SetFontSize(FONT_SIZE);
                    gem.SetColor(gemColor);
                    gem.SetPosition(position_gem);
                    cast.AddActor("gems", gem);
                }
            }

            // start the game
            KeyboardService keyboardService = new KeyboardService(CELL_SIZE);
            VideoService videoService 
                = new VideoService(CAPTION, MAX_X, MAX_Y, CELL_SIZE, FRAME_RATE, false);
            Director director = new Director(keyboardService, videoService);
            director.StartGame(cast);
        }
    }
}