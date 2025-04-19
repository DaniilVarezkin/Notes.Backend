using AutoMapper;
using Notes.Application.Common.Mapping;
using Notes.Application.Notes.Commands.CreateNote;
using System.ComponentModel.DataAnnotations;

namespace Notes.WebApi.Models
{
    public class CreateNoteDto : IMapWith<CreateNoteCommand>
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateNoteDto, CreateNoteCommand>()
                .ForMember(dest => dest.Title, opt => { 
                    opt.MapFrom(src => src.Title);
                })
                .ForMember(dest => dest.Details, opt => {
                    opt.MapFrom(src => src.Details);
                });
        }
    }
}
