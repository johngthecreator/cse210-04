using System.Collections.Generic;
using System;
using Unit04.Game.Casting;
using Unit04.Game.Services;


namespace Unit04.Game.Directing
{
    /// <summary>
    /// <para>A person who directs the game.</para>
    /// <para>
    /// The responsibility of a Director is to control the sequence of play.
    /// </para>
    /// </summary>
    public class Director
    {
        private KeyboardService _keyboardService = null;
        private VideoService _videoService = null;
        private int cellSize = 15;
        private int currScore = 0;
        private int points = 0;

        /// <summary>
        /// Constructs a new instance of Director using the given KeyboardService and VideoService.
        /// </summary>
        /// <param name="keyboardService">The given KeyboardService.</param>
        /// <param name="videoService">The given VideoService.</param>
        public Director(KeyboardService keyboardService, VideoService videoService)
        {
            this._keyboardService = keyboardService;
            this._videoService = videoService;
        }

        /// <summary>
        /// Starts the game by running the main game loop for the given cast.
        /// </summary>
        /// <param name="cast">The given cast.</param>
        public void StartGame(Cast cast)
        {
            _videoService.OpenWindow();
            while (_videoService.IsWindowOpen())
            {
                GetInputs(cast);
                DoUpdates(cast);
                DoOutputs(cast);
            }
            _videoService.CloseWindow();
        }

        /// <summary>
        /// Gets directional input from the keyboard and applies it to the robot.
        /// </summary>
        /// <param name="cast">The given cast.</param>
        private void GetInputs(Cast cast)
        {
            Actor robot = cast.GetFirstActor("robot");
            Point velocity = _keyboardService.GetDirection();
            robot.SetVelocity(velocity);     
        }

        /// <summary>
        /// Updates the robot's position and resolves any collisions with artifacts.
        /// </summary>
        /// <param name="cast">The given cast.</param>
        private void DoUpdates(Cast cast)
        {
            Actor banner = cast.GetFirstActor("banner");
            Actor robot = cast.GetFirstActor("robot");
            List<Actor> rocks = cast.GetActors("rocks");
            List<Actor> gems = cast.GetActors("gems");

            banner.SetText($"Score: {currScore}");
            int maxX = _videoService.GetWidth();
            int maxY = _videoService.GetHeight();
            robot.MoveNext(maxX, maxY);
            Random rand = new Random();

            foreach (Actor actor in rocks)
            {
                int y = rand.Next(0,3);
                Point direct = new Point(0, y);
                direct = direct.Scale(cellSize);
                actor.SetVelocity(direct);
                actor.MoveNext(maxX,maxY);
                if (robot.GetPosition().Equals(actor.GetPosition())){
                    FallingObject rock = (FallingObject) actor;
                    currScore = rock.getPoints() + currScore;
                    banner.SetText($"Score: {currScore}");
                    cast.RemoveActor("rocks", actor);
                }
                makeMoreFalling();
            } 

            foreach (Actor actor in gems)
            {
                int y = rand.Next(0,3);
                Point direct = new Point(0, y);
                direct = direct.Scale(cellSize);
                actor.SetVelocity(direct);
                actor.MoveNext(maxX,maxY);
                if (robot.GetPosition().Equals(actor.GetPosition())){
                    FallingObject gem = (FallingObject) actor;
                    currScore = gem.getPoints() + currScore;
                    banner.SetText($"Score: {currScore}");
                    cast.RemoveActor("gems", actor);
                    makeMoreFalling();
                }
            } 
        }

        /// <summary>
        /// Draws the actors on the screen.
        /// </summary>
        /// <param name="cast">The given cast.</param>
        public void DoOutputs(Cast cast)
        {
            List<Actor> actors = cast.GetAllActors();
            _videoService.ClearBuffer();
            _videoService.DrawActors(actors);
            _videoService.FlushBuffer();
        }
        public void makeMoreFalling(){
            Random random = new Random();
            Cast cast = new Cast();
            for (int i = 0; i < 5; i++)
            {
                int x = random.Next(1, 60);
                int y = random.Next(1, 40);

                int r = random.Next(0, 256);
                int g = random.Next(0, 256);
                int b = random.Next(0, 256);
                Color rockColor = new Color(50 ,90 ,150 );
                Color gemColor = new Color(50 ,170 ,210 );
                for(int rockCount = 0; rockCount <= 5; rockCount++){
                    Point position_rock = new Point(x*rockCount, 4);
                    position_rock = position_rock.Scale(15);
                    FallingObject rock = new FallingObject(-5);
                    rock.SetText("O");
                    rock.SetFontSize(20);
                    rock.SetColor(rockColor);
                    rock.SetPosition(position_rock);
                    cast.AddActor("rocks", rock);
                }
                for(int gemCount = 0; gemCount <= 5; gemCount++){
                    FallingObject gem = new FallingObject(10);
                    Point position_gem = new Point(x+3*gemCount, 5);
                    position_gem = position_gem.Scale(15);
                    gem.SetText("*");
                    gem.SetFontSize(20);
                    gem.SetColor(gemColor);
                    gem.SetPosition(position_gem);
                    cast.AddActor("gems", gem);
                }
            }

        }
    }
}