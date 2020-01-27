using System;
using System.Linq;
using Ninject;

namespace UnitOfWork
{
    class Program
    {
        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            Console.WriteLine(App.EnterToStart);
            Console.ReadLine();

            //  Default the method of instantiation of the service
            var useIoC = false;
            //  Check if creation using the Ninject IoC has been set in the parameters
            if (args.Any())
                useIoC = args.Contains("-ioc", StringComparer.CurrentCultureIgnoreCase);

            //  Instantiate the test service factory, then instantiate the service itself using the factory
            var serviceFactory = new ServiceFactory(new StandardKernel(new IoC()));
            var service = serviceFactory.Create(useIoC);

            //  Call the service to check it's functioning
            var result = service.Archive(DateTime.Today);

            //  Display result and terminate the program
            Console.WriteLine();
            Console.WriteLine(App.CallToGetAllReturned, result);
            Console.WriteLine();
            Console.WriteLine(App.EnterToTerminate);
            Console.ReadLine();


        }
    }
}
