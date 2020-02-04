using System;
using System.Collections.Generic;
using System.Linq;
using Core.Domain.Model;
using Ninject;

namespace UnitOfWork
{
    class Program
    {
        public enum OptionsEnum
        {
            Quit,
            Basic,
            SettingsService,
            ArchiveService,
            Continue
        }

        static void Main(string[] args)
        {
            //  Default the method of instantiation of the service(s)
            var useIoC = false;
            //  Check if creation using the Ninject IoC has been set in the parameters
            if (args.Any())
                useIoC = args.Contains("-ioc", StringComparer.CurrentCultureIgnoreCase);

            //  Instantiate the IoC
            var ioc = new StandardKernel(new IoC());

            //  Instantiate the test ArchiveServiceFactory, then use it to instantiate the ArchiveService;
            var archiveServiceFactory = new ArchiveServiceFactory(ioc);
            var archiveService = archiveServiceFactory.Create(useIoC);
            
            //  Instantiate the MemberProfileService
            var memberServiceFactory = new MemberServiceFactory(ioc);
            var memberService = memberServiceFactory.Create(useIoC);

            //  Instantiate the SettingsService
            var settingsServiceFactory = new SettingsServiceFactory(ioc);
            var settingService = settingsServiceFactory.Create(useIoC);

            //  Instantiate the basic transaction service
            var basicService = new BasicTransaction();


            //  Process Menu, keep returning to menu until 'quit' selected
            var quitApp = false;
            while (!quitApp)
            {
                switch (GetOption())
                {
                    case OptionsEnum.Quit:
                        quitApp = true;
                        break;
                    case OptionsEnum.Basic:
                        var resultBasic =  basicService.Execute();
                        DisplayResult("Basic Transaction", resultBasic);
                        PressEnterToContinue();
                        break;
                    case OptionsEnum.SettingsService:
                        IEnumerable<Settings> settingsCollection = new List<Settings>()
                        {
                            new Settings()
                            {
                                Id=0,
                                EmailAccount = "tim@dev.com",
                                SlowResponseTime = 1000,
                                ScanFrequency = FrequencyEnum.Daily,
                                ArchiveRunDetails = new List<ArchiveDetail>()
                            },
                            new Settings()
                            {
                                Id=2,
                                EmailAccount = "tim@dev.com",
                                SlowResponseTime = 1000,
                                ScanFrequency = FrequencyEnum.Daily,
                                ArchiveRunDetails = null
                            }
                        };
                        var resultSettings = settingService.AddMultipleSettings(settingsCollection);
                        DisplayResult("Settings Service", resultSettings);
                        PressEnterToContinue();
                        break;
                    case OptionsEnum.ArchiveService:
                        var archiveDate = new DateTime(2019,9,3);
                        var memberProfile = memberService.Get(1);
                        //  Call the service to check it's functioning
                        var result = archiveService.Archive(archiveDate, memberProfile);
                        DisplayResult("Archive", result);
                        PressEnterToContinue();
                        break;
                    default:
                        break;
                }
            }
        }


        public static OptionsEnum GetOption()
        {
            Console.Clear();
            Console.WriteLine(App.ProgramTitle);
            Console.WriteLine(App.ProgramUnderline);
            Console.WriteLine("");
            Console.WriteLine(App.Menu_Select_option + "\r\n");
            Console.WriteLine("\t" + App.Menu_Option_1);
            Console.WriteLine("\t" + App.Menu_Option_2);
            Console.WriteLine("\t" + App.Menu_Option_3);
            Console.WriteLine("\t" + App.Enter_Q_to_quit);

            var keypressed = Console.ReadKey(false).KeyChar.ToString().ToLower();

            Console.WriteLine("");

            switch (keypressed)
            {
                case "q":
                    return OptionsEnum.Quit;
                case "1":
                    return OptionsEnum.Basic;
                case "2":
                    return OptionsEnum.SettingsService;
                case "3":
                    return OptionsEnum.ArchiveService;
                default:
                    return OptionsEnum.Continue;
            }
        }

        public static void PressEnterToContinue()
        {
            Console.WriteLine();
            Console.WriteLine(App.EnterToTerminate);
            Console.ReadLine();
        }

        public static void DisplayResult(string service, object result )
        {
            Console.WriteLine("Result:");
            Console.WriteLine(App.CallReturnCode, service , result);
        }

    }
}
