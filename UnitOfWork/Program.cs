using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Core.Domain.Model;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Ninject;
using Ninject.Infrastructure.Language;

namespace UnitOfWork
{
    class Program
    {
        public enum optionsEnum
        {
            Quit,
            Basic,
            SettingsService,
            ArchiveService,
            Continue
        }

        static void Main(string[] args)
        {
            //  Default the method of instantiation of the service
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
                    case optionsEnum.Quit:
                        quitApp = true;
                        break;
                    case optionsEnum.Basic:
                        var resultBasic =  basicService.Execute();
                        DisplayResult("Basic Transaction", resultBasic);
                        PressEnterToContinue();
                        break;
                    case optionsEnum.SettingsService:
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
                    case optionsEnum.ArchiveService:
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


        public static optionsEnum GetOption()
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

            switch (keypressed)
            {
                case "q":
                    return optionsEnum.Quit;
                case "1":
                    return optionsEnum.Basic;
                case "2":
                    return optionsEnum.SettingsService;
                case "3":
                    return optionsEnum.ArchiveService;
                default:
                    return optionsEnum.Continue;
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
            Console.WriteLine(App.CallReturnCode,service , result);
        }


        //public static void basicTxn()
        //{
        //    var context = new SiteMonitorDbDataContext();

        //    var settings1 = new Settings()
        //    {
        //        Id=0,
        //        EmailAccount = "tim@dev.com",
        //        SlowResponseTime = 1000,
        //        ScanFrequency = FrequencyEnum.Daily,
        //        ArchiveRunDetails = new List<ArchiveDetail>()
        //    };

        //    var settings2 = new Settings()
        //    {
        //        Id=2,
        //        EmailAccount = "tim@dev.com",
        //        SlowResponseTime = 1000,
        //        ScanFrequency = FrequencyEnum.Daily,
        //        ArchiveRunDetails = null
        //    };

        //    using (var txn = context.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            //  Insert settings1 record
        //            context.MonitorSettings.Add(settings1);

        //            var res1 =  context.SaveChanges();
        //            var success = (res1 > 0);


        //            if (success)
        //            {
        //                //  Insert settings2 record  if first was ok
        //                context.MonitorSettings.Add(settings2);
        //                var res2 = context.SaveChanges();
        //                success = false; //res2 > 0;
        //            }

        //            if (success)
        //            {
        //                txn.Commit();
        //            }
        //            else
        //            {
        //                txn.Rollback();
        //            }
        //        }
        //        catch
        //        {
        //            txn.Rollback();
        //        }

        //    }

        //}
    }
}
