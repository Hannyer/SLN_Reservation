using Repository.IRepository;
using Repository.Repository;
using Service.IService;
using Service.Service;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace SLN_Reservation
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {

            var container = new UnityContainer();
            //repositorios
            container.RegisterType<IClientRepository, ClientRepository>();
            container.RegisterType<IConfigurationRepository, ConfigurationRepository>();
            container.RegisterType<IDailyJobsRepository, DailyJobsRepository>();
            container.RegisterType<IMenuRepository, MenuRepository>();
            container.RegisterType<IPermissionRepository, PermissionRepository>();
            container.RegisterType<IReservationRepository, ReservationRepository>();
            container.RegisterType<IRoleRepository, RoleRepository>();
            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<IRateRepository, RateRepository>();
            container.RegisterType<IRateTypeRepository, RateTypeRepository>();
            container.RegisterType<IIdentificationTypeRepository, IdentificationTypeRepository>();
            container.RegisterType<IReservationReportRepository, ReservationReportRepository>();
            container.RegisterType<IAboutRepository, AboutRepository>();
            container.RegisterType<IHotelRoomRepository, HotelRoomRepository>();


            //services
            container.RegisterType<IClientService, ClientService>();
            container.RegisterType<IConfigurationService, ConfigurationService>();
            container.RegisterType<IDailyJobsService, DailyJobsService>();
            container.RegisterType<IMenuService, MenuService>();
            container.RegisterType<IPermissionService, PermissionService>();
            container.RegisterType<IReservationService, ReservationService>();
            container.RegisterType<IRoleService, RoleService>();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IRateService, RateService>();
            container.RegisterType<IRateTypeService, RateTypeService>();
            container.RegisterType<IIdentificationTypeService, IdentificationTypeService>();
            container.RegisterType<IReservationReportService, ReservationReportService>();
            container.RegisterType<IAboutService, AboutService>();
            container.RegisterType<IHotelRoomService, HotelRoomService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}