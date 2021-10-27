using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpUtilities.Manipulations
{
    public class ListManipulation
    {
        /// <summary>
        /// Defines a model that will contain all list pagination metadata
        /// </summary>
        /// <typeparam name="ListItemType">defines the type of every list item</typeparam>
        public class PaginationModel<ListItemType>
        {
            public List<ListItemType> PaginatedList { get; set; }
            public int RequestedPage { get; set; }
            public int NumberOfPages { get; set; }
            public int NumberOfItemsPerPage { get; set; }
            public bool ListWasPaginated { get; set; }
        }

        /// <summary>
        /// Use to randomly shuffle the contents of a list, to produce a completely random order
        /// </summary>
        /// <typeparam name="ListItemType">defines the type of every list item</typeparam>
        /// <param name="list" type="List<ListItemType>">defines the list that needs to be shuffled</param>
        public static void ShuffleList<ListItemType>(List<ListItemType> list)
        {
            Random randomGenerator = new Random();
            int counter = list.Count;
            while (counter > 1)
            {
                counter--;
                int randomIndex = randomGenerator.Next(counter + 1);
                ListItemType randomListItem = list[randomIndex];
                list[randomIndex] = list[counter];
                list[counter] = randomListItem;
            }
        }

        /// <summary>
        /// Use to paginate a list, and get all of the pagination metadata
        /// </summary>
        /// <typeparam name="ListItemType">defines the type of every list item</typeparam>
        /// <param name="list" type="List<ListItemType>">defines the list that needs to be paginated</param>
        /// <param name="numberOfItemsPerPage" type="int">defines the amount of items per page</param>
        /// <param name="page" type="int">defines the desired pagination page</param>
        /// <returns>PaginationModel<ListItemType> - the desired list paginated with all its metadata</returns>
        public static PaginationModel<ListItemType> PaginateList<ListItemType>(List<ListItemType> list, int numberOfItemsPerPage, int page)
        {
            int numberOfPages = (int)Math.Ceiling((double)list.Count / numberOfItemsPerPage);
            return new PaginationModel<ListItemType>()
            {
                NumberOfItemsPerPage = numberOfItemsPerPage,
                RequestedPage = page,
                NumberOfPages = numberOfPages,
                PaginatedList = (page <= 0 || page>numberOfPages) ? new List<ListItemType>() : list.Skip(numberOfItemsPerPage * (page - 1)).Take(numberOfItemsPerPage).ToList(), 
                ListWasPaginated = !(page <= 0 || page > numberOfPages)
            };
        }


    }
}
