using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Examples.Charge.Infra.Data.Base
{
    public static class DbSetExtension
    {
        public static void AddOrUpdate<T>(this DbSet<T> dbSet, T data) where T : class
        {
            var context = dbSet.GetContext();
            //var ids = context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.Select(x => x.Name);
            var ids = context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.Select(x => x.Name).AsEnumerable();

            var t = typeof(T);
            List<PropertyInfo> keyFields = new List<PropertyInfo>();

            //foreach (var propt in t.GetProperties())
            //{
            //    var keyAttr = ids.Contains(propt.Name);
            //    if (keyAttr)
            //    {
            //        keyFields.Add(propt);
            //    }
            //}

            foreach (var id in ids)
            {
                var keyAttr = t.GetProperty(id);
                if (keyAttr != null)
                {
                    keyFields.Add(keyAttr);
                }
            }


            if (keyFields.Count <= 0)
            {
                throw new Exception($"{t.FullName} does not have a PrimaryKey field Definid. Unable to exec AddOrUpdate call.");
            }

            //var entities = dbSet.AsNoTracking().AsEnumerable();
            var entities = dbSet.AsNoTracking();

            foreach (var keyField in keyFields)
            {
                var keyVal = keyField.GetValue(data);
                //entities = entities.Where(p => EF.Property<string>(p, keyField.Name) == keyVal.ToString());
                entities = entities.Where(p => p.GetType().GetProperty(keyField.Name).GetValue(p).Equals(keyVal)).AsQueryable();
            }

            var dbVal = entities.FirstOrDefault();
            if (dbVal != null)
            {
                //var entry = context.Entry(dbVal).;


                var keys = context.Entry(dbVal).Metadata.FindPrimaryKey().Properties.ToDictionary(x => x.Name, x => x.PropertyInfo.GetValue(data));
                var where = CondicaoKey<T>(keys);

                if (!dbSet.Local.Any(where))
                {
                    context.Entry(dbVal).CurrentValues.SetValues(data);
                    context.Entry(dbVal).State = EntityState.Modified;
                }
                else
                {
                    dbSet.Local.Remove(dbVal);

                    context.Entry(dbVal).CurrentValues.SetValues(data);
                    context.Entry(dbVal).State = EntityState.Modified;

                    //entry.State = EntityState.Modified;
                    //var attachedEntry = context.Entry(data);
                    //attachedEntry.CurrentValues.SetValues(data);
                    //context.Entry(dbVal).Reload();
                    //context.Entry(dbVal).CurrentValues.SetValues(data);
                    //entry.CurrentValues.SetValues(data);
                }

                return;
            }
            dbSet.Add(data);
        }

        public static void AddOrUpdate<T>(this DbSet<T> dbSet, Expression<Func<T, object>> key, T data) where T : class
        {
            var context = dbSet.GetContext();
            var ids = context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.Select(x => x.Name);
            var t = typeof(T);
            var keyObject = key.Compile()(data);
            PropertyInfo[] keyFields = keyObject.GetType().GetProperties().Select(p => t.GetProperty(p.Name)).ToArray();
            if (keyFields == null)
            {
                throw new Exception($"{t.FullName} does not have a KeyAttribute field. Unable to exec AddOrUpdate call.");
            }
            var keyVals = keyFields.Select(p => p.GetValue(data));
            var entities = dbSet.AsNoTracking().ToList();
            int i = 0;
            foreach (var keyVal in keyVals)
            {
                entities = entities.Where(p => p.GetType().GetProperty(keyFields[i].Name).GetValue(p).Equals(keyVal)).ToList();
                i++;
            }
            if (entities.Any())
            {
                var dbVal = entities.FirstOrDefault();
                var keyAttrs =
                    data.GetType().GetProperties().Where(p => ids.Contains(p.Name)).ToList();
                if (keyAttrs.Any())
                {
                    foreach (var keyAttr in keyAttrs)
                    {
                        keyAttr.SetValue(data,
                            dbVal.GetType()
                                .GetProperties()
                                .FirstOrDefault(p => p.Name == keyAttr.Name)
                                .GetValue(dbVal));
                    }
                    context.Entry(dbVal).CurrentValues.SetValues(data);
                    context.Entry(dbVal).State = EntityState.Modified;
                    return;
                }
            }
            dbSet.Add(data);
        }

        private static string CamposChavePrimaria<TEntity>(TEntity Entidade) where TEntity : class
        {
            var Properties = Entidade.GetType().GetProperties();

            var Keys = new Dictionary<int, object>();
            string Sql = string.Empty;

            foreach (var Property in Properties)
            {
                var Chave = Property.GetCustomAttributes(typeof(KeyAttribute), false);
                var Order = Property.GetCustomAttributes(typeof(ColumnAttribute), false);
                int OrdemChave = 0;
                object ValorKey = 0;



                if (Order.Count() > 0)
                {
                    OrdemChave = ((ColumnAttribute)Order[0]).Order;
                }

                if (Chave.Count() > 0)
                {
                    ValorKey = Property.Name;
                    Keys.Add(OrdemChave, ValorKey);
                }
            }

            int Contador = 0;
            foreach (var item in Keys)
            {
                if (Contador == 0)
                    Sql = string.Format("{0} = (@{1})", item.Value, item.Key);
                else
                    Sql = Sql + string.Format(" && {0} = (@{1})", item.Value, item.Key);
                Contador++;
            }


            return Sql;
        }
        private static object[] ChavePrimaria<TEntity>(TEntity Entidade, bool ValoresString = false) where TEntity : class
        {
            var Properties = Entidade.GetType().GetProperties();

            var Keys = new Dictionary<int, object>();

            foreach (var Property in Properties)
            {
                var Chave = Property.GetCustomAttributes(typeof(KeyAttribute), false);
                var Order = Property.GetCustomAttributes(typeof(ColumnAttribute), false);
                int OrdemChave = 0;
                object ValorKey = 0;


                if (Order.Count() > 0)
                {
                    OrdemChave = ((ColumnAttribute)Order[0]).Order;
                }

                if (Chave.Count() > 0)
                {
                    var Valor = Property.GetValue(Entidade);

                    if (ValoresString)
                        ValorKey = Valor.ToString();
                    else
                        ValorKey = Valor;

                    Keys.Add(OrdemChave, ValorKey);

                }
            }

            var Lima = Keys.OrderBy(x => x.Key).Select(x => x.Value).ToArray();

            return Lima;
        }
        private static object GetPropertySymp(this object Entidade, string Field)
        {
            if (Entidade == null)
            {
                return Entidade;
            }

            var campo = Field.Split('.');
            object entidade = Entidade;

            if (campo.Count() > 1)
            {
                for (int i = 0; i < campo.Length; i++)
                {
                    entidade = entidade.GetType().GetProperty(campo[i]).GetValue(entidade);
                }
            }
            else
            {
                entidade = Entidade.GetType().GetProperty(Field).GetValue(Entidade);
            }

            return entidade;
        }
        public static Func<T, bool> CondicaoKey<T>(Dictionary<string, object> Dictionary)
        {
            Func<T, bool> r;

            var list = Dictionary.ToArray();

            switch (list.Count())
            {
                case 1:
                    return r = x => x.GetPropertySymp(list[0].Key).Equals(list[0].Value);
                case 2:
                    return r = x => x.GetPropertySymp(list[0].Key).Equals(list[0].Value) ||
                                    x.GetPropertySymp(list[1].Key).Equals(list[1].Value);
                case 3:
                    return r = x => x.GetPropertySymp(list[0].Key).Equals(list[0].Value) ||
                                    x.GetPropertySymp(list[1].Key).Equals(list[1].Value) ||
                                    x.GetPropertySymp(list[2].Key).Equals(list[2].Value);
                case 4:
                    return r = x => x.GetPropertySymp(list[0].Key).Equals(list[0].Value) ||
                                    x.GetPropertySymp(list[1].Key).Equals(list[1].Value) ||
                                    x.GetPropertySymp(list[2].Key).Equals(list[2].Value) ||
                                    x.GetPropertySymp(list[3].Key).Equals(list[3].Value);
                case 5:
                    return r = x => x.GetPropertySymp(list[0].Key).Equals(list[0].Value) ||
                                    x.GetPropertySymp(list[1].Key).Equals(list[1].Value) ||
                                    x.GetPropertySymp(list[2].Key).Equals(list[2].Value) ||
                                    x.GetPropertySymp(list[3].Key).Equals(list[3].Value) ||
                                    x.GetPropertySymp(list[4].Key).Equals(list[4].Value);
                case 6:
                    return r = x => x.GetPropertySymp(list[0].Key).Equals(list[0].Value) ||
                                    x.GetPropertySymp(list[1].Key).Equals(list[1].Value) ||
                                    x.GetPropertySymp(list[2].Key).Equals(list[2].Value) ||
                                    x.GetPropertySymp(list[3].Key).Equals(list[3].Value) ||
                                    x.GetPropertySymp(list[4].Key).Equals(list[4].Value) ||
                                    x.GetPropertySymp(list[5].Key).Equals(list[5].Value);
                case 7:
                    return r = x => x.GetPropertySymp(list[0].Key).Equals(list[0].Value) ||
                                    x.GetPropertySymp(list[1].Key).Equals(list[1].Value) ||
                                    x.GetPropertySymp(list[2].Key).Equals(list[2].Value) ||
                                    x.GetPropertySymp(list[3].Key).Equals(list[3].Value) ||
                                    x.GetPropertySymp(list[4].Key).Equals(list[4].Value) ||
                                    x.GetPropertySymp(list[5].Key).Equals(list[5].Value) ||
                                    x.GetPropertySymp(list[6].Key).Equals(list[6].Value);
                case 8:
                    return r = x => x.GetPropertySymp(list[0].Key).Equals(list[0].Value) ||
                                    x.GetPropertySymp(list[1].Key).Equals(list[1].Value) ||
                                    x.GetPropertySymp(list[2].Key).Equals(list[2].Value) ||
                                    x.GetPropertySymp(list[3].Key).Equals(list[3].Value) ||
                                    x.GetPropertySymp(list[4].Key).Equals(list[4].Value) ||
                                    x.GetPropertySymp(list[5].Key).Equals(list[5].Value) ||
                                    x.GetPropertySymp(list[6].Key).Equals(list[6].Value) ||
                                    x.GetPropertySymp(list[7].Key).Equals(list[7].Value);
                case 9:
                    return r = x => x.GetPropertySymp(list[0].Key).Equals(list[0].Value) ||
                                    x.GetPropertySymp(list[1].Key).Equals(list[1].Value) ||
                                    x.GetPropertySymp(list[2].Key).Equals(list[2].Value) ||
                                    x.GetPropertySymp(list[3].Key).Equals(list[3].Value) ||
                                    x.GetPropertySymp(list[4].Key).Equals(list[4].Value) ||
                                    x.GetPropertySymp(list[5].Key).Equals(list[5].Value) ||
                                    x.GetPropertySymp(list[6].Key).Equals(list[6].Value) ||
                                    x.GetPropertySymp(list[7].Key).Equals(list[7].Value) ||
                                    x.GetPropertySymp(list[8].Key).Equals(list[8].Value);
                case 10:
                    return r = x => x.GetPropertySymp(list[0].Key).Equals(list[0].Value) ||
                                    x.GetPropertySymp(list[1].Key).Equals(list[1].Value) ||
                                    x.GetPropertySymp(list[2].Key).Equals(list[2].Value) ||
                                    x.GetPropertySymp(list[3].Key).Equals(list[3].Value) ||
                                    x.GetPropertySymp(list[4].Key).Equals(list[4].Value) ||
                                    x.GetPropertySymp(list[5].Key).Equals(list[5].Value) ||
                                    x.GetPropertySymp(list[6].Key).Equals(list[6].Value) ||
                                    x.GetPropertySymp(list[7].Key).Equals(list[7].Value) ||
                                    x.GetPropertySymp(list[8].Key).Equals(list[8].Value) ||
                                    x.GetPropertySymp(list[9].Key).Equals(list[9].Value);
                default:
                    break;
            }

            return null;
        }
    }

    public static class HackyDbSetGetContextTrick
    {
        public static DbContext GetContext<TEntity>(this DbSet<TEntity> dbSet)
            where TEntity : class
        {
            return (DbContext)dbSet
                .GetType().GetTypeInfo()
                .GetField("_context", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(dbSet);
        }
    }
}
