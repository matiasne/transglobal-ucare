using AutoMapper;
using Google.Cloud.Firestore;
using GQ.Data.Abstractions.Entity;
using GQ.Data.Dto;
using System.Collections;
using UCare.Shared.Infrastructure;

namespace UCare.Infrastructure.Firebase
{
    public class Paging<TEntity> : IPaging where TEntity : class, IEntity, new()
    {
        /// <summary>
        ///
        /// </summary>
        public const string FILTER_IN = "in";
        /// <summary>
        ///
        /// </summary>
        public const string FILTER_NOTIN = "notin";

        /// <summary>
        ///
        /// </summary>
        public const string FILTER_IN_ARRAY = "inarray";

        /// <summary>
        ///
        /// </summary>
        public const string FILTER_CON = "con";

        /// <summary>
        ///
        /// </summary>
        public const string FILTER_T = "=|t";

        /// <summary>
        ///
        /// </summary>
        public const string FILTER_NONE = "x";

        /// <summary>
        ///
        /// </summary>
        public const string FILTER_MATCH = "match";

        public int? PageIndex { get; set; } = 1;
        public int? PageSize { get; set; } = 25;
        public long? PageCount { get; set; } = 0;
        public long? RecordCount { get; set; } = 0;
        public List<GQ.Data.Abstractions.Paging.PagingFilter> Filter { get; set; } = new List<GQ.Data.Abstractions.Paging.PagingFilter>();
        public List<GQ.Data.Abstractions.Paging.PagingOrder> Order { get; set; } = new List<GQ.Data.Abstractions.Paging.PagingOrder>();
        public string[] SearchTextProperties { get; set; }
        public virtual string SearchProperty { get; set; } = "searchText";
        public IEnumerable? Data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Paging()
        {
            PageIndex = 1;
            PageSize = 25;
        }

        private int _maxPageSize = 100;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetMaxPageSize() { return _maxPageSize; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void SetMaxPageSize(int value) { _maxPageSize = value; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        protected virtual Query ApplayFilterNotIn(Query source, GQ.Data.Abstractions.Paging.PagingFilter filter)
        {
            var array = ((IEnumerable<object>)filter.GetValue()).ToList();
            return source.WhereNotIn(filter.Property, array);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        protected virtual Query ApplayFilterIn(Query source, GQ.Data.Abstractions.Paging.PagingFilter filter)
        {
            var array = ((IEnumerable<object>)filter.GetValue()).ToList();
            return source.WhereIn(filter.Property, array);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        protected virtual Query ApplayFilterInArray(Query source, GQ.Data.Abstractions.Paging.PagingFilter filter)
        {
            var array = ((IEnumerable<object>)filter.GetValue()).ToList();
            return source.WhereArrayContainsAny(filter.Property, array);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        protected virtual Query ApplayFilterCon(Query source, GQ.Data.Abstractions.Paging.PagingFilter filter)
        {
            return source.WhereArrayContains(filter.Property, filter.GetValue().ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        protected virtual Query ApplayFilterT(Query source, GQ.Data.Abstractions.Paging.PagingFilter filter)
        {
            if (filter.GetValue().ToString() != "T")
                return source.WhereEqualTo(filter.Property, filter.GetValue());
            return source;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        protected virtual Query ApplayFilterDefault(Query source, GQ.Data.Abstractions.Paging.PagingFilter filter)
        {
            switch (filter.Condition)
            {
                case "=":
                    return source.WhereEqualTo(filter.Property, filter.GetValue());
                case "!=":
                    return source.WhereNotEqualTo(filter.Property, filter.GetValue());
                case ">":
                    return source.WhereGreaterThan(filter.Property, filter.GetValue());
                case ">=":
                case "=>":
                    return source.WhereGreaterThanOrEqualTo(filter.Property, filter.GetValue());
                case "<":
                    return source.WhereLessThan(filter.Property, filter.GetValue());
                case "<=":
                case "=<":
                    return source.WhereLessThanOrEqualTo(filter.Property, filter.GetValue());

            }
            return source;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        protected virtual async Task<Query> ApplayFilter(Query source)
        {
            if (Filter != null)
            {
                //Aplicar Filtro
                if (Filter.Count > 0)
                {
                    foreach (var item in Filter)
                    {
                        if (item.Condition != null)
                        {
                            switch (item.Condition.ToLower().ToString())
                            {
                                case FILTER_NOTIN:
                                    {
                                        source = ApplayFilterNotIn(source, item);
                                        break;
                                    }
                                case FILTER_IN:
                                    {
                                        source = ApplayFilterIn(source, item);
                                        break;
                                    }
                                case FILTER_IN_ARRAY:
                                    {
                                        source = ApplayFilterInArray(source, item);
                                        break;
                                    }
                                case FILTER_CON:
                                    {
                                        source = ApplayFilterCon(source, item);
                                        break;
                                    }
                                case FILTER_T:
                                    {
                                        source = ApplayFilterT(source, item);
                                        break;
                                    }
                                case FILTER_NONE:
                                    {
                                        break;
                                    }
                                default:
                                    {
                                        source = ApplayFilterDefault(source, item);
                                        break;
                                    }
                            }
                        }
                    }
                }

                int total = (await source.Select("Id").GetSnapshotAsync()).Count;
                this.RecordCount = total;
                this.PageCount = total / PageSize;

                if (total % PageSize > 0) PageCount++;
            }

            return source;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        protected virtual Query ApplayOrder(Query source)
        {
            if (Order != null)
            {
                foreach (var item in Order)
                {
                    switch (item.Direction)
                    {
                        case "+":
                            {
                                source = source.OrderBy(item.Property);
                                break;

                            }
                        case "-":
                            {
                                source = source.OrderByDescending(item.Property);
                                break;
                            }
                    }
                }
            }
            return source;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="q"></param>
        /// <param name="properties"></param>
        /// <returns></returns>
        //public virtual Query FilterSearchText([NotNull] string searchText, Query q, params string[] properties)
        //{
        //    if (!string.IsNullOrWhiteSpace(searchText))
        //    {
        //        var value = searchText.ToString();
        //        var values = value.Split(' ');
        //        var search = new StringBuilder();
        //        for (var i = 0; i < values.Length; i++)
        //        {
        //            search.Append("(");
        //            foreach (var prop in properties)
        //            {
        //                search.Append($"{prop}.Contains(@{i}) OR ");
        //            }
        //            search = search.Remove(search.Length - 4, 4);
        //            search.Append(") AND ");
        //        }

        //        var index = search.ToString().LastIndexOf("AND");
        //        if (index > 0)
        //        {
        //            search = search.Remove(search.Length - 5, 5);
        //        }

        //        q = q.(search.ToString(), values);
        //    }
        //    return q;
        //}


        public virtual async Task Apply<TEntityFirebase>(dynamic query, dynamic _mapper)
        {
            Mapper mapper = (Mapper)_mapper;
            var source = query as Query;
            if (source != null)
            {
                if (PageSize > GetMaxPageSize()) PageSize = GetMaxPageSize();

                //var searchFilter = Filter.FirstOrDefault(x => x.Property.ToLower() == SearchProperty.ToLower() && x.Condition.ToLower() == FILTER_NONE);
                //if (searchFilter != null && !string.IsNullOrWhiteSpace((searchFilter.GetValue() ?? "").ToString()) && SearchTextProperties != null && SearchTextProperties.Length > 0)
                //{
                //    source = FilterSearchText((searchFilter.GetValue() ?? "").ToString(), source, SearchTextProperties);
                //}

                source = await ApplayFilter(source);

                source = ApplayOrder(source);

                //Paginamos la lista de datos
                var result = await source.Offset(((PageIndex ?? 1) - 1) * (PageSize ?? 25)).Limit(PageSize ?? 25).GetSnapshotAsync();

                var sourceData = new List<TEntity>();

                foreach (var item in result)
                {
                    sourceData.Add(mapper.Map<TEntity>(item.ConvertTo<TEntityFirebase>()));
                }

                Data = sourceData;
            }
            else
                throw new NotImplementedException();
        }
        public virtual IEnumerator? Convert(List<TEntity> data)
        {
            yield return data;
        }
    }

    public class Paging<TEntity, TDto> : Paging<TEntity> where TEntity : class, IEntity, new() where TDto : Dto<TEntity, TDto>, new()
    {
        public override IEnumerator? Convert(List<TEntity> data)
        {
            yield return new TDto().SetEntity(data);
        }
    }
}