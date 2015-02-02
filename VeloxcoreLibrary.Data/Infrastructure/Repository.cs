using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VeloxcoreLibrary.Model;

namespace VeloxcoreLibrary.Data.Infrastructure
{
	public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
	{
		#region Members

		internal DbSet<TEntity> dbSet;
		private DataBaseContext _context;

		#endregion

		#region Constructor

		public Repository(DataBaseContext context)
		{
			dbSet = context.Set<TEntity>();
			_context = context;
		}

		#endregion

		#region Public Methods
		
		public virtual IEnumerable<TEntity> GetAll()
		{
			return GetRecords();
		}

		public virtual TEntity GetByID(object id)
		{
			return dbSet.Find(id);
		}

		public virtual void Insert(TEntity entity)
		{
			dbSet.Add(entity);
		}

		public virtual void Delete(object id)
		{
			TEntity entityToDelete = dbSet.Find(id);
			Delete(entityToDelete);
		}

		public virtual void Update(TEntity entityToUpdate)
		{
			dynamic entry = _context.Entry<TEntity>(entityToUpdate);

			if (entry.State == EntityState.Detached)
			{
				TEntity attachedEntity = dbSet.Find(GetKey(entityToUpdate));

				// You need to have access to key
				if ((attachedEntity != null))
				{
					dynamic attachedEntry = _context.Entry(attachedEntity);
					attachedEntry.CurrentValues.SetValues(entityToUpdate);
				}
				else
				{
					// This should attach entity
					entry.State = EntityState.Modified;
				}
			}
		}

		#endregion

		#region Protected Methods

		protected virtual IEnumerable<TEntity> GetRecords(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>,
			IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
		{
			IQueryable<TEntity> query = dbSet;

			if (filter != null)
			{
				query = query.Where(filter);
			}

			foreach (string includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
			{
				query = query.Include(includeProperty);
			}

			if (orderBy != null)
			{
				return orderBy(query).ToList();
			}
			else
			{
				return query.ToList();
			}
		}

		protected virtual PagedResult<TEntity> GetPage(int pageIndex, int pageSize, Func<IQueryable<TEntity>,
			IOrderedQueryable<TEntity>> orderBy, Expression<Func<TEntity, bool>> filter = null, string includeProperties = "")
		{
			IQueryable<TEntity> query = dbSet;

			if (filter != null)
			{
				query = query.Where(filter);
			}

			foreach (string includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
			{
				query = query.Include(includeProperty);
			}

			return new PagedResult<TEntity>
			{
				Results = orderBy(query).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList(),
				RowCount = query.Count()
			};
		}

		#endregion

		#region Abstract Methods

		protected abstract object[] GetKey(TEntity entity);

		#endregion
	}
}
