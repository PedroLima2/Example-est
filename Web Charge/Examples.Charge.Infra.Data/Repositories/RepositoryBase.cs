using Examples.Charge.Domain.Base.Interfaces;
using Examples.Charge.Infra.Data.Base;
using Examples.Charge.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Examples.Charge.Infra.Data.Repositories
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        private readonly ExampleContext _context;

        public RepositoryBase(ExampleContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public virtual TEntity Add(TEntity Entity)
        {
            //Preserva Valores Originais
            var EntidadeOriginal = CloneAll(Entity);

            //Limpa o Objeto Somente com os campos Para Atualizar
            TEntity EntidadeLocal = Activator.CreateInstance<TEntity>();
            CloneEntidadeCamposInsert(Entity, EntidadeLocal);

            //Adiciona a Entidade ao Contexto para ser INSERIDA
            _context.Set<TEntity>().Add(EntidadeLocal);

            //Salva a Informação no Banco de dados
            _context.SaveChanges();

            //Pegar valores da Chave Primaria
            CopyToKey(EntidadeLocal, Entity);

            //Copia Todos os valores, exceto chave primaria
            CloneEntidadePropertysNotInsert(EntidadeOriginal, Entity);

            return Entity;
        }

        public virtual TEntity AddOrUpdate(TEntity Entidade)
        {
            //Preserva Valores Originais
            var EntidadeOriginal = CloneAll(Entidade);

            //Limpa o Objeto Somente com os campos Para Atualizar
            TEntity EntidadeLocal = Activator.CreateInstance<TEntity>();
            CloneEntidadeCamposInsert(Entidade, EntidadeLocal);

            //Adiciona a Entidade ao Contexto para ser INSERIDA
            _context.Set<TEntity>().AddOrUpdate(EntidadeLocal);

            //Salva a Informação no Banco de dados
            _context.SaveChanges();

            //Pegar valores da Chave Primaria
            CopyToKey(EntidadeLocal, Entidade);

            //Copia Todos os valores, exceto chave primaria
            CloneEntidadePropertysNotInsert(EntidadeOriginal, Entidade);

            return Entidade;
        }

        public virtual async Task<IEnumerable<TEntity>> FindAllAsync() => await Task.Run(() => GetQuery());

        public virtual IEnumerable<TEntity> GetAll()
        {
            return GetQuery().ToList();
        }

        public virtual TEntity GetById(int id)
        {
            //return GetQuery().FirstOrDefault("",id);
            return _context.Set<TEntity>().Find(id);
        }

        public virtual IQueryable<TEntity> GetQuery()
        {
            return _context.Set<TEntity>();
        }

        public virtual void Remove(TEntity Entity)
        {
            try
            {
                _context.Set<TEntity>().Remove(Entity);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual TEntity Update(TEntity Entity)
        {
            //Limpa o Objeto Somente com os campos Para Atualizar
            TEntity EntidadeLocal = Activator.CreateInstance<TEntity>();

            //Preserva Valores Originais
            var EntidadeOriginal = CloneAll(Entity);

            CloneEntidadeCamposInsert(Entity, EntidadeLocal);

            _context.Set<TEntity>().Update(EntidadeLocal);

            //Salva a Informação no Banco de dados
            _context.SaveChanges();

            //Copia Todos os valores, exceto chave primaria
            CloneEntidadePropertysNotInsert(EntidadeOriginal, Entity);

            return Entity;
        }


        public virtual void Dispose()
        {
            _context.Dispose();
        }

        #region Metódos
        protected virtual TEntityt CloneAll<TEntityt>(TEntityt objetc) where TEntityt : class
        {
            var entidade = Activator.CreateInstance(typeof(TEntityt));

            foreach (var PropertyInfo in objetc.GetType().GetProperties())
            {
                //if(PropertyInfo.GetValue(objetc).GetType() != )
                entidade.GetType().GetProperty(PropertyInfo.Name).SetValue(entidade, PropertyInfo.GetValue(objetc));
            }

            return entidade as TEntityt;
        }

        protected virtual TEntity CloneEntidadeCamposInsert<TEntity>(TEntity objetcOrigem, TEntity EntidadeDetino) where TEntity : class
        {
            try
            {
                foreach (var PropertyInfo in objetcOrigem.GetType().GetProperties())
                {
                    if (PropertyInfo.PropertyType == typeof(string) | PropertyInfo.PropertyType == typeof(String))
                    {
                        EntidadeDetino.GetType().GetProperty(PropertyInfo.Name).SetValue(EntidadeDetino, PropertyInfo.GetValue(objetcOrigem));
                    }
                    else if (PropertyInfo.PropertyType == typeof(long) | PropertyInfo.PropertyType == typeof(long?))
                    {
                        EntidadeDetino.GetType().GetProperty(PropertyInfo.Name).SetValue(EntidadeDetino, PropertyInfo.GetValue(objetcOrigem));
                    }
                    else if (PropertyInfo.PropertyType == typeof(int) | PropertyInfo.PropertyType == typeof(int?) |
                    PropertyInfo.PropertyType == typeof(Int16) | PropertyInfo.PropertyType == typeof(Int16?) |
                    PropertyInfo.PropertyType == typeof(Int32) | PropertyInfo.PropertyType == typeof(Int32?) |
                    PropertyInfo.PropertyType == typeof(Int64) | PropertyInfo.PropertyType == typeof(Int64?) |
                    PropertyInfo.PropertyType == typeof(float) | PropertyInfo.PropertyType == typeof(float?) |
                    PropertyInfo.PropertyType == typeof(double) | PropertyInfo.PropertyType == typeof(double?) |
                    PropertyInfo.PropertyType == typeof(decimal) | PropertyInfo.PropertyType == typeof(decimal?) |
                    PropertyInfo.PropertyType == typeof(Decimal) | PropertyInfo.PropertyType == typeof(Decimal?) |
                    PropertyInfo.PropertyType == typeof(DateTime) | PropertyInfo.PropertyType == typeof(DateTime?) |
                    PropertyInfo.PropertyType == typeof(DateTimeOffset) | PropertyInfo.PropertyType == typeof(DateTimeOffset?) |
                    PropertyInfo.PropertyType == typeof(sbyte) | PropertyInfo.PropertyType == typeof(sbyte?) |
                    PropertyInfo.PropertyType == typeof(SByte) | PropertyInfo.PropertyType == typeof(SByte?) |
                    PropertyInfo.PropertyType == typeof(short) | PropertyInfo.PropertyType == typeof(short?) |
                    PropertyInfo.PropertyType == typeof(byte) | PropertyInfo.PropertyType == typeof(byte?) |
                    PropertyInfo.PropertyType == typeof(sbyte) | PropertyInfo.PropertyType == typeof(sbyte?) |
                    PropertyInfo.PropertyType == typeof(SByte) | PropertyInfo.PropertyType == typeof(SByte?) |
                    PropertyInfo.PropertyType == typeof(ushort) | PropertyInfo.PropertyType == typeof(ushort?) |
                    PropertyInfo.PropertyType == typeof(uint) | PropertyInfo.PropertyType == typeof(uint?) |
                    PropertyInfo.PropertyType == typeof(ulong) | PropertyInfo.PropertyType == typeof(ulong?) |
                    PropertyInfo.PropertyType == typeof(char) | PropertyInfo.PropertyType == typeof(char?) |
                    PropertyInfo.PropertyType == typeof(Char) | PropertyInfo.PropertyType == typeof(Char?) |
                    PropertyInfo.PropertyType == typeof(Boolean) | PropertyInfo.PropertyType == typeof(Boolean?) |
                    PropertyInfo.PropertyType == typeof(bool) | PropertyInfo.PropertyType == typeof(bool?)
                        )
                    {
                        EntidadeDetino.GetType().GetProperty(PropertyInfo.Name).SetValue(EntidadeDetino, PropertyInfo.GetValue(objetcOrigem));
                    }
                }

                return EntidadeDetino;

            }
            catch (Exception erro)
            {
                throw new Exception(erro.Message);
            }
        }

        protected virtual TEntity CopyToKey<TEntity>(TEntity objetcOrigem, TEntity EntidadeDetino) where TEntity : class
        {
            try
            {
                //foreach (var PropertyInfo in objetcOrigem.GetType().GetProperties())
                //{
                //var Keys = PropertyInfo.GetCustomAttributes(typeof(KeyAttribute), true);

                var Keys = _context.Model
                            .FindEntityType(typeof(TEntity))
                            .FindPrimaryKey().Properties
                            //.Select(x => x.Name)
                            .ToList();

                if (Keys != null && Keys.Count > 0)
                {
                    foreach (var item in Keys)
                    {
                        EntidadeDetino
                            .GetType()
                            .GetProperty(item.Name)
                            .SetValue(EntidadeDetino, objetcOrigem.GetType().GetProperty(item.Name).GetValue(objetcOrigem));
                    }
                }

                //if (objetcOrigem.GetType().GetProperty("DataCadastro") != null)
                //{
                //    EntidadeDetino.GetType().GetProperty("DataCadastro").SetValue(EntidadeDetino, objetcOrigem.GetType().GetProperty("DataCadastro").GetValue(objetcOrigem));
                //}
                //if (objetcOrigem.GetType().GetProperty("Ativo") != null)
                //{
                //    EntidadeDetino.GetType().GetProperty("Ativo").SetValue(EntidadeDetino, objetcOrigem.GetType().GetProperty("Ativo").GetValue(objetcOrigem));
                //}

                return EntidadeDetino;

            }
            catch (Exception erro)
            {
                throw new Exception(erro.Message);
            }
        }

        protected virtual TEntity CloneEntidadePropertysNotInsert<TEntity>(TEntity objetcOrigem, TEntity EntidadeDetino) where TEntity : class
        {
            try
            {
                foreach (var PropertyInfo in objetcOrigem.GetType().GetProperties())
                {
                    // var Keys = PropertyInfo.GetCustomAttributes(typeof(KeyAttribute), true);

                    var Keys = _context.Model.FindEntityType(typeof(TEntity)).FindPrimaryKey().Properties
                          .Select(x => x.Name).ToList();

                    if (Keys.Count() > 0)
                        continue;

                    if (PropertyInfo.PropertyType == typeof(string) | PropertyInfo.PropertyType == typeof(String) |
                    PropertyInfo.PropertyType == typeof(long) | PropertyInfo.PropertyType == typeof(long?) |
                    PropertyInfo.PropertyType == typeof(int) | PropertyInfo.PropertyType == typeof(int?) |
                    PropertyInfo.PropertyType == typeof(Int16) | PropertyInfo.PropertyType == typeof(Int16?) |
                    PropertyInfo.PropertyType == typeof(Int32) | PropertyInfo.PropertyType == typeof(Int32?) |
                    PropertyInfo.PropertyType == typeof(Int64) | PropertyInfo.PropertyType == typeof(Int64?) |
                    PropertyInfo.PropertyType == typeof(float) | PropertyInfo.PropertyType == typeof(float?) |
                    PropertyInfo.PropertyType == typeof(double) | PropertyInfo.PropertyType == typeof(double?) |
                    PropertyInfo.PropertyType == typeof(decimal) | PropertyInfo.PropertyType == typeof(decimal?) |
                    PropertyInfo.PropertyType == typeof(Decimal) | PropertyInfo.PropertyType == typeof(Decimal?) |
                    PropertyInfo.PropertyType == typeof(DateTime) | PropertyInfo.PropertyType == typeof(DateTime?) |
                    PropertyInfo.PropertyType == typeof(DateTimeOffset) | PropertyInfo.PropertyType == typeof(DateTimeOffset?) |
                    PropertyInfo.PropertyType == typeof(sbyte) | PropertyInfo.PropertyType == typeof(sbyte?) |
                    PropertyInfo.PropertyType == typeof(SByte) | PropertyInfo.PropertyType == typeof(SByte?) |
                    PropertyInfo.PropertyType == typeof(short) | PropertyInfo.PropertyType == typeof(short?) |
                    PropertyInfo.PropertyType == typeof(byte) | PropertyInfo.PropertyType == typeof(byte?) |
                    PropertyInfo.PropertyType == typeof(sbyte) | PropertyInfo.PropertyType == typeof(sbyte?) |
                    PropertyInfo.PropertyType == typeof(SByte) | PropertyInfo.PropertyType == typeof(SByte?) |
                    PropertyInfo.PropertyType == typeof(ushort) | PropertyInfo.PropertyType == typeof(ushort?) |
                    PropertyInfo.PropertyType == typeof(uint) | PropertyInfo.PropertyType == typeof(uint?) |
                    PropertyInfo.PropertyType == typeof(ulong) | PropertyInfo.PropertyType == typeof(ulong?) |
                    PropertyInfo.PropertyType == typeof(char) | PropertyInfo.PropertyType == typeof(char?) |
                    PropertyInfo.PropertyType == typeof(Char) | PropertyInfo.PropertyType == typeof(Char?) |
                    PropertyInfo.PropertyType == typeof(Boolean) | PropertyInfo.PropertyType == typeof(Boolean?) |
                    PropertyInfo.PropertyType == typeof(bool) | PropertyInfo.PropertyType == typeof(bool?)
                        )
                        continue;
                    else
                    {
                        EntidadeDetino.GetType().GetProperty(PropertyInfo.Name).SetValue(EntidadeDetino, PropertyInfo.GetValue(objetcOrigem));
                    }
                }

                return EntidadeDetino;

            }
            catch (Exception erro)
            {
                throw new Exception(erro.Message);
            }
        }

        #endregion

    }
}
