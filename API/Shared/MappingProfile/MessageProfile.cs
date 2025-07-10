using AutoMapper;
using API.Shared.Dtos;
using API.Entities;

public class MessageProfile : Profile
{
    public MessageProfile()
    {
        CreateMap<MessageDto, Message>();
        CreateMap<Message, MessageDto>()
            .ForMember(dest => dest.ReceiverId, opt => opt.MapFrom(src => src.Chat.ReceiverId));
        CreateMap<Message, MessageDetailsDto>();
    }
}
