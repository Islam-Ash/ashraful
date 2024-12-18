using ConsoleApplication;

Console.WriteLine("||---------------------------------------||");
Console.WriteLine("||  Welcome to the School Application!   ||");
Console.WriteLine("||---------------------------------------||\n");

while (true)
{
    Console.WriteLine("Please Login");
    Console.Write("Username: ");
    string username = Console.ReadLine();

    Console.Write("Password: ");
    string password = Console.ReadLine();

    using (var db = new AppDbContext())
    {
        var user = db.Users.FirstOrDefault(u => u.UserName == username && u.Password == password);

        if (user != null)
        {
            if (user.Role == UserRole.Admin)
            {
                if (Admin.AdminLogin(username, password))
                    break;
            }
            else if (user.Role == UserRole.Teacher)
            {
                if (Teacher.TeacherLogin(username, password))
                    break;
            }
        }
        else
        {
            Console.WriteLine("|------------------------------------------------|");
            Console.WriteLine("| Invalid Username or Password. Please try again.|");
            Console.WriteLine("|------------------------------------------------|\n");
        }
    }
}
Console.WriteLine("||--------------------------------------||");
Console.WriteLine("||       Exiting the application        ||");
Console.WriteLine("|| To Login, Run The Application Again. ||");
Console.WriteLine("||             Thank You.               ||");
Console.WriteLine("||--------------------------------------||");
Environment.Exit(0);
