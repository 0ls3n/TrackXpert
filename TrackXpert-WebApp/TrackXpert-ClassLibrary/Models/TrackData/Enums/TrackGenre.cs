using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackXpert_ClassLibrary.Models.TrackData.Enums
{
    public enum TrackGenre
    {
        // Popular Genres
        Pop,
        Rock,
        Metal,

        [Display(Name = "Hip-Hop")]
        HipHop,

        Rap,
        Jazz,
        Blues,
        Country,
        Classical,
        Electronic,
        Reggae,
        Funk,
        Soul,
        RnB,

        // Subgenres of Rock and Metal
        Alternative,
        Indie,

        [Display(Name = "Hard Rock")]
        HardRock,

        Punk,
        Grunge,

        [Display(Name = "Progressive Rock")]
        ProgressiveRock,

        [Display(Name = "Thrash Metal")]
        ThrashMetal,

        [Display(Name = "Death Metal")]
        DeathMetal,

        [Display(Name = "Black Metal")]
        BlackMetal,

        [Display(Name = "Doom Metal")]
        DoomMetal,

        [Display(Name = "Symphonic Metal")]
        SymphonicMetal,

        [Display(Name = "Power Metal")]
        PowerMetal,

        // Electronic Subgenres
        House,
        Techno,
        Trance,
        Dubstep,

        [Display(Name = "Drum and Bass")]
        DrumAndBass,

        LoFi,

        // World Music
        Latin,
        KPop,
        Afrobeat,
        Bollywood,
        Reggaeton,

        // Other Genres
        Folk,
        Gospel,
        Ska,
        Ambient,
        Experimental,
        Acoustic,
        Opera
    }
}
