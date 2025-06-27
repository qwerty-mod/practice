using System;

namespace task04
{
    public class Fighter : ISpaceship
    {
        public int Speed => 100;
        public int FirePower => 50;

        public void MoveForward()
        {
            Console.WriteLine("Истребитель быстро движется вперёд.");
        }

        public void Rotate(int angle)
        {
            Console.WriteLine($"Истребитель поворачивается на {angle} градусов.");
        }

        public void Fire()
        {
            Console.WriteLine($"Истребитель стреляет ракетой с силой {FirePower}.");
        }
    }
}