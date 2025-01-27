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
            dbCon.Server = "localhost";
            dbCon.DatabaseName = "bd_c#";
            dbCon.UserName = "root";
            dbCon.Password = "";

            return dbCon;
        }
    }
}