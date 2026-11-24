namespace CP40
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Исследование работы GC\n");

            Console.WriteLine("Начальные показатели:");

            int gen0Start = GC.CollectionCount(0);
            int gen1Start = GC.CollectionCount(1);
            int gen2Start = GC.CollectionCount(2);

            long heapStart = GC.GetGCMemoryInfo().HeapSizeBytes;
            long allocatedStart = GC.GetTotalAllocatedBytes();

            ShowCounters(gen0Start, gen1Start, gen2Start);

            Console.WriteLine($"Размер кучи: {heapStart:N0} байт");

            Console.WriteLine("\nЭтап 1. Мелкие объекты");

            long allocatedBeforeSmall = GC.GetTotalAllocatedBytes();

            for (int i = 0; i < 200_000; i++)
            {
                _ = new byte[50];
            }

            long allocatedAfterSmall = GC.GetTotalAllocatedBytes();

            int gen0After = GC.CollectionCount(0);
            int gen1After = GC.CollectionCount(1);
            int gen2After = GC.CollectionCount(2);

            long heapAfterSmall = GC.GetGCMemoryInfo().HeapSizeBytes;

            Console.WriteLine("После создания 200 000 массивов:");

            ShowCounters(gen0After, gen1After, gen2After);

            Console.WriteLine($"Новых сборок Gen0: {gen0After - gen0Start}");
            Console.WriteLine($"Новых сборок Gen1: {gen1After - gen1Start}");
            Console.WriteLine($"Новых сборок Gen2: {gen2After - gen2Start}");

            Console.WriteLine($"Выделено памяти на мелкие объекты: {allocatedAfterSmall - allocatedBeforeSmall:N0} байт");
            Console.WriteLine($"Размер кучи: {heapAfterSmall:N0} байт");

            Console.WriteLine("\nЭтап 2. Большие объекты");

            int gen0BeforeLarge = GC.CollectionCount(0);
            int gen1BeforeLarge = GC.CollectionCount(1);
            int gen2BeforeLarge = GC.CollectionCount(2);

            long heapBeforeLarge = GC.GetGCMemoryInfo().HeapSizeBytes;
            long allocatedBeforeLarge = GC.GetTotalAllocatedBytes();

            byte[][] largeObjects = new byte[5][];

            for (int i = 0; i < 5; i++)
            {
                largeObjects[i] = new byte[90_000];
            }

            GC.Collect(2, GCCollectionMode.Forced, true, false);

            long allocatedAfterLarge = GC.GetTotalAllocatedBytes();

            int gen0End = GC.CollectionCount(0);
            int gen1End = GC.CollectionCount(1);
            int gen2End = GC.CollectionCount(2);

            long heapEnd = GC.GetGCMemoryInfo().HeapSizeBytes;

            Console.WriteLine("После создания 5 массивов по 90 000 байт:");

            ShowCounters(gen0End, gen1End, gen2End);

            Console.WriteLine($"Новых сборок Gen0: {gen0End - gen0BeforeLarge}");
            Console.WriteLine($"Новых сборок Gen1: {gen1End - gen1BeforeLarge}");
            Console.WriteLine($"Новых сборок Gen2: {gen2End - gen2BeforeLarge}");

            Console.WriteLine($"Выделено памяти на большие объекты: {allocatedAfterLarge - allocatedBeforeLarge:N0} байт");
            Console.WriteLine($"Размер одного массива: 90 000 байт");
            Console.WriteLine($"Количество больших массивов: {largeObjects.Length}");

            Console.WriteLine($"Размер кучи до: {heapBeforeLarge:N0} байт");
            Console.WriteLine($"Размер кучи после: {heapEnd:N0} байт");

            Console.WriteLine("\nИтог эксперимента");

            Console.WriteLine($"Куча в начале: {heapStart:N0} байт");
            Console.WriteLine($"Куча в конце: {heapEnd:N0} байт");

            Console.WriteLine($"Всего выделено памяти: {GC.GetTotalAllocatedBytes() - allocatedStart:N0} байт");

            Console.WriteLine("\nЭксперимент завершен.");

            Console.ReadKey();
        }

        static void ShowCounters(int gen0, int gen1, int gen2)
        {
            Console.WriteLine($"Gen0: {gen0}");
            Console.WriteLine($"Gen1: {gen1}");
            Console.WriteLine($"Gen2: {gen2}");
        }
    }
}
