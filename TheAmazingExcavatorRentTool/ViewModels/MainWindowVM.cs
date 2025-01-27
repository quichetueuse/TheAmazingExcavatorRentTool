namespace TheAmazingExcavatorRentTool.ViewModels;

public class MainWindowVM: BaseVM
{
    
    
    private ExcavatorVM _excavatorvm;
        
    public ExcavatorVM ExcavatorVm
    {
        get { return _excavatorvm; }
        set { _excavatorvm = value; }
    }
    
        
    // private UserVM _uservm;
    //     
    // public UserVM UserVm
    // {
    //     get { return _uservm; }
    //     set { _uservm = value; }
    // }

    public MainWindowVM()
    {
        ExcavatorVm = new ExcavatorVM();
        // _uservm = new UserVM();
    }
}