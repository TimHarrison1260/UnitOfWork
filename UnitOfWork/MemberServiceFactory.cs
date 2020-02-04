using System;
using Core.Domain.Mappers;
using Core.Domain.Model;
using Core.Domain.Services;
using Core.Interfaces.Mappers;
using Core.Interfaces.Services;
using Infrastructure.Data;
using Infrastructure.Factories;
using Infrastructure.Interfaces.Factories;
using Infrastructure.Services;
using Ninject;

namespace UnitOfWork
{
    /// <summary>
    /// Class <c>MemberServiceFactory</c> is responsible for instantiating
    /// the MemberService in the test application
    /// </summary>
    public class MemberServiceFactory
    {
        private readonly StandardKernel _kernel;

        /// <summary>
        /// ctor: accepts the IoC Kernel instance => Ninject
        /// </summary>
        /// <param name="kernel">Ninject Kernel instance</param>
        public MemberServiceFactory(StandardKernel kernel)
        {
            _kernel = kernel ?? throw new ArgumentNullException(nameof(kernel));
        }

        /// <summary>
        /// Create instance of MemberService
        /// </summary>
        /// <returns>Instance of MemberService</returns>
        public IMemberService Create()
        {
            //  Infrastructure;
            // :EF DbContext
            var context = new SiteMonitorDbDataContext();
            //  Infrastructure: RepositoryFactory
            IRepositoryFactory<SiteMonitorDbDataContext> repositoryFactory = new SiteMonitorRepositoryFactory(context);

            //  Unit of Work
            IMemberService memberService = new MemberService(context, repositoryFactory);

            return memberService; 
        }

        /// <summary>
        /// Create instance of MemberService from IoC
        /// </summary>
        /// <param name="useIoc">True create from Ioc, otherwise create</param>
        /// <returns>Instance of MemberService</returns>
        public IMemberService Create(bool useIoc)
        {
            return !useIoc ? Create() : _kernel.Get<IMemberService>();
        }
    }
        
}