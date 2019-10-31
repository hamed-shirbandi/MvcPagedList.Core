using MvcPagedList.Core.Example.Domain;
using MvcPagedList.Core.Example.Enums;
using MvcPagedList.Core.Example.Service.Users.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace MvcPagedList.Core.Example.Service.Users
{
    public class UserService : IUserService
    {

        private List<User> users;

        public UserService()
        {
            users = new List<User>();
        }


        /// <summary>
        /// 
        /// </summary>
        public void Create(UserInput input)
        {
            users.Add(
                new User
                {
                    Id = input.Id,
                    Family = input.Family,
                    Name = input.Name,
                    AddDate = input.AddDate
                });
        }



        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<UserOutput> Search(int page, int recordsPerPage, string term, SortBy sortBy, SortOrder sortOrder, out int pageSize, out int TotalItemCount)
        {
            var queryable = users.AsQueryable();


            #region by term



            if (!string.IsNullOrEmpty(term))
            {
                queryable = queryable.Where(c => c.Family.Contains(term) || c.Name.Contains(term));

            }



            #endregion

            #region ordering



            switch (sortBy)
            {
                case SortBy.AddDate:
                    queryable = sortOrder == SortOrder.Asc ? queryable.OrderBy(u => u.AddDate) : queryable.OrderByDescending(u => u.AddDate);
                    break;
                case SortBy.DisplayName:
                    queryable = sortOrder == SortOrder.Asc ? queryable.OrderBy(u => u.Name).ThenBy(u => u.Family) : queryable.OrderByDescending(u => u.Name).ThenByDescending(u => u.Family);
                    break;
                default:
                    break;
            }




            #endregion

            #region calculate skipe count


            TotalItemCount = queryable.Count();
            pageSize = (int)Math.Ceiling((double)TotalItemCount / recordsPerPage);

            page = page > pageSize || page < 1 ? 1 : page;

            var skiped = (page - 1) * recordsPerPage;


            #endregion

            #region take records



            queryable = queryable.Skip(skiped).Take(recordsPerPage);


            #endregion



            return queryable.Select(u => new UserOutput
            {
                Id = u.Id,
                AddDate = u.AddDate.ToShortDateString(),
                Name = u.Name,
                Family = u.Family,

            }).ToList();
        }
    }
}