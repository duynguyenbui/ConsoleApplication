using System.Collections.Concurrent;

var i = 0;
const int capacity = 10;
var wareHouse = new ConcurrentQueue<int>();
var cancellationTokenSource = new CancellationTokenSource();
var cancellationToken = cancellationTokenSource.Token;

var produce = new Task(() =>
{
    var value = 1;
    while (true)
    {
        if (cancellationToken.IsCancellationRequested) break;

        if (wareHouse.Count < capacity)
        {
            wareHouse.Enqueue(value++);
            Console.WriteLine("Sản xuất sản phẩm #" + (value - 1));
        }
        else
        {
            Console.WriteLine("Kho đầy - ngưng sản xuất");
        }

        Thread.Sleep(1000);
    }
});

var consume = new Task(() =>
{
    while (true)
    {
        if (cancellationToken.IsCancellationRequested) break;

        int product;
        if (wareHouse.TryDequeue(out product))
            Console.WriteLine("Tiêu thụ sản phẩm #" + product);
        else
            Console.WriteLine("Kho hết hàng, tạm dừng tiêu thụ");

        Thread.Sleep(3000);
    }
});

produce.Start();
consume.Start();

while (true)
{
    var c = Console.ReadKey().KeyChar;
    cancellationTokenSource.Cancel();
    break;
}