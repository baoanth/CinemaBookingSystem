﻿namespace CinemaBookingSystem.WebAPI.ViewModels
{
    public class CinemaViewModel
    {
        public int CinemaID { get; set; }
        public string CinemaName { get; set; }
        public string Location { get; set; }
        public string FAX { get; set; }
        public string Hotline { get; set; }
        public string ProvinceID { get; set; }
        public virtual LocationViewModel Province { get; set; }
        public IEnumerable<TheatreViewModel> Theatres { get; set; }
    }
}