using AutoMapper;
using IssueTracker.Application.DTOs;
using IssueTracker.Domain.Entities;

namespace IssueTracker.Application.Mappings;

public class IssueMappingProfile : Profile
{
    public IssueMappingProfile()
    {
        CreateMap<Issue, IssueDto>()
            .ForMember(dest => dest.Assignee,
                opt => opt.MapFrom(src => src.Assignee))
            .ForMember(dest => dest.Attachments,
                opt => opt.MapFrom(src => src.Attachments));

        CreateMap<CreateIssueDto, Issue>()
            .ForMember(dest => dest.Id,
                opt => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(dest => dest.CreatedAt,
                opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.Status,
                opt => opt.MapFrom(_ => Domain.Enums.IssueStatus.Open));

        CreateMap<User, UserDto>();

        CreateMap<Attachment, AttachmentDto>()
            .ForMember(dest => dest.FileUrl,
                opt => opt.MapFrom(src =>
                    $"/uploads/{src.IssueId}/{Uri.EscapeDataString(Path.GetFileName(src.FilePath))}"));
    }
}
