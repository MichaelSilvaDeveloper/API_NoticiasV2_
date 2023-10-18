using Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.InterfacesServices
{
    public interface IServiceMessage
    {
        Task Adicionar(Message objectMessage);

        Task Atualizar(Message objectMessage);

        Task<List<Message>> ListarMessagesAtivas();
    }   
}