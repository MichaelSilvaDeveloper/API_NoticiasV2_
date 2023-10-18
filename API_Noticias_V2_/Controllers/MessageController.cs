using API_Noticias_V2_.Models;
using AutoMapper;
using Domain.Interfaces;
using Domain.Interfaces.InterfacesServices;
using Entities.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API_Noticias_V2_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessage _iMessage;

        private readonly IMapper _iMapper;

        private readonly IServiceMessage _iServiceMessage;

        public MessageController(IMessage iMessage, IMapper iMapper, IServiceMessage iServiceMessage)
        {
            _iMessage = iMessage;
            _iMapper = iMapper;
            _iServiceMessage = iServiceMessage;
        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("/api/AddMessage")]
        public async Task<List<Notifies>> AddMessage(MessageViewModel message)
        {
            message.UserId = await RetornarIdUsuarioLogado();
            var messageMap = _iMapper.Map<Message>(message);
            await _iServiceMessage.Adicionar(messageMap);
            //await _iMessage.Add(messageMap);
            return messageMap.Notificacoes;
        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("/api/UpdateMessage")]
        public async Task<List<Notifies>> UpdateMessage(MessageViewModel message)
        {
            var messageMap = _iMapper.Map<Message>(message);
            await _iServiceMessage.Atualizar(messageMap);
            //await _iMessage.Update(messageMap);
            return messageMap.Notificacoes;
        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("/api/GetMessageById")]
        public async Task<MessageViewModel> GetMessageById(Message message)
        {
            message = await _iMessage.GetEntityById(message.Id);
            var messageMap = _iMapper.Map<MessageViewModel>(message);
            return messageMap;
        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("/api/ListMessage")]
        public async Task<List<MessageViewModel>> ListMessage()
        {
            var mensagens = await _iMessage.List();
            var messageMap = _iMapper.Map<List<MessageViewModel>>(mensagens);
            return messageMap;
        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("/api/ListMessagesActives")]
        public async Task<List<MessageViewModel>> ListMessagesActives()
        {
            var mensagens = await _iServiceMessage.ListarMessagesAtivas();
            var messageMap = _iMapper.Map<List<MessageViewModel>>(mensagens);
            return messageMap;
        }

        private async Task<string> RetornarIdUsuarioLogado()
        {
            if(User != null)
            {
                var idUsuario = User.FindFirst("idUsuario");
                return idUsuario.Value;
            }
            return string.Empty;
        }
    }
}
