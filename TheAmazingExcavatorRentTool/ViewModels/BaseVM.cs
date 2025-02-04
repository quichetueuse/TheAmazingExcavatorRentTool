using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TheAmazingExcavatorRentTool.Services;

namespace TheAmazingExcavatorRentTool.ViewModels
{
    public class BaseVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        
        protected DB getDbCon()
        {
            var dbCon = new DB();
            if (Session.IsAdmin)
            {
                dbCon.Server = "localhost";
                dbCon.DatabaseName = "heavy_app_e5";
                dbCon.UserName = "excav_admin";
                dbCon.Password = "yX8!fknX2bFLax97";
            } else
            {
                dbCon.Server = "localhost";
                dbCon.DatabaseName = "heavy_app_e5";
                dbCon.UserName = "excav_user";
                dbCon.Password = "UN5CDnVl!3b0nx$T";
            }

            return dbCon;
        }
    }
}