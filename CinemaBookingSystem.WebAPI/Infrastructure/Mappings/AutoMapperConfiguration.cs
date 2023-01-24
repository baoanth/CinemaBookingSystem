using AutoMapper;
using CinemaBookingSystem.Model.Models;
using CinemaBookingSystem.ViewModels;

namespace CinemaBookingSystem.WebAPI.Infrastructure.Mappings
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<BookingDetail, BookingDetailViewModel>().ReverseMap();
            CreateMap<Booking, BookingViewModel>().ReverseMap();
            CreateMap<Slide, SlideViewModel>().ReverseMap();
            CreateMap<Cinema, CinemaViewModel>().ReverseMap();
            CreateMap<Comment, CommentViewModel>().ReverseMap();
            CreateMap<Error, ErrorViewModel>().ReverseMap();
            CreateMap<User, UserViewModel>().ReverseMap();
            CreateMap<Movie, MovieViewModel>().ReverseMap();
            CreateMap<Role, RoleViewModel>().ReverseMap();
            CreateMap<Screening, ScreeningViewModel>().ReverseMap();
            CreateMap<ScreeningPosition, ScreeningPositionViewModel>().ReverseMap();
            CreateMap<Contact, ContactViewModel>().ReverseMap();
            CreateMap<Theatre, TheatreViewModel>().ReverseMap();
            CreateMap<VisitorStatistic, VisitorStatisticViewModel>().ReverseMap();
            CreateMap<User, SignupViewModel>().ReverseMap();
        }
    }
}