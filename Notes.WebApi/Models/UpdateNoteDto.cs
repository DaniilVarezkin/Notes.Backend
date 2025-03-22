using AutoMapper;
using Notes.Application.Common.Mapping;
using Notes.Application.Notes.Commands.UpdateNote;

namespace Notes.WebApi.Models
{
    public class UpdateNoteDto : IMapWith<UpdateNoteCommand>
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateNoteDto, UpdateNoteCommand>()
                .ForMember(dest => dest.Id, 
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Details,
                    opt => opt.MapFrom(src => src.Details));
        }
    }
}
