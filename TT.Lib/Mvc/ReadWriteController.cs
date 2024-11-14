using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TT.Lib.DataAccess;

namespace TT.Lib.Mvc
{
    ///// <summary>
    ///// Defines methods for a read only business logic layer service.
    ///// </summary>
    ///// <typeparam name="TEntity">The entity type.</typeparam>
    //public interface IReadService<TEntity>
    //{
    //    /// <summary>
    //    /// Returns all <typeparamref name="TEntity"/>s.
    //    /// </summary>
    //    /// <returns>All <typeparamref name="TEntity"/>s.</returns>
    //    Task<IEnumerable<TEntity>> Get();

    //    /// <summary>
    //    /// Get the <typeparamref name="TEntity"/> by the given <paramref name="key"/> parameter.
    //    /// </summary>
    //    /// <param name="keyValues">The keys.</param>
    //    /// <returns>The <typeparamref name="TEntity"/> for the given <paramref name="key"/></returns>
    //    Task<TEntity> GetByKey(params object[] keyValues);
    //}

    ///// <summary>
    ///// Defines methods for a read write business logic layer service.
    ///// </summary>
    ///// <typeparam name="TEntity">The entity type.</typeparam>
    //public interface IReadWriteService<TEntity> : IReadService<TEntity>
    //{
    //    /// <summary>
    //    /// Adds an <typeparamref name="TEntity"/> to the <see cref="IReadWriteService{TEntity}"/>.
    //    /// </summary>
    //    /// <param name="entity">The <typeparamref name="TEntity"/> to  add to the <see cref="IReadWriteService{TEntity}"/></param>
    //    /// <returns>The added <typeparamref name="TEntity"/>.</returns>
    //    Task<TEntity> Add(TEntity entity);

    //    /// <summary>
    //    /// Adds <typeparamref name="TEntity"/>s to the <see cref="IReadWriteService{TEntity}"/>.
    //    /// </summary>
    //    /// <param name="entities">The <typeparamref name="TEntity"/>s to add to the <see cref="IReadWriteService{TEntity}"/>.</param>
    //    /// <returns>The added <typeparamref name="TEntity"/>s.</returns>
    //    Task<IEnumerable<TEntity>> AddRange(IEnumerable<TEntity> entities);

    //    /// <summary>
    //    /// Update an <typeparamref name="TEntity"/> to the <see cref="IReadWriteService{TEntity}"/>.
    //    /// </summary>
    //    /// <param name="entity">The <typeparamref name="TEntity"/> to update to the <see cref="IReadWriteService{TEntity}"/>.</param>
    //    /// <returns>The updated <typeparamref name="TEntity"/></returns>
    //    Task Update(TEntity entity);

    //    /// <summary>
    //    /// Remove a <typeparamref name="TEntity"/> from the <see cref="IReadWriteService{TEntity}"/>.
    //    /// </summary>
    //    /// <param name="entity">The <typeparamref name="TEntity"/> to remove from the <see cref="IReadWriteService{TEntity}"/>.</param>
    //    Task Remove(TEntity entity);

    //    /// <summary>
    //    /// Remove a <typeparamref name="TEntity"/> from the <see cref="IReadWriteService{TEntity}"/> by the given key / keys/>.
    //    /// </summary>
    //    Task RemoveByKey(params object[] keyValues);
    //}

    /// <summary>
    /// Provides an <see cref="ReadController{TEntity}"/> implementation with an underlying <see cref="IReadService{TEntity}"/> for business logic layer.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public abstract class ReadController<TEntity, TKey> : ControllerBase
        where TEntity : class
        where TKey : IComparable<TKey>
    {
        protected readonly IReadService<TEntity> readService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadController{TEntity}"/> class.
        /// Initalizes a new instance of <see cref="ReadController{TEntity}"/> class.
        /// </summary>
        /// <param name="readService">The <see cref="IReadService{TEntity}"/> which is used for business logic layer.</param>        
        public ReadController(IReadService<TEntity> readService)
        {
            this.readService = readService;
        }

        /// <summary>
        /// Returns all <typeparamref name="TEntity"/>s.
        /// </summary>
        /// <returns>All <typeparamref name="TEntity"/>s.</returns>
        [HttpGet]
        public virtual async Task<IEnumerable<TEntity>> Get()
        {
            return await this.readService.Get();
        }

        [HttpGet("{id}")]
        public virtual async Task<TEntity> Get(TKey id)
        {
            return await this.readService.GetByKey(id);
        }
    }


    /// <summary>
    /// Provides an <see cref="ReadWriteController{TEntity, TKey}"/> implementation with an underlying <see cref="IReadWriteService{TEntity, TKey}"/> for business logic layer.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public abstract class ReadWriteController<TEntity, TKey> : ReadController<TEntity, TKey>
        where TEntity : class
        where TKey : IComparable<TKey>
    {
        private readonly IReadWriteService<TEntity> readWriteService;

        /// <summary>
        /// Initalizes a new instance of <see cref="ReadWriteController{TEntity}"/> class.
        /// </summary>
        /// <param name="readWriteService">The <see cref="IReadWriteService{TEntity, TKey}"/> which is used for business logic layer.</param>       
        public ReadWriteController(IReadWriteService<TEntity> readWriteService)
            : base(readWriteService)
        {
            this.readWriteService = readWriteService;
        }

        /// <summary>
        /// Update an <typeparamref name="TEntity"/> to the <see cref="ReadWriteController{TEntity, TKey}"/>.
        /// </summary>
        /// <param name="entity">The <typeparamref name="TEntity"/> to update to the <see cref="ReadWriteController{TEntity, TKey}"/>.</param>
        /// <returns>The updated <typeparamref name="TEntity"/></returns>
        [HttpPost]
        public virtual async Task Post([FromBody] TEntity entity)
        {
            await this.readWriteService.Update(entity);
        }

        /// <summary>
        /// Adds <typeparamref name="TEntity"/>s to the <see cref="ReadWriteController{TEntity, TKey}"/>.
        /// </summary>
        /// <param name="entity">The <typeparamref name="TEntity"/>s to add to the <see cref="ReadWriteController{TEntity, TKey}"/>.</param>
        /// <returns>The added <typeparamref name="TEntity"/>s.</returns>
        [HttpPut]
        public virtual async Task<TEntity> Put([FromBody] TEntity entity)
        {
            return await this.readWriteService.Add(entity);
        }

        /// <summary>
        /// Remove a <typeparamref name="TEntity"/> from the <see cref="ReadWriteController{TEntity, TKey}"/> by the given <typeparamref name="TKey"/>.
        /// </summary>
        /// <param name="key">The <typeparamref name="TEntity"/> from the <typeparamref name="TEntity"/>.</param>
        [HttpDelete("{key}")]
        public virtual async Task<TEntity> Delete(TEntity entity)
        {
            await this.readWriteService.Remove(entity);

            return entity;
        }
    }
}
