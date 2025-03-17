using System;

namespace Rise.Shared.Movies;

public class CinemaDto
{
    public  int id {  get; set; }
    public string Name { get; set; }
    public List<DateTime> Showtimes { get; set; }
}