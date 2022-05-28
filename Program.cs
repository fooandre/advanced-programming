void Run()
{
    Console.Clear();
    MenuService menu = new();
    // menu.Run();
    while (true) menu.Display();
}

Run();