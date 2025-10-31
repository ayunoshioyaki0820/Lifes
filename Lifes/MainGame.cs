using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifes
{
    public enum TutorialSteps
    {
        None,
        Move,
        Edit,
        Done
    }
    public class MainGame
    {
        private List<Creature> creatures = new List<Creature>();
        private double evolveTimer = 0;
        private GraphicsDevice graphicsDevice;

        public MainGame(GraphicsDevice device)
        {
            graphicsDevice = device;
            for (int i = 0; i < 10; i++)
                creatures.Add(new Creature(20, graphicsDevice));
        }

        public void Update(GameTime gameTime)
        {
            foreach (var c in creatures)
                c.Update();

            evolveTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (evolveTimer > 5)
            {
                var newGen = new List<Creature>();
                foreach (var c in creatures)
                    newGen.Add(new Creature(creatures[0], creatures[1], graphicsDevice));
                creatures = newGen;
                evolveTimer = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var c in creatures)
                c.Draw(spriteBatch);
        }
    }

}
