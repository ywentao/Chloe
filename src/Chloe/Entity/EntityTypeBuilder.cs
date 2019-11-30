﻿using Chloe.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Chloe.Entity
{
    public class EntityTypeBuilder<TEntity> : IEntityTypeBuilder<TEntity>
    {
        public EntityTypeBuilder()
        {
            this.EntityType = new EntityType(typeof(TEntity));

            foreach (PrimitiveProperty property in this.EntityType.PrimitiveProperties)
            {
                if (property.Property.Name.ToLower() == "id")
                {
                    /*默认为主键，且自增*/
                    property.IsPrimaryKey = true;

                    if (Utils.IsAutoIncrementType(property.Property.PropertyType))
                        property.IsAutoIncrement = true;
                }
            }
        }
        public EntityType EntityType { get; private set; }

        IEntityTypeBuilder AsNonGenericBuilder()
        {
            return this;
        }

        public IEntityTypeBuilder<TEntity> MapTo(string table)
        {
            this.AsNonGenericBuilder().MapTo(table);
            return this;
        }
        IEntityTypeBuilder IEntityTypeBuilder.MapTo(string table)
        {
            this.EntityType.TableName = table;
            return this;
        }

        public IEntityTypeBuilder<TEntity> HasSchema(string schema)
        {
            this.AsNonGenericBuilder().HasSchema(schema);
            return this;
        }
        IEntityTypeBuilder IEntityTypeBuilder.HasSchema(string schema)
        {
            this.EntityType.SchemaName = schema;
            return this;
        }

        public IEntityTypeBuilder<TEntity> HasAnnotation(object value)
        {
            this.AsNonGenericBuilder().HasAnnotation(value);
            return this;
        }
        IEntityTypeBuilder IEntityTypeBuilder.HasAnnotation(object value)
        {
            if (value == null)
                throw new ArgumentNullException();

            this.EntityType.Annotations.Add(value);
            return this;
        }

        public IEntityTypeBuilder<TEntity> Ignore(Expression<Func<TEntity, object>> property)
        {
            string propertyName = PropertyNameExtractor.Extract(property);
            this.Ignore(propertyName);
            return this;
        }
        public IEntityTypeBuilder Ignore(string property)
        {
            this.EntityType.PrimitiveProperties.RemoveAll(a => a.Property.Name == property);
            return this;
        }
        public IPrimitivePropertyBuilder<TProperty> Property<TProperty>(Expression<Func<TEntity, TProperty>> property)
        {
            string propertyName = PropertyNameExtractor.Extract(property);
            IPrimitivePropertyBuilder<TProperty> propertyBuilder = this.Property(propertyName) as IPrimitivePropertyBuilder<TProperty>;
            return propertyBuilder;
        }
        public IPrimitivePropertyBuilder Property(string property)
        {
            PrimitiveProperty entityProperty = this.EntityType.PrimitiveProperties.FirstOrDefault(a => a.Property.Name == property);

            if (entityProperty == null)
                throw new ArgumentException($"The mapping property list doesn't contain property named '{property}'.");

            IPrimitivePropertyBuilder propertyBuilder = Activator.CreateInstance(typeof(PrimitivePropertyBuilder<>).MakeGenericType(entityProperty.Property.PropertyType), entityProperty) as IPrimitivePropertyBuilder;
            return propertyBuilder;
        }

        public IComplexPropertyBuilder<TProperty> HasOne<TProperty>(Expression<Func<TEntity, TProperty>> property)
        {
            string propertyName = PropertyNameExtractor.Extract(property);
            IComplexPropertyBuilder<TProperty> propertyBuilder = this.HasOne(propertyName) as IComplexPropertyBuilder<TProperty>;
            return propertyBuilder;
        }
        public IComplexPropertyBuilder HasOne(string property)
        {
            PropertyInfo entityProperty = this.GetEntityProperty(property);
            IComplexPropertyBuilder propertyBuilder = Activator.CreateInstance(typeof(ComplexPropertyBuilder<>).MakeGenericType(entityProperty.PropertyType), entityProperty) as IComplexPropertyBuilder;
            return propertyBuilder;
        }

        public ICollectionPropertyBuilder<TProperty> HasMany<TProperty>(Expression<Func<TEntity, TProperty>> property)
        {
            string propertyName = PropertyNameExtractor.Extract(property);
            ICollectionPropertyBuilder<TProperty> propertyBuilder = this.HasMany(propertyName) as ICollectionPropertyBuilder<TProperty>;
            return propertyBuilder;
        }
        public ICollectionPropertyBuilder HasMany(string property)
        {
            PropertyInfo entityProperty = this.GetEntityProperty(property);
            ICollectionPropertyBuilder propertyBuilder = Activator.CreateInstance(typeof(CollectionPropertyBuilder<>).MakeGenericType(entityProperty.PropertyType), entityProperty) as ICollectionPropertyBuilder;
            return propertyBuilder;
        }

        PropertyInfo GetEntityProperty(string property)
        {
            PropertyInfo entityProperty = this.EntityType.Type.GetProperty(property);

            if (entityProperty == null)
                throw new ArgumentException($"Cannot find the property named '{property}'.");

            return entityProperty;
        }
    }
}
