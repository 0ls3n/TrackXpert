using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackXpert_ClassLibrary.Models.TrackData.Enums
{
	public enum TrackKey
	{
		// Major Keys
		C,

		[Display(Name = "C#")]
		CSharp,

		Db,
		D,

		[Display(Name = "D#")]
		DSharp,

		Eb,
		E,
		F,

		[Display(Name = "F#")]
		FSharp,

		Gb,
		G,

		[Display(Name = "G#")]
		GSharp,

		Ab,
		A,

		[Display(Name = "A#")]
		ASharp,

		Bb,
		B,

		// Minor Keys
		Cm,

		[Display(Name = "C#m")]
		CSharpM,

		Dbm,
		Dm,

		[Display(Name = "D#m")]
		DSharpM,

		Ebm,
		Em,
		Fm,

		[Display(Name = "F#m")]
		FSharpM,

		Gbm,
		Gm,

		[Display(Name = "G#m")]
		GSharpM,

		Abm,
		Am,

		[Display(Name = "A#m")]
		ASharpM,

		Bbm,
		Bm
	}
}
