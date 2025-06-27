using System;

namespace task04
{
    public class Cruiser : ISpaceship
    {
        public int Speed => 50;
        public int FirePower => 100;

        public void MoveForward()
        {
            Console.WriteLine("Крейсер медленно движется вперёд.");
        }

        public void Rotate(int angle)
        {
            Console.WriteLine($"Крейсер поворачивается на {angle} градусов.");
        }

        public void Fire()
        {
            Console.WriteLine($"Крейсер стреляет ракетой с силой {FirePower}.");
        }
    }
}

