using Domain.Interfaces;
using Domain.Interfaces.InterfacesServices;
using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class ServiceMessage : IServiceMessage
    {
        private readonly IMessage _iMessage;

        public ServiceMessage(IMessage iMessage)
        {
            _iMessage = iMessage;
        }

        public async Task Adicionar(Message objectMessage)
        {
            var validaTitulo = objectMessage.ValidarPropriedadeString(objectMessage.Titulo, "Titulo");
            if (validaTitulo)
            {
                objectMessage.DataAlteracao = DateTime.Now;
                objectMessage.Ativo = true;
                await _iMessage.Add(objectMessage);
            }
        }

        public async Task Atualizar(Message objectMessage)
        {
            var validaTitulo = objectMessage.ValidarPropriedadeString(objectMessage.Titulo, "Titulo");
            if (validaTitulo)
            {
                objectMessage.DataAlteracao = DateTime.Now;
                objectMessage.Ativo = true;
                await _iMessage.Update(objectMessage);
            }
        }

        public async Task<List<Message>> ListarMessagesAtivas()
        {
            return await _iMessage.ListarMessage(x => x.Ativo);  
        }
    }
}