using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsApp.Library
{
    public class UserThemeFilterService : IResultFilter
    {

        private readonly Data.ContactsAppDataContext _data;
        public UserThemeFilterService(Data.ContactsAppDataContext data)
        {
            _data = data;
        }



        public string ThemeNameFromDbForLayout()
        {

            int fakeUserId = 2;
            var userIdInDb = _data.Users.Find(fakeUserId);

            if (userIdInDb == null)
                return "";
            if (userIdInDb.ThemeSelection == null)
                return "";
            if (userIdInDb.ThemeSelection == "Dracula")
                return "dracula-theme";
            if (userIdInDb.ThemeSelection == "atomOneDark")
                return "atom-one-dark-theme";

            return userIdInDb.ThemeSelection.ToString();
        }







        public void OnResultExecuted(ResultExecutedContext context)
        {
            //throw new NotImplementedException();
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            var controller = context.Controller as Controller;
            string themeName = ThemeNameFromDbForLayout();
            if(controller != null)
                controller.ViewData["userTheme"] = themeName;
            ///base.OnResultExecuting(context);
        }
    }
}
