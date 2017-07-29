﻿using MagicMirror.Business.Models;
using MagicMirror.DataAccess.Entities;
using System.Threading.Tasks;

namespace MagicMirror.Business.Services
{
    public interface IService<T> where T : Model
    {
        Task<T> GetModelAsync();

        T MapEntityToModel(Entity entity);

        T CalculateMappedValues(T model);
    }
}