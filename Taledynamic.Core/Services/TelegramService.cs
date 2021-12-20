using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Taledynamic.Core.Exceptions;
using Taledynamic.Core.Interfaces;
using Taledynamic.DAL.Entities;
using Taledynamic.DAL.Models.DTOs;
using Taledynamic.DAL.Models.Internal;
using Taledynamic.DAL.Models.Requests.TableRequests;
using Taledynamic.DAL.Models.Responses;
using Taledynamic.DAL.MongoModels;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Taledynamic.Core.Services
{
    public partial class TelegramService: BaseService<TelegramUser>, ITelegramService 
    {
        private TaledynamicContext _context { get; }
        private IMongoCollection<JsonModel> _documents { get; }
        public TelegramService(TaledynamicContext context, IMongoDbSettings settings) : base(context)
        {
            _context = context;
            
            var client = new MongoClient(settings.ConnectionString);
            
            var database = client.GetDatabase(settings.DatabaseName);
            _documents = database.GetCollection<JsonModel>(settings.JsonCollectionName);
        }
    }
}