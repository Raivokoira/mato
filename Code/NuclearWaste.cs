using Godot;
using System;

namespace SnakeGame
{
    public partial class NuclearWaste : Collectable
    {


        public override void Collect(Snake snake)
        {

            snake.QueueFree();

            // Replacing the NucWastes with a new one.
            Level.Current.ReplaceNuclearWaste();
        }
    }
}