using AutoMapper;
using API.Shared.Dtos;
using API.Entities;

public class MessageProfile : Profile
{
    public MessageProfile()
    {
        CreateMap<MessageDto, Message>();
        CreateMap<Message, MessageDto>();
        CreateMap<Message, MessageDetailsDto>();
    }
}
