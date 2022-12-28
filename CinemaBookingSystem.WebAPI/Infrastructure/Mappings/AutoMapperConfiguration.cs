using AutoMapper;
using CinemaBookingSystem.ClientWeb.Models;
using CinemaBookingSystem.Model.Models;

namespace CinemaBookingSystem.WebAPI.Infrastructure.Mappings
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<Province, ProvinceViewModel>().ReverseMap();
            CreateMap<BookingDetail, BookingDetailViewModel>().ReverseMap();
            CreateMap<Booking, BookingViewModel>().ReverseMap();
            CreateMap<Carousel, CarouselViewModel>().ReverseMap();
            CreateMap<Cinema, CinemaViewModel>().ReverseMap();
            CreateMap<Comment, CommentViewModel>().ReverseMap();
            CreateMap<Error, ErrorViewModel>().ReverseMap();
            CreateMap<User, UserViewModel>().ReverseMap();
            CreateMap<Movie, MovieViewModel>().ReverseMap();
            CreateMap<Role, RoleViewModel>().ReverseMap();
            CreateMap<Screening, ScreeningViewModel>().ReverseMap();
            CreateMap<ScreeningPosition, ScreeningPositionViewModel>().ReverseMap();
            CreateMap<SupportOnline, SupportOnlineViewModel>().ReverseMap();
            CreateMap<Theatre, TheatreViewModel>().ReverseMap();
            CreateMap<VisitorStatistic, VisitorStatisticViewModel>().ReverseMap();
        }
    }
}
